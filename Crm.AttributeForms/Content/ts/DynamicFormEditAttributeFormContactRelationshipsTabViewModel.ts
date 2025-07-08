///<reference path="../../../../Content/@types/index.d.ts" />
import {namespace} from "../../../../Content/ts/namespace";

class DynamicFormEditAttributeFormContactRelationshipsTabViewModel extends window.Main.ViewModels.ViewModelBase {
	contactTypes = ko.observableArray([]);

	constructor() {
		super();
	}

	async init(): Promise<void> {
		let contactTypes = await fetch(window.Helper.Url.resolveUrl("~/Crm.AttributeForms/AttributeFormContactRelationship/GetContactTypes")).then(x => x.json());
		contactTypes = contactTypes.sort((a, b) => a.Value.localeCompare(b.Value));
		contactTypes.unshift({Key: null, Value: window.Helper.String.getTranslatedString("PleaseSelect")});
		this.contactTypes(contactTypes);
	}
}

namespace("Crm.DynamicForms.ViewModels").DynamicFormEditAttributeFormContactRelationshipsTabViewModel = DynamicFormEditAttributeFormContactRelationshipsTabViewModel;