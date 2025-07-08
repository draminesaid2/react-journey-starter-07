/// <reference path="../../../../node_modules/lodash/lodash.js" />
;(function () {
	window.Helper.Database.EventListeners["beforeCreate"].BeforeCreateGuidEventListener = function beforeCreateGuidEventListener(sender, items) {
		var elementType = sender.name;
		var entitySet = window.database.getEntitySetFromElementType(elementType);
		var keyProperties = entitySet.elementType.memberDefinitions.getKeyProperties();
		var isGuidType = keyProperties.length === 1 && keyProperties[0].dataType === window.$data.Guid;
		var idProperty = keyProperties[0].name;

		if (isGuidType) {
			items = Array.isArray(items) ? items : [items];
			items.forEach(function (item) {
				if (item[idProperty] == null || item[idProperty] === "00000000-0000-0000-0000-000000000000") {
					item[idProperty] = window.$data.createGuid().toString().toLowerCase();
				}
			});
		}
	};
	var hasItemStatusProperty = function(storedEntityName) {
		return window.Helper.Offline.status === "offline" && window.Helper.Database.hasProperty(storedEntityName, "ItemStatus");
	};
	window.Helper.Database.EventListeners["beforeCreate"].BeforeCreateItemStatusEventListener = {
		condition: hasItemStatusProperty,
		listener: function beforeCreateItemStatusEventListener(sender, items) {
			items = Array.isArray(items) ? items : [items];
			items.forEach(function(item) {
				if (item.ItemStatus === window.ko.ItemStatus.Draft || item.ItemStatus === window.ko.ItemStatus.Persisted) {
					item.ItemStatus = window.ko.ItemStatus.Added;
				}
			});
		}
	};
	var setLocalTimestampEventListener = {
		condition: function(storedEntityName) {
			return window.Helper.Offline.status === "offline" && window.Helper.Database.hasProperty(storedEntityName, "localTimestamp");
		},
		listener: function(sender, items) {
			items = Array.isArray(items) ? items : [items];
			items.forEach(function(item) {
				item.localTimestamp = new Date();
			});
		}
	};
	window.Helper.Database.EventListeners["beforeCreate"].BeforeCreateSetLocalTimestampEventListener = setLocalTimestampEventListener;
	window.Helper.Database.EventListeners["beforeUpdate"].BeforeUpdateSetLocalTimestampEventListener = setLocalTimestampEventListener;
	window.Helper.Database.EventListeners["beforeUpdate"].BeforeUpdateItemStatusEventListener = {
		condition: hasItemStatusProperty,
		listener: function(sender, items) {
			items = Array.isArray(items) ? items : [items];
			items.forEach(function(item) {
				if (item.ItemStatus === undefined) {
					throw "ItemStatus undefined";
				}
				if (item.ItemStatus === window.ko.ItemStatus.Persisted) {
					item.ItemStatus = window.ko.ItemStatus.Modified;
				}
			});
		}
	};
	window.Helper.Database.EventListeners["afterDelete"].AfterDeleteItemStatusEventListener = {
		condition: hasItemStatusProperty,
		listener: function afterDeleteItemStatusEventListener(sender, items) {
			items = Array.isArray(items) ? items : [items];
			if (items.length > 1) {
				throw "Batch Soft Deleting not supported";
			}
			if (items.length === 0) {
				return true;
			}
			var item = items[0];
			if (item.ItemStatus === undefined) {
				throw "ItemStatus undefined";
			}
			if (item.ItemStatus !== window.ko.ItemStatus.Added) {
				var elementType = sender.name;
				var elementSet = window.database.getEntitySetFromElementType(elementType);
				var entity = new elementSet.createNew(item);
				entity.ItemStatus = window.ko.ItemStatus.Removed;
				elementSet.add(entity);
				elementSet.saveChanges();
			}
			return true;
		}
	};
	window.Helper.Database.EventListeners["beforeDelete"].BeforeDeleteItemStatusEventListener = {
		condition: hasItemStatusProperty,
		listener: function beforeDeleteItemStatusEventListener(sender, items) {
			items = Array.isArray(items) ? items : [items];
			if (items.length > 1) {
				throw "Batch Soft Deleting not supported";
			}
			if (items.length === 0) {
				return true;
			}
			var item = items[0];
			if (item.ItemStatus === undefined) {
				throw "ItemStatus undefined";
			}
			if (item.ItemStatus === window.ko.ItemStatus.Draft) {
				return false;
			}
			return true;
		}
	};
	var setTransientItemsEventListener = {
		condition: hasItemStatusProperty,
		listener: function(sender, items) {
			var entityName = items.getType().name;
			if (entityName === "Main_NumberingSequence") {
				return;
			}
			var transientItems = Array.isArray(items) ? items : [items];
			var keyProperty = window.Helper.Database.getKeyProperty(entityName);
			transientItems.forEach(function(item) {
				var transientItemInformationIndex = window.Helper.Offline.transientItemInformation().indexOf(window._.find(window.Helper.Offline.transientItemInformation(), function(x) { return x.Id === item[keyProperty] && x.Type === entityName; }));
				if (transientItemInformationIndex === -1) {
					window.Helper.Offline.transientItemInformation.push({ Id: item[keyProperty], Type: entityName });
				}
			});
		}
	};
	window.Helper.Database.EventListeners["afterCreate"].AfterCreateSetTransientItemsEventListener = setTransientItemsEventListener;
	window.Helper.Database.EventListeners["afterUpdate"].AfterUpdateSetTransientItemsEventListener = setTransientItemsEventListener;
	window.Helper.Database.EventListeners["afterDelete"].AfterDeleteTransientItemsEventListener = {
		condition: hasItemStatusProperty,
		listener: function afterDeleteTransientItemsEventListener(sender, items) {
			var entityName = items.getType().name;
			if (entityName === "Main_NumberingSequence") {
				return true;
			}
			var deletedItems = Array.isArray(items) ? items : [items];
			var keyProperty = window.Helper.Database.getKeyProperty(entityName);
			deletedItems.forEach(function(item) {
				var transientItemInformationIndex = window.Helper.Offline.transientItemInformation().indexOf(window._.find(window.Helper.Offline.transientItemInformation(), function(x) { return x.Id === item[keyProperty] && x.Type === entityName; }));
				if (transientItemInformationIndex !== -1) {
					window.Helper.Offline.transientItemInformation.splice(transientItemInformationIndex, 1);
				}
			});
			return true;
		}
	};

	var baseInitialize = window.Helper.Database.initialize;
	window.Helper.Database.initialize = function() {
		var attached = window.Helper.Database.EventListeners.attached;
		return baseInitialize.apply(this, arguments).then(function() {
			if (attached) {
				return;
			}
			if (window.Helper.Offline.status === "offline") {
				window.Helper.Database.addGlobalFilter(function() {
					return {
						filter: "it.ItemStatus == null || it.ItemStatus !== this.itemStatus",
						filterScope: { itemStatus: window.ko.ItemStatus.Removed },
						requiredColumns: ["ItemStatus"]
					};
				});
			}
		});
	};

	var registerEventHandlers = window.Helper.Database.registerEventHandlers;
	window.Helper.Database.registerEventHandlers = function(viewModel, tableEventHandlers) {
		var wrappedTableEventHandlers = {};
		Object.keys(tableEventHandlers).forEach(function(table) {
			wrappedTableEventHandlers[table] = {};
			var tableEventHandler = tableEventHandlers[table];
			Object.keys(tableEventHandler).forEach(function (event) {
				if (tableEventHandler[event]) {
					wrappedTableEventHandlers[table][event] = function(sender, entity) {
						if (entity.ItemStatus !== window.ko.ItemStatus.Removed) {
							tableEventHandler[event].apply(viewModel, arguments);
						}
					};
				}
			});
		});
		registerEventHandlers.call(this, viewModel, wrappedTableEventHandlers);
	};
})();