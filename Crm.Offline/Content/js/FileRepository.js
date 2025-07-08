/// <reference path="../../../../Content/js/jaydata/jaydataproviders/IndexedDbProProvider.js" />
;
(function($data) {
	// local only column
	window.Helper.Database.registerTable("Main_FileResource", {
		IsSynced: {
			type: "$data.Boolean",
			defaultValue: false
		},
		SyncDate: {
			type: "$data.Date",
			defaultValue: null
		}
	});
	// prevent jaydata from converting blobs
	var blobToString = $data.Blob.toString;
	$data.Blob.toString = function(value) {
		if (!!value && value instanceof Blob) {
			return value;
		}
		return blobToString.apply(this, arguments);
	}
	$data.Container.registerConverter("$data.Blob", { "default": function(value) { return value; } });

	$data.Entity.extend("File", {
		Id: { type: "$data.Guid", key: true, computed: false },
		File: { type: "$data.Blob" }
	});
	window.FilesDatabase = $data.EntityContext.extend("FilesDatabase", {
		File: { type: $data.EntitySet, elementType: File }
	});
	var filesDatabase = new window.FilesDatabase({
		provider: "indexedDb",
		databaseName: window.Helper.Database.getStoragePrefix() + "Files"
	});

	window.FileRepository = function() {
	};

	window.FileRepository.prototype.init = function() {
		var fileRepository = this;
		var d = new $.Deferred();
		window.Helper.Sync.init().then(function (database) {
			fileRepository.database = database;
			filesDatabase
				.onReady(function() {
					d.resolve();
				});
		});
		return d.promise();
	};

	window.FileRepository.prototype.downloadFileResources = function(fileResources, skipErrors) {
		var fileRepository = this;
		var d = new $.Deferred();
		var failed = [];
		var successful = [];
		fileRepository.getNewOrUpdatedIds(fileResources).then(function(ids) {
			window.async.eachSeries(fileResources,
				function(fileResource, cb) {
					var downloadRequired = ids[window.ko.unwrap(fileResource.Id)] === true;
					if (downloadRequired) {
						fileRepository.downloadFile(fileResource).then(function () {
							fileResource = fileRepository.database.attachOrGet(fileResource);
							fileResource.IsSynced = true;
							fileResource.SyncDate = fileResource.ModifyDate;
							successful.push(fileResource);
							cb();
						}).fail(function (e) {
							window.Log.warn("downloading / storing failed");
							window.Log.warn(e);
							failed.push(fileResource);
							cb(skipErrors ? undefined : e);
						});
					} else if (fileResource.IsSynced === false) {
						fileResource = fileRepository.database.attachOrGet(fileResource);
						fileResource.IsSynced = true;
						fileResource.SyncDate = fileResource.ModifyDate;
						successful.push(fileResource);
						cb();
					} else {
						successful.push(fileResource);
						cb();
					}
				},
				function (cbError) {
					if (cbError) {
						d.reject(cbError);
						return;
					}
					fileRepository.database.saveChanges().then(function() {
						window.database.cacheReset();
						d.resolve(successful, failed);
					}).fail(function(e) {
						window.Log.error("saving file resources failed");
						d.reject(e);
					});
				});
		});
		return d.promise();
	};

	window.FileRepository.prototype.deleteExpiredFiles = function(existingFileIdsQueryFactory) {
		var pageSize = 10;
		return filesDatabase.File.count().then(function(count) {
			var d = new $.Deferred();
			if (count === 0) {
				return d.resolve().promise();
			}
			var skip = 0;
			var queue = window.async.queue(function(_, cb) {
				filesDatabase.File.orderBy("it.Id").skip(skip).take(pageSize).toArray().then(function(files) {
					var ids = files.map(function(x) { return x.Id });
					return existingFileIdsQueryFactory(ids).toArray().then(function(existingIds) {
						return { existingIds: existingIds, files: files };
					});
				}).then(function(result) {
					var existingIds = result.existingIds.reduce(function(m, x) { m[x] = true; return m; }, {});
					var expiredFiles = result.files.filter(function(x) { return !existingIds[x.Id] });
					expiredFiles.forEach(function(x) { filesDatabase.File.remove(x); });
					skip += result.files.length - expiredFiles.length;
					return filesDatabase.saveChanges();
				}).then(function() {
					cb();
				});
			});
			queue.drain = d.resolve;
			queue.push(Array(Math.ceil(count / pageSize)));
			return d.promise();
		});
	};

	window.FileRepository.prototype.deleteFiles = function () {
		var fileRepository = this;
		return fileRepository.database.Main_FileResource.filter(function(it) {
			return it.IsSynced === true;
		}).forEach(function(fileResource) {
			fileRepository.database.attachOrGet(fileResource);
			fileResource.IsSynced = false;
			fileResource.SyncDate = null;
		}).then(function() {
			return fileRepository.database.saveChanges();
		}).then(function() {
			window.database.cacheReset();
			return filesDatabase.File.forEach(function(file) {
				filesDatabase.File.remove(file);
			});
		}).then(function() {
			return filesDatabase.saveChanges();
		});
	};

	window.FileRepository.prototype.downloadFile = async function(fileResource) {
		const fileRepository = this;
		const id = window.ko.unwrap(fileResource.Id);
		let url = await window.Helper.FileResource.getDownloadLink(fileResource);
		let res = await fetch(url, {method: "GET"});
		let result = await res.blob();
		await fileRepository.storeFile(id, result);
	};

	window.FileRepository.prototype.getFile = function(id) {
		return filesDatabase
			.File
			.find(id);
	};

	window.FileRepository.prototype.getNewOrUpdatedIds = function(fileResources) {
		var ids = fileResources.reduce(function(map, x) {
			map[window.ko.unwrap(x.Id)] = {
				ModifyDate: window.ko.unwrap(x.ModifyDate),
				SyncDate: window.ko.unwrap(x.SyncDate)
			};
			return map;
		}, {});
		return filesDatabase.File
			.filter(function(x) { return x.Id in this.ids; }, { ids: Object.keys(ids) })
			.map(function(x) { return x.Id; })
			.toArray()
			.then(function(existingIds) {
				existingIds.forEach(function(id) {
					ids[id].Existing = true;
				});
				return Object.keys(ids).reduce(function(o, x) {
					if (!x.Existing || x.ModifyDate !== x.SyncDate) {
						o[x] = true;
					}
					return o;
				}, {});
			});
	};

	window.FileRepository.prototype.storeFile = async function(id, blob) {
		try {
			let file = await this.getFile(id);
			filesDatabase.File.attach(file);
			file.File = blob;
		} catch {
			let file = filesDatabase.File.File.create({ Id: id, File: blob });
			filesDatabase.File.add(file);
		}
		await filesDatabase.saveChanges();
	};

	window.FileRepository.prototype.getFreeDiskSpace = function() {
		return (navigator.storage && navigator.storage.estimate) ? navigator.storage.estimate().then(function(estimation) {
			return estimation.quota - estimation.usage;
		}) : new Promise(function(resolve, reject) { resolve(null); });
	};

	window.FileRepository.prototype.getFiles = function() {
		return filesDatabase.File.toArray();
	};
})($data);