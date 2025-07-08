///<reference path="../../../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace"

class FlowRuleEditModalViewModel extends window.Main.ViewModels.ViewModelBase {

	rule: KnockoutObservable<Main.Flow.Rest.Model.ObservableMainFlow_FlowRule>;
	actions: object[];

	constructor() {
		super();
		this.rule = window.ko.observable(null);
		this.actions = [
			{ key: "Created", value: window.Helper.String.getTranslatedString("Created") },
			{ key: "Modified", value: window.Helper.String.getTranslatedString("Modified") },
			{ key: "Deleted", value: window.Helper.String.getTranslatedString("Deleted") }
		];
	}

	async init(id: string, params: any): Promise<void> {
		if (!id) {
			this.rule(window.database.MainFlow_FlowRule.defaultType.create().asKoObservable());
			window.database.add(this.rule());
		} else {
			this.rule((await window.database.MainFlow_FlowRule.find(id)).asKoObservable());
			window.database.attachOrGet(this.rule());
		}
	}

	async save(): Promise<void> {
		this.loading(true);
		let errors = window.ko.validation.group(this.rule);
		await errors.awaitValidation();
		if (errors().length > 0) {
			this.loading(false);
			errors.showAllMessages();
			return;
		}
		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch(e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"), window.Helper.String.getTranslatedString("Error_InternalServerError"), "error");
		}
	}

	entityTypeMapForSelect2Display(entityType): object {
		return {
			id: entityType.Name,
			item: entityType,
			text: entityType.Name
		};
	}

	getElementByIdQuery(query, value) {
		return query.filter(`it.Name === '${value}'`);
	}

	dispose(): void {
		window.database.detach(this.rule());
	}
}

export function FlowRuleEditModalViewModelBuilder() {

	return new FlowRuleEditModalViewModel()
}

//@ts-ignore
namespace("Main.Flow.ViewModels").FlowRuleEditModalViewModel = FlowRuleEditModalViewModel;