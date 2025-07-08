(function(ko) {
	function canRestoreLastPage() {
		var canRestoreWasSet = localStorage["restoreLastPage"] === "true";
		var lastPageIsStored = localStorage["lastPage"] != null && localStorage["lastPage"] !== "";
		return canRestoreWasSet && lastPageIsStored;
	};

	function resetCanRestoredLastPage() {
		localStorage["restoreLastPage"] = false;
	};

	function isSyncRequired(routeValues) {
		var canRestore = canRestoreLastPage();
		resetCanRestoredLastPage();
		if (!canRestore) {
			return true;
		}

		var initSyncNotDone = localStorage["lastSync"] == null || localStorage["lastSync"] === "";
		if (initSyncNotDone) {
			return true;
		}

		var performSyncRequestedByUser = !!routeValues && !!routeValues.sync;
		if (performSyncRequestedByUser) {
			return true;
		}

		return false;
	};


	var init = window.Main.ViewModels.HomeStartupViewModel.prototype.init;
	window.Main.ViewModels.HomeStartupViewModel.prototype.init = function (onlineStatus, routeValues) {
		var model = this;
		var onlineStatusChanged = (onlineStatus === "online" || onlineStatus === "offline") && window.Helper.Offline.status !== onlineStatus;
		if (onlineStatusChanged && window.Helper.Offline.transientItemCount() === 0) {
			window.Helper.Offline.status = onlineStatus;
			if (!!window.database) {
				// database provider & schema has to be initialized again
				window.location.reload();
				return new $.Deferred().promise();
			}
		}
		if (window.Helper.Offline.status === "offline") {
			var databaseMigrationStep = {
				cancellable: ko.observable(true),
				description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "DatabaseMigration")),
				once: true,
				run: window.Helper.Sync.migrate,
				status: ko.observable(null),
				priority: 790
			};
			model.steps.push(databaseMigrationStep);
		
			if (window.Helper.Offline.transientItemCount() > 0) {
				var dataSynchronizationToServerStep = {
					cancellable: ko.observable(onlineStatus !== "online"),
					completedItems: ko.observable(null),
					completion: ko.observable(0),
					description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "DataSynchronizationToServer")),
					details: ko.observable(null),
					run: window.Helper.Sync.syncToServer.bind(this, dataSynchronizationToServerProgressHandler),
					status: ko.observable(null),
					totalItems: ko.observable(null),
					priority: 785
				};

				function dataSynchronizationToServerProgressHandler(i, n) {
					if (typeof i === "string") {
						dataSynchronizationToServerStep.details(window.Helper.String.getTranslatedString(i));
					} else {
						dataSynchronizationToServerStep.completion(Math.round(i * 100 / n));
						dataSynchronizationToServerStep.completedItems(i - 1);
						dataSynchronizationToServerStep.totalItems(n);
					}
				}

				model.steps.push(dataSynchronizationToServerStep);

				if (onlineStatus === "online") {
					var switchToOnlineModeStep = {
						cancellable: ko.observable(false),
						description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "SwitchToOnlineMode")),
						run: function () {
							window.Helper.Offline.status = onlineStatus;
							window.location.reload();
						},
						status: ko.observable(null),
						priority: dataSynchronizationToServerStep.priority - 1
					};
					model.steps.push(switchToOnlineModeStep);
				}
			}

			var dataSynchronizationFromServerStep = {
				cancellable: ko.observable(true),
				completedItems: ko.observable(null),
				completion: ko.observable(0),
				description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "DataSynchronizationFromServer")),
				details: ko.observable(null),
				detailsCompletion: ko.observable(0),
				run: window.Helper.Sync.syncFromServer.bind(this, dataSynchronizationFromServerProgressHandler),
				status: ko.observable(null),
				totalItems: ko.observable(null),
				priority: 780,
				subCompletedItems: ko.observable(null),
				subTotalItems: ko.observable(null)
			};

			function dataSynchronizationFromServerProgressHandler(i, n, sub, subI, subN) {
				if (!!i && !!n) {
					dataSynchronizationFromServerStep.completion(Math.round(i * 100 / n));
					dataSynchronizationFromServerStep.completedItems(i - 1);
					dataSynchronizationFromServerStep.totalItems(n);
				}
				if (!!sub) {
					dataSynchronizationFromServerStep.details(window.Helper.String.getTranslatedString(sub));
					if (!!subI && !!subN) {
						dataSynchronizationFromServerStep.subCompletedItems(subI);
						dataSynchronizationFromServerStep.subTotalItems(subN);
						dataSynchronizationFromServerStep.detailsCompletion(Math.round(subI * 100 / subN));
					} else {
						dataSynchronizationFromServerStep.detailsCompletion(0);
					}
				}
			}

			model.steps.push(dataSynchronizationFromServerStep);

			var registerNumberingServiceStep = {
				cancellable: ko.observable(true),
				description: ko.pureComputed(window.Helper.String.getTranslatedString.bind(null, "RegisterNumberingService")),
				run: window.Crm.Offline.Bootstrapper.registerNumberingService,
				status: ko.observable(null),
				priority: 775
			};
			model.steps.push(registerNumberingServiceStep);
		}

		if (!isSyncRequired(routeValues)) {
			if (model.cancellable() === true) {
				model.cancel();
			} else {
				model.cancellable.subscribe(function(cancellable) {
					if (cancellable === true) {
						model.cancel();
					}
				});
			}
		}
		
		model.errorMessage.subscribe(errorMessage => {
			let multipleTabsErrorMessage = window.Helper.String.getTranslatedString("MultipleTabsErrorMessage");
			if (errorMessage.indexOf(multipleTabsErrorMessage) === -1 && errorMessage.indexOf("Access Handles cannot be created if there is another open Access Handle or Writable stream associated with the same file") !== -1){
				model.errorMessage(multipleTabsErrorMessage + "\n" + model.errorMessage());
			}
		})
		
		return init.apply(this, arguments);
	}

	var cancel = window.Main.ViewModels.HomeStartupViewModel.prototype.cancel;
	window.Main.ViewModels.HomeStartupViewModel.prototype.cancel = function() {
		window.Helper.Sync.abortSync();
		window.Helper.Sync.resetCurrentSyncStatus();
		cancel.apply(this, arguments);
	}
})(window.ko);