import moment from "moment";
export class HelperOffline {
	transientItemInformation: KnockoutObservableArray<any> = ko.observableArray(JSON.parse(window.Helper.Database.getFromLocalStorage("transientItems")) || []);
	signalrOnline: KnockoutObservable<boolean> = ko.observable<boolean>($.connection && $.connection.ProfilingHub && $.connection.ProfilingHub.state === "Connected");
	navigatorOnline: KnockoutObservable<boolean> = ko.observable<boolean>(window.navigator.onLine);
	online: KnockoutComputed<boolean> = ko.computed<boolean>( () => {
		// TODO: navigatorOnline() sometimes returns false in android cordova app
		// return navigatorOnline() && signalrOnline();
		return this.signalrOnline();
	});
	transientItemCount = ko.computed( () => {
		return this.transientItemInformation().length;
	});

	status: string

	constructor() {
		this.transientItemInformation.subscribe(function(data) {
			window.Helper.Database.saveToLocalStorage("transientItems", JSON.stringify(data));
		});

		window.addEventListener("offline", () => {
			this.navigatorOnline(false);
		}, false);
		window.addEventListener("online", () => {
			this.navigatorOnline(true);
		}, false);

		document.addEventListener("load", () => {
			if ($.connection && $.connection.ProfilingHub) {
				$($.connection.ProfilingHub).on("stateChanged", () => {
					this.signalrOnline($.connection.ProfilingHub.state === "Connected");
				});
			}
		});

		document.addEventListener("Initialized", () => this.signalrOnline($.connection && $.connection.ProfilingHub && $.connection.ProfilingHub.state === "Connected"));

		if (!this.hasOwnProperty("status")) {
			Object.defineProperty(this, "status", {
				get: function () {
					const availableOfflineStatuses = window.Helper.DOM.getMetadata("AvailableOfflineStatuses").split(",");
					const storedOfflineStatus = window.Helper.Database.getFromLocalStorage("offlineStatus");
					if (availableOfflineStatuses.indexOf(storedOfflineStatus) !== -1) {
						return storedOfflineStatus;
					}
					return availableOfflineStatuses[0];
				},
				set: function(status) {
					window.Helper.Database.saveToLocalStorage("offlineStatus", status);
				}
			});
		}

		const getDefaultStorageOptions = window.Helper.Database.getStorageOptions;
		window.Helper.Database.getStorageOptions = () => {
			if (this.status === "online") {
				return getDefaultStorageOptions();
			} else {
				return {
					provider: "local",
					databaseName: window.Helper.Database.getStoragePrefix() + "Lmobile",
					maxSize: 4 * 1024 * 1024,
					dbCreation: window.$data.storageProviders.DbCreationType.DropTableIfChange,
					queryCache: true
				};
			}
		};
	}

	async initializeReplicationHint(viewModel: any, settingName: string, hintTranslationKey: string): Promise<void> {
		viewModel.replicationGroups = ko.observableArray([]);
		viewModel.replicationGroupSettings = ko.observableArray([]);
		viewModel.replicationHint(null);

		if (window.Helper.Offline.status === "online" || !window.database.Main_ReplicationGroup || !window.database.MainReplication_ReplicationGroupSetting) {
			return;
		}
		const historySettings = await window.database.MainReplication_ReplicationGroupSetting.filter("it.Name === this.settingName && it.IsEnabled === true", {settingName: settingName}).take(1).toArray();
		let historySetting = historySettings.length > 0 ? historySettings[0] : null;
		let defaultHistoryParameter = null;
		if (historySetting === null) {
			const replicationGroup = await window.database.Main_ReplicationGroup.first("it.Key === this.settingName", {settingName: settingName});
			defaultHistoryParameter = replicationGroup && replicationGroup.DefaultValue;
		}
		const historySince = historySetting ? historySetting.Parameter : defaultHistoryParameter;
		if (historySince === 0) {
			return;
		}
		const maxContactHistoryDays = (viewModel.minDate && viewModel.minDate()) ? moment().diff(viewModel.minDate(), "days") : null;
		if (maxContactHistoryDays !== null && historySince >= maxContactHistoryDays) {
			return;
		}
		viewModel.replicationHint(window.Helper.String.getTranslatedString(hintTranslationKey).replace("{0}", historySince));
	}
}

// @ts-ignore
(window.Helper = window.Helper || {}).Offline = new HelperOffline();
