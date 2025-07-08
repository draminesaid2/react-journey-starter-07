///<reference path="../../../../Content/@types/index.d.ts"/>
import $ from "jquery";
import {namespace} from "../../../../Content/ts/namespace";

declare global {
	interface Window {
		odatajs: any
		lastGetSegmentReceived: any
	}
}

let syncActive = false;
const syncClientIdParamName = "clientId";
let request = null;
const requestMaxRetries = 3;
let requestRetries = 0;

let syncStatus = null;
let replicationGroupSettings = {};
let migrationNeeded = [];
let isMigrationFinished = false;
let lastCompleteSync = JSON.parse(window.Helper.Database.getFromLocalStorage("lastSync")) == null ? null : new Date(JSON.parse(window.Helper.Database.getFromLocalStorage("lastSync")));
let localDatabaseWithoutRelations = null;

function invokeWebService(url: string, method: string, params: object | string, requestTimeout?: number, maxRetries?: number): JQuery.Promise<any> {
	requestTimeout = requestTimeout || window.Crm.Offline.Settings.RequestTimeout;
	if (Number.isInteger(maxRetries) === false) {
		maxRetries = requestMaxRetries;
	}
	const d = $.Deferred();
	if (syncActive === false) {
		return d.promise();
	}
	params = params || {};
	if (request != null && request.state() === "pending") {
		request.abort();
	}
	window.Log.debug("Helper.Sync.js: invoking webservice on " + url + ", timeout " + requestTimeout * Math.pow(2, requestRetries));
	request = $.ajax({
		type: method,
		contentType: "application/json; charset=utf-8",
		url: window.Helper.Url.resolveUrl(url),
		data: params,
		dataType: "json",
		success: function (results) {
			try {
				requestRetries = 0;
				d.resolve(results);
			} catch (e) {
				const msg = "Exception when calling invokeWebService method on url " + url + " with exception " + (e && e.toString ? e.toString() : JSON.stringify(e));
				window.Log.warn(msg);
				if (requestRetries < maxRetries) {
					requestRetries++;
					// hammer time!
					invokeWebService(url, method, params).then(d.resolve).fail(d.reject);
				} else {
					d.reject(e);
				}
			}
		},
		error: function (error, status) {
			window.Log.debug("Helper.Sync.js: invoking webservice returned error " + error + ", status " + status);
			// dont hammer / call error function when request was manually cancelled
			if (status === "abort") {
				return;
			}
			if ((method === "GET" || method === "PUT") && requestRetries < maxRetries) {
				requestRetries++;
				// hammer time!
				window.Log.debug("Helper.Sync.js: invoking webservice again after error " + requestRetries + "/" + maxRetries + ", timeout " + requestTimeout * Math.pow(2, requestRetries));
				invokeWebService(url, method, params).then(d.resolve).fail(d.reject);
			} else {
				d.reject(error);
			}
		},
		timeout: requestTimeout * Math.pow(2, requestRetries)
	});
	return d.promise();
}

async function getTransientItems() {
	const transientItems = {};
	let definitions = await window.Helper.Database.getDefinitions();
	await Promise.all(definitions.map(definition => {
		if (!!definition.table) {
			transientItems[definition.table] = {};
			transientItems[definition.table].Definition = definition;
			transientItems[definition.table].Items = [];
			return new Promise((resolve, reject) => {
				getTransientItemsOfTable(definition.table)
					.then(results => transientItems[definition.table].Items = results)
					.then(resolve)
					.fail(e => {
						const error = !!e ? e.message || e : e;
						window.Log.error(`window.Helper.Sync: getTransientItems: getting transient items failed for ${definition.table}: ${error}`);
						reject(e);
					});
			});
		}
		return Promise.resolve();
	}));
	const allTransientItems = [];
	for (const transientItemsIndex in transientItems) {
		if (transientItems.hasOwnProperty(transientItemsIndex)) {
			for (let i = 0; i < transientItems[transientItemsIndex].Items.length; i++) {
				const transientItem = transientItems[transientItemsIndex].Items[i];
				const definition = transientItems[transientItemsIndex].Definition;
				localDatabaseWithoutRelations[definition.table].attachOrGet(transientItem);
				allTransientItems.push({Item: transientItem, Definition: definition});
			}
		}
	}
	window.Helper.Offline.transientItemInformation(allTransientItems.map(transientItem => {
		const storageKey = transientItem.Item.getType().name;
		return {Id: transientItem[window.Helper.Database.getKeyProperty(storageKey)], Type: storageKey};
	}));
	await Promise.all(allTransientItems.map((transientItem) =>
		window.Helper.Database.getTransactionIds(transientItem.Definition.table, transientItem.Item).then(function (transactionIds) {
			transientItem.TransactionIds = transactionIds;
		}).catch(function (e) {
			window.Log.error("window.Helper.Sync: getTransientItems: getting transactionid for  items failed for " + window.JSON.stringify(transientItem) + ": " + window.JSON.stringify(e));
		})));
	return allTransientItems;
}

function getTransientItemsOfTable(table) {
	if (table === "Main_FileResource") {
		const fileResourceSchema = window.Helper.Database.getSchema("Main_FileResource");
		const hasExtensions = JSON.stringify(fileResourceSchema.ExtensionValues.defaultValue) !== JSON.stringify({});
		const map = "{" +
			Object.getOwnPropertyNames(fileResourceSchema).filter(function (x) {
				return x !== "Content" && !fileResourceSchema[x].keys && (hasExtensions || x !== "ExtensionValues");
			})
				.map(function (x) {
					return x + ": it." + x;
				}).join(", ") +
			"}";
		return localDatabaseWithoutRelations[table]
			.map(map)
			.filter("it.ItemStatus != this.value", {value: window.ko.ItemStatus.Persisted})
			.toArray()
			.then(function (results) {
				return results.map(function (x) {
					return localDatabaseWithoutRelations[table][table].create(x);
				});
			});
	}
	return localDatabaseWithoutRelations[table]
		.filter("it.ItemStatus != this.value", {value: window.ko.ItemStatus.Persisted})
		.toArray();
}

function uniq(items: string[]): string[] {
	return [...new Set(items)];
}


async function getTransientItemsGroupedByTransaction() {
	let transientItems = await getTransientItems();
	const results = {};
	const transactionIdMap = {};
	const generatedIdsArray = [];
	transientItems.forEach(function (transientItem) {
		const transactionIds: string[] = uniq((transientItem.TransactionIds || [null]).map(function (id) {
			return id || window.$data.createGuid().toString();
		}));
		if (transactionIds.length > 1) {
			const newTid = window.$data.createGuid().toString();
			generatedIdsArray.push(newTid);
			const updateIds = {};
			transactionIds.forEach(function (id) {
				if (transactionIdMap[id]) {
					results[newTid] = results[newTid] || [];
					Array.prototype.push.apply(results[newTid], results[transactionIdMap[id]]);
					delete results[transactionIdMap[id]];
					updateIds[transactionIdMap[id]] = true;
				} else {
					transactionIdMap[id] = newTid;
				}
				updateIds[id] = true;
			});
			Object.keys(transactionIdMap).forEach(function (key) {
				if (updateIds.hasOwnProperty(key) || updateIds.hasOwnProperty(transactionIdMap[key])) {
					transactionIdMap[key] = newTid;
				}
			});
		} else {
			transactionIdMap[transactionIds[0]] = transactionIdMap[transactionIds[0]] || transactionIds[0];
		}
		const transactionId = transactionIdMap[transactionIds[0]];
		results[transactionId] = results[transactionId] || [];
		results[transactionId].push(transientItem);
	});
	const newResultsObject = {};
	Object.keys(results).forEach(function (key) {
		if (generatedIdsArray.indexOf(key) === -1) {
			newResultsObject[window.$data.createGuid().toString()] = results[key];
		} else {
			newResultsObject[key] = results[key];
		}
	});
	return newResultsObject;
}

function mapElement(src, dest, _) {
	Object.getOwnPropertyNames(src).forEach(function (propertyName) {
		if (dest[propertyName] !== undefined) {
			dest[propertyName] = src[propertyName];
		}
	});
}

function insertInStorage(elements, storageKey) {
	if (Array.isArray(elements)) {
		const dbElements = elements.map(element => {
			element.localTimestamp = (new Date()).getTime();
			element.ItemStatus = window.ko.ItemStatus.Persisted;
			return localDatabaseWithoutRelations[storageKey][storageKey].create(element);
		});
		localDatabaseWithoutRelations[storageKey].addMany(dbElements);
	} else {
		elements.ItemStatus = window.ko.ItemStatus.Persisted;
		const dbElement = localDatabaseWithoutRelations[storageKey][storageKey].create(elements);
		localDatabaseWithoutRelations[storageKey].add(dbElement);
	}
}

function updateInStorage(elementInStorage, element, storageKey) {
	const keyProperty = window.Helper.Database.getKeyProperty(storageKey);
	if (element.ItemStatus === undefined && (elementInStorage.ItemStatus === window.ko.ItemStatus.Modified || elementInStorage.ItemStatus === window.ko.ItemStatus.Removed)) {
		window.Log.info("skipping update of " + keyProperty + ": " + element[keyProperty] + " in: " + storageKey + " because the element has unsynced local changes");
		return;
	} else {
		window.Log.debug("updating " + keyProperty + ": " + element[keyProperty] + " in: " + storageKey);
		localDatabaseWithoutRelations[storageKey].attachOrGet(elementInStorage);
		mapElement(element, elementInStorage, storageKey);
		elementInStorage.localTimestamp = (new Date()).getTime();
		elementInStorage.ItemStatus = window.ko.ItemStatus.Persisted;
	}
}

function batchInsertOrUpdateInStorage(elements, storageKey) {
	const d = $.Deferred();
	if (elements.length === 0) {
		return d.resolve().promise();
	}
	window.Log.debug("window.Helper.Sync: batch preparing inserting/updating");
	let start = performance.now();
	const keyProperty = window.Helper.Database.getKeyProperty(storageKey);
	const elementKeys = elements.map(element => element[keyProperty]);
	localDatabaseWithoutRelations[storageKey]
		.filter("it." + keyProperty + " in elementKeys", {elementKeys: elementKeys})
		.map("{ " + keyProperty + ": it." + keyProperty + ", ItemStatus: it.ItemStatus }")
		.toArray(function (persistentElements) {
			window.Log.debug("window.Helper.Sync: batch preparing inserting/updating read elements from db in " + (performance.now() - start) + " ms");
			start = performance.now();
			const persistentKeys = persistentElements.map(persistentElement => persistentElement[keyProperty]);
			let inserted = 0;
			let updated = 0;
			for (const element of elements) {
				if (persistentKeys.indexOf(element[keyProperty]) === -1) {
					insertInStorage(element, storageKey);
					inserted++;
				} else {
					const elementInStorage = localDatabaseWithoutRelations[storageKey][storageKey].create();
					elementInStorage[keyProperty] = element[keyProperty];
					elementInStorage.ItemStatus = window.ko.ItemStatus.Persisted;
					updateInStorage(elementInStorage, element, storageKey);
					updated++;
				}
			}
			window.Log.debug("window.Helper.Sync: batch preparing inserting/updating done after " + (performance.now() - start) + ", executing queries (inserts: " + inserted + ", updates:" + updated + ")");
			start = performance.now();
			localDatabaseWithoutRelations[storageKey].saveChanges()
				.done(function () {
					window.Log.debug("window.Helper.Sync: batch inserting/updating done, inserted " + inserted + ", updated " + updated + " in " + (performance.now() - start) + " ms");
					d.resolve();
				})
				.fail(function (e) {
					const errorMessage = "Inserting/Updating data to local database \"" + storageKey + "\" failed: " + e.message;
					window.Log.error(errorMessage);
					d.reject(errorMessage);
				});
		});
	return d.promise();
}

// TODO: use actual batches if supported by provider
function batchRemoveFromStorage(elementIds, storageKey) {
	if (elementIds.length === 0) {
		return $.Deferred().resolve().promise();
	} else {
		const keyProperty = window.Helper.Database.getKeyProperty(storageKey);
		for (const item of elementIds) {
			const element = {};
			element[keyProperty] = item;
			localDatabaseWithoutRelations[storageKey].remove(element);
		}
		return localDatabaseWithoutRelations.saveChanges();
	}
}

function getChangeRequest(transientItem, transactionId, contentId) {
	let method = null;
	if (transientItem.ItemStatus === window.ko.ItemStatus.Added || transientItem.ItemStatus === window.ko.ItemStatus.Modified) {
		method = "POST";
	} else if (transientItem.ItemStatus === window.ko.ItemStatus.Removed) {
		method = "DELETE";
	}
	if (method != null) {
		const typeName = transientItem.getType().name;
		return {
			requestUri: window.Helper.Url.resolveUrl("~/api/" + typeName),
			data: JSON.parse(JSON.stringify(transientItem)),
			method: method,
			headers: {
				"Content-ID": contentId,
				"Transaction-ID": transactionId,
				"Crm.Offline.Sync": true
			}
		};
	}
	window.Log.error("ItemStatus not found: " + JSON.stringify(transientItem));
	return null;
}

function handleItemSyncedToServer(transientItem, storageKey) {
	if (transientItem.ItemStatus === window.ko.ItemStatus.Added || transientItem.ItemStatus === window.ko.ItemStatus.Modified) {
		transientItem.ItemStatus = window.ko.ItemStatus.Persisted;
	} else if (transientItem.ItemStatus === window.ko.ItemStatus.Removed) {
		localDatabaseWithoutRelations[storageKey].remove(transientItem);
	} else {
		window.Log.error("ItemStatus not found: " + JSON.stringify(transientItem));
	}
}

function uploadFileResources(fileResourceIds) {
	const d = $.Deferred();
	if (fileResourceIds.length === 0) {
		return d.resolve().promise();
	}

	Promise.all(fileResourceIds.map(fileResourceId => {
		return new Promise((resolve, reject) => {localDatabaseWithoutRelations.Main_FileResource.find(fileResourceId).then(resolve).fail(reject)})
			.then((fileResource: any) => {
				const blob = window.Helper.String.base64toBlob(fileResource.Content, fileResource.ContentType);
				const formData = new FormData();
				formData.append("data", blob, fileResource.Id);
				return new Promise((resolve2, reject2) => {$.ajax({
					type: "POST",
					url: window.Helper.Url.resolveUrl("~/File/AddFile"),
					data: formData,
					processData: false,
					contentType: false
				}).then(resolve2).fail(reject2)});
			});
	})).then(d.resolve).catch(d.reject);

	return d.promise();
}

function syncItemsToServer(orderedTransientItems, transactionId) {
	const d = $.Deferred();
	const changeRequests = [];
	let contentId = 1;

	const fileResourceIds = orderedTransientItems.filter(function (transientItem) {
		return transientItem.Definition.table === "Main_FileResource" && transientItem.Item.ItemStatus !== window.ko.ItemStatus.Removed;
	}).map(function (transientItem) {
		return transientItem.Item.Id;
	});
	uploadFileResources(fileResourceIds).then(function () {
		orderedTransientItems.forEach(function (transientItem) {
			const changeRequest = getChangeRequest(transientItem.Item, transactionId, contentId++);
			if (changeRequest != null) {
				changeRequests.push(changeRequest);
			}
		});

		window.odatajs.oData.request({
				requestUri: window.Helper.Url.resolveUrl("~/api/$batch"),
				method: "POST",
				data: {__batchRequests: [{__changeRequests: changeRequests}]}
			},
			function (data, response) {
				if (response.statusCode != 200) {
					window.Log.warn(response);
					d.reject(response);
					return;
				}
				const errors = [];
				data.__batchResponses.forEach(function (batchResponse) {
					batchResponse.__changeResponses.forEach(function (changeResponse) {
						if (changeResponse.statusCode != 200) {
							errors.push(changeResponse);
						}
					});
				});
				if (errors.length > 0) {
					window.Log.warn(errors);
					d.reject(response);
					return;
				}
				orderedTransientItems.forEach(function (transientItem) {
					localDatabaseWithoutRelations.attach(transientItem.Item);
					handleItemSyncedToServer(transientItem.Item, transientItem.Definition.table);
				});
				localDatabaseWithoutRelations.saveChanges().then(d.resolve).fail(d.reject);
			},
			d.reject,
			window.odatajs.oData.batch.batchHandler);
	}).fail(function (error) {
		d.reject(error);
	});
	return d.promise();
}

async function syncToServer(progressHandler) {
	const d = $.Deferred();
	let transientItemGroups = await getTransientItemsGroupedByTransaction();
	const transactionIds = Object.getOwnPropertyNames(transientItemGroups);
	if (transactionIds.length === 0) {
		window.Helper.Offline.transientItemInformation([]);
		isMigrationFinished = true;
		d.resolve();
		return;
	}
	let hasFailedTransactions = false;

	await Promise.all(transactionIds.map(async transactionId => {
		const orderedTransientItems = transientItemGroups[transactionId].sort((item1, item2) => {
			if (!item1.Item.localTimestamp || item1.Item.localTimestamp === "" || !item2.Item.localTimestamp || item2.Item.localTimestamp === "") {
				window.Log.warn("Helper.Sync.syncToServer: transientItem has no localTimestamp or localTimestamp is empty: item1:" + JSON.stringify(item1.Item) + " item2:" + JSON.stringify(item2.Item));
				return 0;
			}
			return item1.Item.localTimestamp - item2.Item.localTimestamp;
		});
		progressHandler(transactionIds.indexOf(transactionId), transactionIds.length);
		try {
			await syncItemsToServer(orderedTransientItems, transactionId);
		} catch (e: any) {
			hasFailedTransactions = true;
			const entities = orderedTransientItems.map(function (x) {
				return x.Item;
			});
			window.Log.error("failed to sync " + orderedTransientItems.length + " entities for transaction " + transactionId + ": entities: " + JSON.stringify(entities) + " - error: " + JSON.stringify(e));
			orderedTransientItems.forEach(function (transientItem) {
				localDatabaseWithoutRelations.detach(transientItem.Item);
			});
			if (e && e.request && e.request.body) {
				delete e.request.body;
			}
			if (e && e.request && e.request.data) {
				delete e.request.data;
			}
			window.Log.error("failed to sync " + orderedTransientItems.length + " entities for transaction " + transactionId + ": entities: " + JSON.stringify(orderedTransientItems) + " - error: " + JSON.stringify(e));
			throw e;
		}
	}));
	window.Helper.Offline.transientItemInformation([]);
	if (hasFailedTransactions === false) {
		isMigrationFinished = true;
	}
}

function clearTableIfRequired(definition, params, result) {
	const initialSync = !!params && (params.clientId == null || params.clientId !== result.ClientId);
	const firstSegment = result.Segment === 1 || result.TotalSegments === 0;
	if (initialSync && firstSegment) {
		return clearTable(definition.table);
	}
	return $.Deferred().resolve().promise();
}

function clearTable(table) {
	let clear;
	if (localDatabaseWithoutRelations.storageProvider.supportedSetOperations.batchDelete) {
		clear = localDatabaseWithoutRelations[table].removeAll();
	} else {
		clear = localDatabaseWithoutRelations[table].forEach(function (result) {
			localDatabaseWithoutRelations[table].remove(result);
		});
	}
	return clear.then(function () {
		localDatabaseWithoutRelations[table].saveChanges();
	});
}

function useBulkInsert(params, result) {
	const bulkInsertSupported = !!localDatabaseWithoutRelations.storageProvider.supportedSetOperations.batchDelete;
	const initialSync = !!params && (params.clientId == null || params.clientId !== result.ClientId);
	return bulkInsertSupported && result.TotalSegments > 0 && initialSync;
}

function bulkInsert(definition, result) {
	const dbElements = result.RestObjects.map((element) => {
		element.ItemStatus = window.ko.ItemStatus.Persisted;
		return localDatabaseWithoutRelations[definition.table][definition.table].create(element);
	});
	return localDatabaseWithoutRelations.bulkInsert(localDatabaseWithoutRelations[definition.table], undefined, dbElements).then(function () {
		return localDatabaseWithoutRelations[definition.table].saveChanges();
	});
}

function handleDataFromServer(definition, params, result) {
	return clearTableIfRequired(definition, params, result).then(function () {
		if (useBulkInsert(params, result)) {
			return bulkInsert(definition, result);
		} else {
			return batchInsertOrUpdateInStorage(result.RestObjects || [], definition.table).then(function () {
				return batchRemoveFromStorage(result.ExpiredIds || [], definition.table);
			});
		}
	});
}

function getFromServer(definition, params) {
	const d = $.Deferred();
	window.Log.debug("Helper.Sync.js: getFromServer");
	if (!definition.model || !definition.plugin) {
		const errorMessage = "Helper.Sync getFromServer failed: model + pluginName not specified";
		window.Log.error(errorMessage);
		d.reject(errorMessage);
	}
	if (params == null || !params.segment) {
		const clientId = params != null && !!params.clientId ? params.clientId : null;
		invokeWebService("~/Crm.Offline/Sync", "GET", {
			clientId: clientId,
			model: definition.model,
			pluginName: definition.plugin,
			replicationGroupSettings: replicationGroupSettings
		}).then(function (results) {
			d.resolve(results);
		}).fail(function (error: any) {
			const errorMessage = "Error when calling GetAll method on Sync RestController for " + definition.model + ", " + definition.plugin + ": " + (error.responseText || error.statusText || error);
			window.Log.error(errorMessage);
			d.reject(errorMessage);
		});
	} else {
		params.model = definition.model;
		params.pluginName = definition.plugin;
		const now = new Date();
		if (!!window.lastGetSegmentReceived) {
			window.Log.debug("getSegment called " + (now.getTime() - window.lastGetSegmentReceived.getTime()).toString() + " ms after last getSegment was received");
		}
		invokeWebService("~/Crm.Offline/Sync/GetSegment", "GET", params).then(function (result) {
			window.lastGetSegmentReceived = new Date();
			window.Log.debug("getSegment received " + (window.lastGetSegmentReceived.getTime() - now.getTime()).toString() + " ms after request was sent");
			d.resolve(result);
		}).fail(function (error: any) {
			const errorMessage = "Error when calling GetSegment method on Sync RestController for " + definition.model + ", " + definition.plugin + ": " + (error.responseText || error.statusText || error);
			window.Log.error(errorMessage);
			d.reject(errorMessage);
		});
	}
	return d.promise();
}

function getSegmentsFromServer(definition, params, progressHandler) {
	const d = $.Deferred();

	const saveSyncStatus = function () {
		if (params && params.container && params.segment) {
			syncStatus.container = params.container;
			syncStatus.segment = params.segment;
			window.Helper.Database.saveToLocalStorage("syncStatus", JSON.stringify(syncStatus));
		}
	};
	saveSyncStatus();
	const getData = function () {
		const def = $.Deferred();
		getFromServer(definition, params).then(function (result) {
			if (result.Segment === 0 || result.Segment === 1) {
				updateClientId(definition, result.ClientId);
			}
			handleDataFromServer(definition, params, result).then(function () {
				if (!!result.Segment) {
					progressHandler(null, null, definition.table, result.Segment, result.TotalSegments);
				}
				def.resolve(result);
			}).fail(def.reject);
		}).fail(def.reject);
		return def.promise();
	};

	function completeSegment(clientId) {
		if (!params.container) {
			return d.resolve(clientId);
		}
		return invokeWebService("~/Crm.Offline/Sync/CompleteSegmentContainer", "GET", params).then(function () {
			d.resolve(clientId);
		}).fail(function (error: any) {
			const msg = "Error when calling CompleteSegmentContainer method on Sync RestController for " + definition.model + ", " + definition.plugin + ": " + (error.responseText || error.statusText || error);
			window.Log.warn(msg);
			d.resolve(clientId);
		});
	}

	getData().then(function (result) {
		const clientId = result.ClientId;
		params.container = result.Container;
		params.clientId = params.clientId || null;
		if (!result.Segment || result.TotalSegments === 1) {
			completeSegment(clientId);
			return null;
		}
		const getSegment = function () {
			if (params.segment) {
				params.segment++;
			} else {
				params.segment = 2;
			}
			saveSyncStatus();
			return getData();
		};
		const def = $.Deferred();
		(async () => {
			let flag = false;
			let errorMessage = "";
			let segmentCount = result.TotalSegments - (params.segment || 1);
			for (let i = 0; i < segmentCount; i++) {
				try {
					await getSegment();
				} catch (error) {
					flag = true;
					errorMessage = error ? error.toString() : "error when getting segment " + params.segment + " from server, container " + params.container;
				}
			}
			if (flag) {
				def.reject(errorMessage);
				return;
			}
			params.model = definition.model;
			params.pluginName = definition.plugin;
			completeSegment(clientId);
		})();
		return def.promise();
	}).fail(function (e) {
		// Server did not find corresponding segment, we have to begin from scratch
		if (typeof e === "string" && e.indexOf("KeyNotFoundException") !== -1) {
			syncStatus.container = null;
			syncStatus.segment = null;
			window.Helper.Database.saveToLocalStorage("syncStatus", JSON.stringify(syncStatus));
			getUpdatedDataFromServer(definition, syncStatus, progressHandler).then(d.resolve).fail(d.reject);
			return;
		} else {
			d.reject(e);
		}
	});
	return d.promise();
}

function updateClientId(definition, clientId) {
	const clientIdStorageKey = definition.table + "_clientId";
	window.Helper.Database.saveToLocalStorage(clientIdStorageKey, JSON.stringify(clientId));
}

function getUpdatedDataFromServer(definition, savedSyncStatus, progressHandler) {
	const d = $.Deferred();
	const params: any = {};
	if (!!savedSyncStatus && !!savedSyncStatus.container && !!savedSyncStatus.segment) {
		params.container = savedSyncStatus.container;
		params.segment = savedSyncStatus.segment;
	} else {
		const clientIdStorageKey = definition.table + "_clientId";
		const clientId = JSON.parse(window.Helper.Database.getFromLocalStorage(clientIdStorageKey));
		params[syncClientIdParamName] = clientId;
	}
	getSegmentsFromServer(definition, params, progressHandler).then(function (clientId) {
		d.resolve(clientId);
	}).fail(d.reject);
	return d.promise();
}

function syncFromServerObject(definitions, objectlist, progressHandler, j) {
	const d = $.Deferred();
	syncStatus = {step: j};
	window.Helper.Database.saveToLocalStorage("syncStatus", JSON.stringify(syncStatus));
	progressHandler(j + 1, objectlist.length, definitions[objectlist[j].Index].table);
	getUpdatedDataFromServer(definitions[objectlist[j].Index], syncStatus, progressHandler).then(function () {
		j++;
		if (j === objectlist.length) {
			window.Helper.Database.saveToLocalStorage("syncStatus", JSON.stringify(null));
			d.resolve();
			return d.promise();
		} else {
			syncFromServerObject(definitions, objectlist, progressHandler, j).then(d.resolve).fail(d.reject);
			return d.promise();
		}
	}).fail(function (error: any) {
		const errorMessage = "Error when calling GetAll method on Sync RestController for " + definitions[objectlist[j].Index].model + ", " + definitions[objectlist[j].Index].plugin + ": " + (error.responseText || error.statusText || error);
		window.Log.error(errorMessage);
		d.reject(errorMessage);
	});
	return d.promise();
}

function syncFromServer(definitions: any[], progressHandler, i?) {
	const d = $.Deferred();
	if (typeof i == "undefined") {
		const definitionArray = [];
		const objectToSync = [];
		definitions.forEach((definition, index) => {
			const clientIdStorageKey = definition.table + "_clientId";
			const clientId = JSON.parse(window.Helper.Database.getFromLocalStorage(clientIdStorageKey));
			if (clientId !== null && clientId !== undefined) {
				definitionArray.push({
					Model: definition.model,
					Plugin: definition.plugin,
					ClientId: clientId,
					Index: index
				});
			} else {
				objectToSync.push({
					Model: definition.model,
					Plugin: definition.plugin,
					Index: index
				});
			}
		});
		if (definitionArray.length === 0) {
			syncFromServerObject(definitions, objectToSync, progressHandler, 0).then(d.resolve).fail(d.reject);
		} else {
			const timeout = window.Crm.Offline.Settings.RequestTimeout * 4;
			const size = parseInt(window.Crm.Offline.Settings.CheckRequestSize, 10);
			let progress = 0;
			const definitionBatches = [];
			for (let j = 0; j < definitionArray.length; j += size) {
				definitionBatches.push(definitionArray.slice(j, j + size));
			}
			let results = [];

			(async () => {
				let flag = false;
				let errorMessage;
				for (const definitionBatch of definitionBatches) {
					try {
						const checkResults = await invokeWebService("~/Crm.Offline/Sync/Check", "PUT", JSON.stringify({
							Definitions: definitionBatch,
							ReplicationGroupSettings: replicationGroupSettings
						}), timeout, 1);
						results = results.concat(checkResults);
						progress += size;
						progressHandler(1, 1, "FetchingSynchronizationInformation", progress, definitionArray.length);
					} catch (error: any) {
						errorMessage = "Error when calling Check method on Sync RestController: " + (error.responseText || error.statusText || error);
						window.Log.error(errorMessage);
						flag = true;
					}
				}
				if (!flag) {
					if (objectToSync.length !== 0) {
						results = results.concat(objectToSync);
					}
					if (results.length === 0) {
						syncStatus = JSON.parse(window.Helper.Database.getFromLocalStorage("syncStatus")) || {step: 0};
						d.resolve();
					} else {
						syncFromServerObject(definitions, results, progressHandler, 0).then(d.resolve).fail(d.reject);
					}
				} else {
					d.reject(errorMessage);
				}
			})();
		}
	} else {
		syncFromServer(definitions, progressHandler, 0);
	}
	return d.promise();

}

// extending Helper.Database to include ItemStatus
const baseGetIndices = window.Helper.Database.getIndices;
window.Helper.Database.getIndices = function (storageKey) {
	const indices = baseGetIndices(storageKey);
	if (window.Helper.Database.getStorageOptions().provider === "webApi" || window.Helper.Database.getStorageOptions().provider === "oData") {
		return indices;
	}
	if (migrationNeeded.indexOf(storageKey) !== -1) {
		return [];
	}
	const indexName = storageKey + "_ItemStatus";
	const hasItemStatusIndex = indices.some(function (index) {
		return index.name === indexName;
	});
	if (!hasItemStatusIndex) {
		indices.unshift({name: indexName, keys: ["ItemStatus"]});
	}
	return indices;
};

function getExtendedSchema(schema, storageKey) {
	const extendedSchema: any = {};
	Object.getOwnPropertyNames(schema)
		.sort()
		.forEach(function (column) {
			extendedSchema[column] = schema[column];
		});
	if (extendedSchema.localTimestamp === undefined) {
		extendedSchema.localTimestamp = {type: "int"};
	}
	if (extendedSchema.ItemStatus === undefined) {
		extendedSchema.ItemStatus = {type: "int", defaultValue: window.ko.ItemStatus.Draft};
	}
	return extendedSchema;
}

// extending Helper.Database to include ItemStatus & localTimestamp
const baseGetSchema = window.Helper.Database.getSchema;
window.Helper.Database.getSchema = function (storageKey) {
	const schema = baseGetSchema(storageKey);
	if (window.Helper.Database.getStorageOptions().provider === "webApi" || window.Helper.Database.getStorageOptions().provider === "oData") {
		return schema;
	}
	if (schema === null) {
		return null;
	}
	if (storageKey) {
		let tableSchema = getExtendedSchema(schema, storageKey);
		if (migrationNeeded.length) {
			tableSchema = getTableSchemaWithoutRelations(tableSchema);
		}
		return tableSchema;
	}
	const extendedSchema = {};
	Object.getOwnPropertyNames(schema)
		.forEach(function (x) {
			extendedSchema[x] = getExtendedSchema(schema[x], x);
		});
	return extendedSchema;
};

const resetMigratedClientIds = function () {
	for (let i = 0; i < migrationNeeded.length; i++) {
		const storageKey = migrationNeeded[i];
		window.Log.info("Resetting clientId of migrated table " + storageKey);
		window.Helper.Database.saveToLocalStorage(storageKey + "_clientId", null);
	}
};

let cachedDefinitions = null;
const baseGetRawDefinitions = window.Helper.Database.getRawDefinitions;
window.Helper.Database.getRawDefinitions = function (): any {
	if (window.Helper.Database.getStorageOptions().provider === "webApi" ||
		window.Helper.Database.getStorageOptions().provider === "oData") {
		return baseGetRawDefinitions();
	}
	if (cachedDefinitions) {
		return $.Deferred().resolve(cachedDefinitions).promise();
	}
	return baseGetRawDefinitions().then(function (newDefinitions) {
		const schemaInfo = window.Helper.Database.getFromLocalStorage("SchemaInfo");
		let deferred;
		if (!schemaInfo) {
			deferred = reconstructSchema(newDefinitions);
		} else {
			deferred = $.Deferred().resolve(schemaInfo).promise();
		}
		return deferred.then(function (schemaInfo) {
			if (schemaInfo) {
				const oldDefinitions = JSON.parse(schemaInfo);
				const oldPluginSchema = window.Helper.Object.sortObject(oldDefinitions.tables);
				const newPluginSchema = window.Helper.Object.sortObject(newDefinitions.tables);

				const normalizeType = function (type) {
					if (type && type.AssemblyQualifiedName && type.AssemblyQualifiedName.defaultValue) {
						const newDefault = type.AssemblyQualifiedName.defaultValue.replace(/Version=\d+\.\d+\.\d+\.\d+,\s?/gi, "");
						type.AssemblyQualifiedName.defaultValue = newDefault;
					}
					return type;
				};

				Object.keys(oldPluginSchema).forEach(function (pluginName) {
					const oldTypes = oldPluginSchema[pluginName] || {};
					const newTypes = newPluginSchema[pluginName] || {};
					Object.keys(oldTypes).forEach(function (typeName) {
						let isMigrationNeeded = false;
						const oldType = normalizeType(oldTypes[typeName]);
						const newType = normalizeType(newTypes[typeName]);
						const oldSchema = JSON.stringify(oldType);
						const newSchema = JSON.stringify(newType);
						if (oldSchema !== newSchema) {
							isMigrationNeeded = true;
						} else {
							isMigrationNeeded = Object.keys(oldType).some(function (column) {
								const columnTypeName = oldType[column].type;
								const oldComplexTypeSchema = JSON.stringify(oldDefinitions.complexTypes[columnTypeName]);
								const newComplexTypeSchema = JSON.stringify(newDefinitions.complexTypes[columnTypeName]);
								return oldComplexTypeSchema !== newComplexTypeSchema;
							});
						}
						if (isMigrationNeeded) {
							const tableName = (pluginName + "_" + typeName).replace(".", "");
							const hasTransientItems = window.Helper.Offline.transientItemInformation().some(function (x) {
								return x.Type === tableName;
							});
							if (migrationNeeded.indexOf(tableName) === -1) {
								if (hasTransientItems) {
									window.Log.info("Schema of table " + tableName + " changed, with transient items");
								} else {
									window.Log.info("Schema of table " + tableName + " changed, but has no transient items");
								}
								migrationNeeded.push(tableName);
							}
						}
					});
				});
				if (migrationNeeded.length) {
					if (window.Helper.Offline.transientItemInformation().length === 0) {
						window.Log.info("Schema changed, but no transient items present - using new schema");
						resetMigratedClientIds();
						migrationNeeded = [];
					} else {
						cachedDefinitions = oldDefinitions;
					}
				}
			}
			if (!cachedDefinitions) {
				cachedDefinitions = newDefinitions;
			}
			return cachedDefinitions;
		});
	});
};

function getDatabaseSchemaWithoutRelations(databaseSchema) {
	const databaseSchemaWithoutRelations = {};
	Object.getOwnPropertyNames(databaseSchema).forEach(function (table) {
		databaseSchemaWithoutRelations[table] = getTableSchemaWithoutRelations(databaseSchema[table]);
	});
	return databaseSchemaWithoutRelations;
}

function getTableSchemaWithoutRelations(tableSchema) {
	const tableSchemaWithoutRelations = {};
	Object.getOwnPropertyNames(tableSchema).forEach(function (column) {
		if (!tableSchema[column].inverseProperty) {
			tableSchemaWithoutRelations[column] = tableSchema[column];
		}
	});
	return tableSchemaWithoutRelations;
}

function reconstructSchema(newDefinitions) {
	const tmpSchemaInfo = {complexTypes: {}, tables: {}};
	window.Helper.Database.getLocalStorageKeys().forEach(function (key) {
		if (key.indexOf("SchemaInfo_") === 0) {
			const tableName = key.replace("SchemaInfo_", "");
			const pluginType = tableName.split("_");
			let plugin;
			if (pluginType[0].indexOf("Crm") === 0) {
				plugin = "Crm." + pluginType[0].replace("Crm", "");
			} else if (pluginType[0].indexOf("Main") === 0) {
				plugin = "Main";
			} else if (pluginType[0].indexOf("Sms") === 0) {
				plugin = "Sms." + pluginType[0].replace("Sms", "");
			} else if (pluginType[0].indexOf("Customer") === 0) {
				plugin = "Customer." + pluginType[0].replace("Customer", "");
			} else {
				throw "unexpected tablename " + tableName;
			}
			if (newDefinitions.tables.hasOwnProperty(plugin) === false) {
				return;
			}
			const type = pluginType[1];
			const schema = JSON.parse(window.Helper.Database.getFromLocalStorage(key));
			if (tmpSchemaInfo.tables.hasOwnProperty(plugin) === false) {
				tmpSchemaInfo.tables[plugin] = {};
			}
			if (newDefinitions.tables[plugin].hasOwnProperty(type) === false) {
				return;
			}
			if (tmpSchemaInfo.tables[plugin].hasOwnProperty(type)) {
				throw "type already added " + type;
			}
			tmpSchemaInfo.tables[plugin][type] = schema;
		}
	});

	function retrieveSchemaFromDb() {
		const deferred = $.Deferred();
		const dbColumns = {};
		let db;
		try {
			const storageOptions = window.Helper.Database.getStorageOptions();
			if (storageOptions.provider === "local" || storageOptions.provider === "indexedDb") {
				const dbName = storageOptions.databaseName;
				db = window.openDatabase(dbName, "", "0", 0);
			}
		} catch (e) {
			window.Log.error("open database to retrieve schema failed", e);
			return deferred.reject(e);
		}

		db.transaction(function (tx) {
			tx.executeSql("SELECT name, sql FROM sqlite_master WHERE type = 'table'", null, function (tx, results) {
				for (let i = 0; i < results.rows.length; i++) {
					const table = results.rows.item(i);
					var types = {};
					let columns = table.sql.match(new RegExp("\\(.*\\)", "g"));
					if (columns) {
						columns = columns[0].slice(1, columns[0].length - 1).match(new RegExp("\\[.*?][ ].*?([,]|[ ]|[)])", "g")) || [];
						columns.forEach(function (column) {
							column.substring(0, column.length - 1);
							const data = column.trim().split(" ");
							const name = data[0];
							let type;
							if (data[1].indexOf("INTEGER") === 0) {
								type = "int";
							} else if (data[1].indexOf("TEXT") === 0) {
								type = "string";
							} else if (data[1].indexOf("REAL") === 0) {
								type = "number";
							} else {
								throw "datatype not supported " + column;
							}
							types[name.slice(1, name.length - 1)] = type;
						});
					}
					dbColumns[table.name] = types;
				}
				deferred.resolve(dbColumns);
			});
		});
		return deferred.promise();
	}

	return retrieveSchemaFromDb().then(function (dbColumns) {
		Object.keys(tmpSchemaInfo.tables).forEach(function (plugin) {
			Object.keys(tmpSchemaInfo.tables[plugin]).forEach(function (type) {
				const table = tmpSchemaInfo.tables[plugin][type];
				if (table.hasOwnProperty("ExtensionValues")) {
					const ev = table["ExtensionValues"];
					if (tmpSchemaInfo.complexTypes.hasOwnProperty(ev.type)) {
						throw "complex type already added " + ev.type;
					}
					tmpSchemaInfo.complexTypes[ev.type] = {};
					Object.keys(ev.defaultValue).forEach(function (property) {
						const tableName = plugin.replace(".", "") + "_" + type;
						const dataType = dbColumns[tableName]["ExtensionValues__" + property];
						if (dataType) {
							if (tmpSchemaInfo.complexTypes[ev.type].hasOwnProperty(property)) {
								throw "complex type already has this property " + property;
							}

							tmpSchemaInfo.complexTypes[ev.type][property] = {
								key: false,
								computed: false,
								type: dataType,
								elementType: null,
								defaultValue: null
							};
						}
					});
				}
			});
		});

		if (Object.keys(tmpSchemaInfo.tables).length) {
			return JSON.stringify(tmpSchemaInfo);
		}
		return null;
	});
}

const initLocalDatabaseWithoutRelations = function () {
	if (localDatabaseWithoutRelations != null) {
		return $.Deferred().resolve(localDatabaseWithoutRelations).promise();
	}
	const d = $.Deferred();
	const dbDefinitionWithoutRelations = {};
	const schemaWithoutRelations = getDatabaseSchemaWithoutRelations(window.Helper.Database.getSchema(null));
	Object.getOwnPropertyNames(schemaWithoutRelations).forEach(function (table) {
		const hasKeyProperty = Object.getOwnPropertyNames(schemaWithoutRelations[table]).some(function (propertyName) {
			return schemaWithoutRelations[table][propertyName].key === true;
		});
		if (hasKeyProperty) {
			$data.Entity.extend("Crm.Offline.DatabaseModelWithoutRelations." + table, schemaWithoutRelations[table]);
			dbDefinitionWithoutRelations[table] = {
				type: $data.EntitySet,
				elementType: namespace("Crm.Offline.DatabaseModelWithoutRelations")[table]
			};
		}
	});
	window.Crm.Offline.LmobileDatabaseWithoutRelations = $data.EntityContext.extend("window.Crm.Offline.LmobileDatabaseWithoutRelations", dbDefinitionWithoutRelations);
	const storageOptions = window.Helper.Database.getStorageOptions();
	if (storageOptions.provider === "oData" || storageOptions.provider === "local")
		storageOptions.queryCache = false;
	const databaseWithoutRelations = new window.Crm.Offline.LmobileDatabaseWithoutRelations(storageOptions);
	databaseWithoutRelations.onReady({
		success: function () {
			localDatabaseWithoutRelations = databaseWithoutRelations;
			d.resolve(localDatabaseWithoutRelations);
		},
		error: function (e) {
			window.Log.error("LmobileDatabaseWithoutRelations could not be initialized: " + e.message);
			d.reject(e);
		}
	});
	return d.promise();
};

const reloadReplicationGroupSettings = async function () {
	if (!window.database.MainReplication_ReplicationGroupSetting) {
		replicationGroupSettings = {};
		return;
	}
	let results = await window.database.MainReplication_ReplicationGroupSetting.filter(it => it.IsEnabled === true).toArray();
	replicationGroupSettings = results.reduce((previousValue, currentValue) => {
		previousValue[currentValue.Name] = currentValue.Parameter;
		return previousValue;
	}, {});
}


window.Helper.Sync ||= {

	init: initLocalDatabaseWithoutRelations,

	syncToServer: function (progressHandler) {
		const d = $.Deferred();
		(async function syncToServerAsync(){
			if (syncActive === true) {
				const errorMessage = "Helper.Sync.syncToServer: sync already active";
				window.Log.warn(errorMessage);
				return d.reject(errorMessage).promise();
			}
			syncActive = true;
			const syncDone = function () {
				syncActive = false;
				d.resolve();
			};
			await initLocalDatabaseWithoutRelations();
			try {
				await syncToServer(progressHandler);
				syncDone();
			} catch (e) {
				d.reject(e);
			}
		})();
		return d.promise();
	},

	syncFromServer: function (progressHandler) {
		const d = $.Deferred();
		if (syncActive === true) {
			const errorMessage = "Helper.Sync.syncFromServer: sync already active";
			window.Log.warn(errorMessage);
			return d.reject(errorMessage).promise();
		}
		syncActive = true;
		const syncDone = function () {
			syncActive = false;
			lastCompleteSync = new Date();
			window.Helper.Database.saveToLocalStorage("lastSync", JSON.stringify(lastCompleteSync));
			d.resolve();
		};
		initLocalDatabaseWithoutRelations().then(function () {
			window.Helper.Database.getDefinitions().then(function (definitions) {
				$.ajax({
					method: "GET",
					url: window.Helper.Url.resolveUrl("~/Crm.Offline/Sync/WaitForPostings"),
					cache: false,
					timeout: window.Crm.Offline.Settings.WaitForPostingServiceTimeoutInSec * 1000
				}).always(function () {
					reloadReplicationGroupSettings().then(function() {
						syncFromServer(definitions, progressHandler).then(syncDone).fail(function (e) {
							d.reject(e);
						});
					}).catch(d.reject);
					
				});
			});
		}).fail(function () {
			d.reject();
		});

		return d.promise();
	},

	resetCurrentSyncStatus: function () {
		window.Helper.Database.saveToLocalStorage("syncStatus", null);
	},

	migrate: function () {
		let deferred = null;
		if (migrationNeeded.length === 0) {
			isMigrationFinished = true;
			deferred = $.Deferred().resolve().promise();
		} else {
			deferred = window.Helper.Sync.syncToServer(function () {
			}).then(function () {
				if (isMigrationFinished) {
					resetMigratedClientIds();
					window.Helper.Sync.resetCurrentSyncStatus();
				}
			});

		}

		deferred.then(function () {
			if (isMigrationFinished) {
				return baseGetRawDefinitions().then(function (newDefinitions) {
					window.Helper.Database.saveToLocalStorage("SchemaInfo", JSON.stringify(newDefinitions));
					window.Helper.Database.getLocalStorageKeys().filter(function (key) {
						return key.indexOf("SchemaInfo_") === 0;
					}).forEach(function (key) {
						window.Helper.Database.removeFromLocalStorage(key);
					});
				});
			}
			return $.Deferred().resolve().promise();
		})
			.then(function () {
				if (!isMigrationFinished || migrationNeeded.length) {
					window.location.reload();
					return $.Deferred().promise();
				}
				return $.Deferred().resolve().promise();
			});
		return deferred;
	},

	abortSync: function () {
		if (request != null && request.state() === "pending") {
			request.abort();
		}
		syncActive = false;
	},

	lastCompleteSync: function () {
		return lastCompleteSync;
	}
};
