/// <reference path="../../../../Content/js/namespace.js" />
/// <reference path="../../../../Content/js/system/knockout-3.5.1.js" />
/// <reference path="../../../../Content/js/knockout.custom.js" />
/// <reference path="../../../../Content/js/system/jquery-1.11.3.min.js" />
/// <reference path="Helper.Offline.js" />
/// <reference path="../../../../Content/js/system/async-1.4.2.js" />
namespace("Crm.Offline.ViewModels").OfflineViewModel = function () {
	var self = this;

	if (window.FileRepository) {
		self.documentCategoryKeys = [];
		self.fileRepository = new window.FileRepository();
		self.fileSyncActive = window.ko.observable(false);
		self.fileSyncError = window.ko.observable(null);
		self.fileSyncQueue = null;
		self.fileSyncStatus = window.ko.observable(null);
		self.filesOffline = window.ko.observable(null);
		self.filesTotal = window.ko.observable(null);
		self.spaceUsed = window.ko.observable(null);
		self.spaceFree = window.ko.observable(null);
	}

	self.online = window.Helper.Offline.online;
	self.status = window.ko.observable(window.Helper.Offline.status);
	self.transientItemCount = window.Helper.Offline.transientItemCount;
	self.currentDate = window.ko.observable(new Date());
	self.lastSyncAgo = window.ko.computed(function () {
		var currentDate = window.ko.unwrap(self.currentDate);
		var lastCompleteSync = window.Helper.Sync.lastCompleteSync();
		if (!lastCompleteSync) {
			return "";
		}
		var diff = currentDate - lastCompleteSync;
		if (window.Helper.String.getTranslatedString("JustSynchronized").indexOf("Translation Missing") !== -1) {
			return ""; // translations not initialized yet
		}

		var diffSeconds = diff / 1000;
		if (diffSeconds < 0) {
			return window.Helper.String.getTranslatedString("JustSynchronized");
		}
		var hh = Math.floor(diffSeconds / 3600);
		var mm = Math.floor(diffSeconds % 3600 / 60);
		var ss = Math.floor(diffSeconds % 3600 % 60);
		if (hh > 23) {
			return window.Helper.String.getTranslatedString('LastSyncOnAt')
				.replace('{0}', window.Globalize.formatDate(lastCompleteSync, { skeleton: "Md" }))
				.replace('{1}', window.Globalize.formatDate(lastCompleteSync, { time: "short" }));
		}
		if (hh === 0 && mm === 0) {
			return window.Helper.String.getTranslatedString("JustSynchronized");
		}
		var ago = "";
		if (hh === 1) {
			ago += "1 " + window.Helper.String.getTranslatedString("Hour") + ", ";
		} else if (hh > 1) {
			ago += hh + " " + window.Helper.String.getTranslatedString("Hours") + ", ";
		}
		if (mm === 1) {
			ago += "1 " + window.Helper.String.getTranslatedString("Minute") + "";
		} else {
			ago += mm + " " + window.Helper.String.getTranslatedString("Minutes") + "";
		}
		return window.Helper.String.getTranslatedString("LastSyncAgo").replace("{0}", ago);
	});
	self.showLastSyncAgo = window.ko.computed(function () {
		return !!self.lastSyncAgo() && self.status() === 'offline';
	});
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.init = function (routeValues) {
	var self = this;

	self.currentDateInterval = setInterval(function () {
		if (self.status() !== window.Helper.Offline.status) {
			self.status(window.Helper.Offline.status);
		}
		self.currentDate(new Date());
	}, 10000);

	if (self.status() === "online"
		|| !window.FileRepository
		|| !window.AuthorizationManager.currentUserHasPermission("Sync::DocumentCategory")
		|| !window.AuthorizationManager.currentUserHasPermission("Sync::DocumentAttribute")
		|| !window.AuthorizationManager.currentUserHasPermission("Sync::FileResource")) {
		return new $.Deferred().resolve().promise();
	}
	return window.database.Main_DocumentCategory.filter(function(it) {
		return it.OfflineRelevant === true;
	}).map(function (it) {
		return it.Key;
	}).distinct().toArray(self.documentCategoryKeys).then(function () {
		return self.fileRepository.init();
	}).then(function () {
		return self.updateOfflineFileCount();
	}).then(function () {
		return self.updateFreeDiskSpace();
	}).then(function () {
		document.addEventListener("Initialized", self.syncFiles.bind(self));
		self.syncFiles();
	});
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.dispose = function () {
	var self = this;
	clearInterval(self.currentDateInterval);
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.cancel = function () {
	var self = this;
	if (self.fileSyncQueue) {
		self.fileSyncQueue.kill();
		self.fileSyncQueue = null;
	}
	self.fileSyncActive(false);
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.deleteFiles = function() {
	var self = this;
	window.Helper.Confirm.confirmDelete().then(function() {
		self.fileSyncActive(true);
		return self.fileRepository.deleteFiles().then(function() {
			return self.updateFreeDiskSpace();
		}).then(function() {
			self.filesOffline(0);
			self.spaceUsed(0);
			self.fileSyncStatus(null);
			self.fileSyncActive(false);
		}).fail(function(e) {
			self.fileSyncActive(false);
			self.fileSyncError(e);
		});
	});
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.getOfflineRelevantFiles = function() {
	var viewModel = this;
	return window.database.Main_FileResource.filter("it.OfflineRelevant === true || it.DocumentAttributes.OfflineRelevant === true || it.DocumentAttributes.DocumentCategoryKey in this.documentCategoryKeys", { documentCategoryKeys: viewModel.documentCategoryKeys });
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.sync = function() {
	window.Helper.Url.reloadAndReturnToLocation();
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.syncFiles = function () {
	var self = this;
	self.fileSyncError(null);
	self.fileSyncActive(true);
	var query = self.getOfflineRelevantFiles().filter(function(it) { return it.IsSynced === false || it.SyncDate !== it.ModifyDate; });
	var failed = 0;
	var successful = 0;
	self.updateOfflineFileCount().then(function() {
		return query.count();
	}).then(function(count) {
		var d = new $.Deferred();
		if (count === 0) {
			self.fileSyncError(null);
			self.fileSyncStatus(null);
			return d.resolve().promise();
		}
		var pageSize = 10;
		self.fileSyncQueue = window.async.queue(function(task, cb) {
			var fileSyncStatus = window.Helper.String.getTranslatedString("FileDownloadStatusDownloading").replace("{0}", successful).replace("{1}", count);
			if (failed > 0) {
				fileSyncStatus += "\r\n\r\n";
				fileSyncStatus += window.Helper.String.getTranslatedString("FileDownloadFailed").replace("{0}", failed);
			}
			self.fileSyncStatus(fileSyncStatus);
			query
				.orderBy(function(x) { return x.Id; })
				.skip(failed)
				.take(pageSize)
				.toArray()
				.then(function(fileResources) {
					return self.fileRepository.downloadFileResources(fileResources, true);
				})
				.then(function(successfulFileResources, failedFileResources) {
					failed += failedFileResources.length;
					successful += successfulFileResources.length;
					self.filesOffline(self.filesOffline() + (successfulFileResources.length));
					self.spaceUsed(window._.reduce(successfulFileResources, function(a, b) { return a + b.Length; }, self.spaceUsed() || 0));
					return self.updateFreeDiskSpace();
				}).then(cb).fail(function(e) {
					window.Log.error(e);
					cb();
				});
		});
		self.fileSyncQueue.drain = function() {
			var fileSyncStatus = window.Helper.String.getTranslatedString("FileDownloadStatusSuccess").replace("{0}", successful);
			if (failed > 0) {
				fileSyncStatus += "\r\n\r\n";
				fileSyncStatus += window.Helper.String.getTranslatedString("FileDownloadFailed").replace("{0}", failed);
			}
			self.fileSyncStatus(fileSyncStatus);
			d.resolve();
		};
		self.fileSyncQueue.error = function(e) {
			self.documentAttributeListIndexViewModel.downloadStatus("Downloaded failed");
			self.fileSyncActive(false);
			d.resolve();
		};
		self.fileSyncQueue.push(Array(Math.ceil(count / pageSize)));
		return d.promise();
	}).then(function() {
		return self.fileRepository.deleteExpiredFiles(function(ids) { return self.getOfflineRelevantFiles().filter("it.Id in this.ids", { ids: ids }).map("it.Id") });
	}).then(function() {
		self.fileSyncActive(false);
	});
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.updateFreeDiskSpace = function() {
	var viewModel = this;
	return viewModel.fileRepository.getFreeDiskSpace().then(function(freeDiskSpaceInBytes) {
		viewModel.spaceFree(freeDiskSpaceInBytes);
	});
};
window.Crm.Offline.ViewModels.OfflineViewModel.prototype.updateOfflineFileCount = function() {
	var viewModel = this;
	return viewModel.fileRepository.getFiles().then(function(files) {
		var sum = files.filter(function(file) { return file.File !== null; }).reduce(function(s, file) {
			s.count++;
			s.size += file.File.size;
			return s;
		}, { count: 0, size: 0 });
		viewModel.spaceUsed(sum.size);
		viewModel.filesOffline(sum.count);
		return viewModel.getOfflineRelevantFiles().count();
	}).then(function(totalRelevant) {
		viewModel.filesTotal(totalRelevant);
	});
};
$(document).on("click", "#menu-sync, #menu-sync a", function(e) {
	e.preventDefault();
	window.Crm.Offline.ViewModels.OfflineViewModel.prototype.sync();
	return false;
});
