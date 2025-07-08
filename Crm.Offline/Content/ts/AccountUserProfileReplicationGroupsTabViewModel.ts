import {namespace} from "../../../../Content/ts/namespace";

export class AccountUserProfileReplicationGroupsTabViewModel extends window.Main.ViewModels.ViewModelBase {
	user: KnockoutObservable<any>;
	replicationGroups = ko.observableArray<any>([]);
	replicationGroupSettings = ko.observableArray<any>([]);

	constructor() {
		super();
	}

	async init(parentViewModel): Promise<void> {
		this.user = parentViewModel.user;
		const replicationGroups = await window.Helper.Lookup.getLocalized("Main_ReplicationGroup");
		this.replicationGroups(replicationGroups.filter(x => x.Key && window.database[x.TableName]));
		await window.database.MainReplication_ReplicationGroupSetting.toArray(this.replicationGroupSettings);
		this.replicationGroupSettings().forEach(replicationGroupSetting => {
			window.database.attachOrGet(replicationGroupSetting.innerInstance);
		});
		this.replicationGroups().forEach(replicationGroup => {
			let replicationGroupSetting = this.replicationGroupSettings().find(x => x.Name() === replicationGroup.Key);
			if (!replicationGroupSetting) {
				replicationGroupSetting = window.database.MainReplication_ReplicationGroupSetting.defaultType.create().asKoObservable();
				replicationGroupSetting.Name(replicationGroup.Key);
				if (window.Helper.Offline.status === "offline") {
					replicationGroupSetting.ItemStatus(window.ko.ItemStatus.Draft);
					window.database.add(replicationGroupSetting.innerInstance);
				}
				this.replicationGroupSettings.push(replicationGroupSetting);
			}
			replicationGroup.Setting = replicationGroupSetting;
		});
	}

	async saveReplicationGroupSettings(): Promise<void> {
		let errors = ko.validation.group(this.replicationGroupSettings());
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);
		await window.database.saveChanges();
		this.replicationGroupSettings().forEach(replicationGroupSetting => {
			window.database.attachOrGet(replicationGroupSetting.innerInstance);
		});
		this.loading(false);
		this.showSnackbar(window.Helper.String.getTranslatedString("Stored"));
	}
}

namespace("Main.ViewModels").AccountUserProfileReplicationGroupsTabViewModel = AccountUserProfileReplicationGroupsTabViewModel;