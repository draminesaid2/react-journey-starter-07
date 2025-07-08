///<reference path="../../../../Content/@types/index.d.ts" />
import {namespace} from "../../../../Content/ts/namespace";

class DynamicFormCreateViewModel extends window.Main.ViewModels.ViewModelBase {

	dynamicForm = ko.observable<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicForm>(null);
	dynamicFormLocalization = ko.observable<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicFormLocalization>(null);
	file = ko.observable<Crm.Rest.Model.ObservableMain_FileResource>(null);
	errors = ko.validation.group([this.dynamicForm, this.dynamicFormLocalization, this.file], {deep: true});
	dynamicFormExport: any;

	constructor() {
		super();
	}

	cancel() {
		window.database.detach(this.dynamicForm().innerInstance);
		window.database.detach(this.dynamicFormLocalization().innerInstance);
		window.history.back();
	}

	async importFromFile(dynamicFormExport) {
		//Workaround for https://github.com/simple-odata-client/Simple.OData.Client/issues/297
		let bufferLimit = 950;
		let bufferSize = 0;
		let dynamicForm = this.dynamicForm().innerInstance;
		dynamicForm.HideEmptyOptional = dynamicFormExport.DynamicForm.HideEmptyOptional;
		dynamicForm.ExtensionValues = dynamicFormExport.DynamicForm.ExtensionValues;

		let defaultLanguageStatusKey = await window.Helper.Lookup.queryDefaultLookupKey("CrmDynamicForms_DynamicFormStatus", "Draft");
		for (const dynamicFormLanguageRest of dynamicFormExport.Languages) {
			if (dynamicFormLanguageRest.LanguageKey !== dynamicForm.DefaultLanguageKey) {
				let dynamicFormLanguage = window.database.CrmDynamicForms_DynamicFormLanguage.defaultType.create();
				dynamicFormLanguage.DynamicFormKey = dynamicForm.Id;
				(dynamicFormLanguage as any).ExtensionValues = dynamicFormLanguageRest.ExtensionValues;
				dynamicFormLanguage.LanguageKey = dynamicFormLanguageRest.LanguageKey;
				dynamicFormLanguage.StatusKey = defaultLanguageStatusKey;
				window.database.add(dynamicFormLanguage);
				bufferSize++;
				if (bufferSize === bufferLimit) {
					await window.database.saveChanges();
					bufferSize = 0;
				}
			}
		}

		let fileResourceIds = {};
		for (const fileResourceRest of dynamicFormExport.FileResources) {
			let fileResource = window.database.Main_FileResource.defaultType.create();
			fileResource.Content =  fileResourceRest.Content.$value;
			fileResource.ContentType =  fileResourceRest.ContentType;
			(fileResource as any).ExtensionValues =  fileResourceRest.ExtensionValues;
			fileResource.Filename =  fileResourceRest.Filename;
			fileResource.Length = fileResourceRest.Length;
			fileResource.OfflineRelevant = fileResourceRest.OfflineRelevant;
			window.database.add(fileResource);
			fileResourceIds[fileResourceRest.Id] = fileResource.Id;
			bufferSize++;
			if (bufferSize === bufferLimit) {
				await window.database.saveChanges();
				bufferSize = 0;
			}
		}
		
		let dynamicFormElementIds = {};
		for await (const dynamicFormElementRest of dynamicFormExport.Elements) {
			// @ts-ignore
			let dynamicFormElementType = window.database.DynamicFormElementRest.elementType.inheritedTo.find(dynamicFormElement => {
				let dynamicFormElementTypeName = dynamicFormElement.name.split("_")[1];
				return dynamicFormElementRest.$type == "Crm.DynamicForms.Rest.Model." + dynamicFormElementTypeName + "Rest, Crm.DynamicForms";
			});
			let dynamicFormElement = dynamicFormElementType.create();

			let oldDynamicFormElementId = dynamicFormElementRest.Id;
			for (let propertyName in dynamicFormElementRest) {
				if (!Array.isArray(dynamicFormElementRest[propertyName])) {
					dynamicFormElement[propertyName] = dynamicFormElementRest[propertyName];
				}
			}
			
			if (dynamicFormElement.FileResourceId){
				dynamicFormElement.FileResourceId = fileResourceIds[dynamicFormElement.FileResourceId];
			}
			
			dynamicFormElement.DynamicFormKey = dynamicForm.Id;
			dynamicFormElement.Id = window.Helper.String.emptyGuid();
			window.database.add(dynamicFormElement);
			dynamicFormElementIds[oldDynamicFormElementId] = dynamicFormElement.Id;
			bufferSize++;
			if (bufferSize === bufferLimit) {
				await window.database.saveChanges();
				bufferSize = 0;
			}
		}

		for (const dynamicFormLocalizationRest of dynamicFormExport.Localizations) {
			if (dynamicFormLocalizationRest.Value == "") {
				continue;
			}

			if (dynamicFormLocalizationRest.DynamicFormElementId === null && dynamicFormLocalizationRest.Language === dynamicForm.DefaultLanguageKey) {
				continue;
			}

			let dynamicFormLocalization = window.database.CrmDynamicForms_DynamicFormLocalization.defaultType.create();
			dynamicFormLocalization.ChoiceIndex = dynamicFormLocalizationRest.ChoiceIndex;
			dynamicFormLocalization.DynamicFormId = dynamicForm.Id;
			dynamicFormLocalization.DynamicFormElementId = dynamicFormLocalizationRest.DynamicFormElementId !== null ? dynamicFormElementIds[dynamicFormLocalizationRest.DynamicFormElementId] : null;
			(dynamicFormLocalization as any).ExtensionValues = dynamicFormLocalizationRest.ExtensionValues;
			dynamicFormLocalization.Hint = dynamicFormLocalizationRest.Hint;
			dynamicFormLocalization.Id = 0;
			dynamicFormLocalization.Language = dynamicFormLocalizationRest.Language;
			dynamicFormLocalization.Value = dynamicFormLocalizationRest.Value;
			window.database.add(dynamicFormLocalization);
			bufferSize++;
			if (bufferSize === bufferLimit) {
				await window.database.saveChanges();
				bufferSize = 0;
			}
		}

		for (const dynamicFormElementRest of dynamicFormExport.Elements) {
			for (const dynamicFormElementRuleRest of dynamicFormElementRest.Rules) {
				let dynamicFormElementRule = window.database.CrmDynamicForms_DynamicFormElementRule.defaultType.create();
				dynamicFormElementRule.DynamicFormId = dynamicForm.Id;
				dynamicFormElementRule.DynamicFormElementId = dynamicFormElementIds[dynamicFormElementRuleRest.DynamicFormElementId];
				(dynamicFormElementRule as any).ExtensionValues = dynamicFormElementRuleRest.ExtensionValues;
				dynamicFormElementRule.Id = window.Helper.String.emptyGuid();
				dynamicFormElementRule.MatchType = dynamicFormElementRuleRest.MatchType;
				dynamicFormElementRule.Type = dynamicFormElementRuleRest.Type;
				window.database.add(dynamicFormElementRule);
				bufferSize++;
				if (bufferSize === bufferLimit) {
					await window.database.saveChanges();
					bufferSize = 0;
				}
				for (const dynamicFormElementRuleConditionRest of dynamicFormElementRuleRest.Conditions) {
					let dynamicFormElementRuleCondition = window.database.CrmDynamicForms_DynamicFormElementRuleCondition.defaultType.create();
					dynamicFormElementRuleCondition.DynamicFormElementId = dynamicFormElementIds[dynamicFormElementRuleConditionRest.DynamicFormElementId];
					dynamicFormElementRuleCondition.DynamicFormElementRuleId = dynamicFormElementRule.Id;
					(dynamicFormElementRuleCondition as any).ExtensionValues = dynamicFormElementRuleConditionRest.ExtensionValues;
					dynamicFormElementRuleCondition.Filter = dynamicFormElementRuleConditionRest.Filter;
					dynamicFormElementRuleCondition.Id = window.Helper.String.emptyGuid();
					dynamicFormElementRuleCondition.Value = dynamicFormElementRuleConditionRest.Value;
					window.database.add(dynamicFormElementRuleCondition);
					bufferSize++;
					if (bufferSize === bufferLimit) {
						await window.database.saveChanges();
						bufferSize = 0;
					}
				}
			}
		}

		if(bufferSize !== 0){
			await window.database.saveChanges();
		}
	}

	async init(): Promise<void> {
		this.file(window.database.Main_FileResource.defaultType.create().asKoObservable());

		let dynamicForm = window.database.CrmDynamicForms_DynamicForm.defaultType.create();
		dynamicForm.DefaultLanguageKey = await window.Helper.Culture.languageCulture();
		window.database.add(dynamicForm);
		this.dynamicForm(dynamicForm.asKoObservable());

		let dynamicFormLanguage = window.database.CrmDynamicForms_DynamicFormLanguage.defaultType.create();
		dynamicFormLanguage.DynamicFormKey = dynamicForm.Id;
		dynamicFormLanguage.LanguageKey = dynamicForm.DefaultLanguageKey;
		dynamicFormLanguage.StatusKey = await window.Helper.Lookup.queryDefaultLookupKey("CrmDynamicForms_DynamicFormStatus", "Draft");
		window.database.add(dynamicFormLanguage);

		let dynamicFormLocalization = window.database.CrmDynamicForms_DynamicFormLocalization.defaultType.create(); 
		dynamicFormLocalization.DynamicFormId = dynamicForm.Id;
		dynamicFormLocalization.DynamicFormElementId = null;
		dynamicFormLocalization.Language = dynamicForm.DefaultLanguageKey;
		window.database.add(dynamicFormLocalization);
		this.dynamicFormLocalization(dynamicFormLocalization.asKoObservable());

		this.file().Content.subscribe((content) => {
			if (content) {
				try {
					this.dynamicFormExport = JSON.parse(decodeURIComponent(escape(atob(content))));
					this.dynamicForm().CategoryKey(this.dynamicFormExport.DynamicForm.CategoryKey);
					let localization = this.dynamicFormExport.Localizations.find(x => x.DynamicFormElementId === null && x.Language === dynamicForm.DefaultLanguageKey);
					if (localization) {
						this.dynamicFormLocalization().Hint(localization.Hint);
						this.dynamicFormLocalization().Value(localization.Value);
					}
				} catch (e) {
					this.dynamicFormExport = e;
				}
			} else {
				this.dynamicFormExport = null;
			}
		});
		this.file().Length.extend({
			validation: {
				validator: () => !(this.dynamicFormExport instanceof Error),
				message: window.Helper.String.getTranslatedString("RuleViolation.Format").replace("{0}", window.Helper.String.getTranslatedString("File")),
				onlyIf: () => !!this.file().Content()
			}
		})
	}

	async submit(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			return;
		}

		if (this.dynamicForm().CategoryKey() !== "PDF-Checklist" && this.dynamicFormExport) {
			try {
				await this.importFromFile(this.dynamicFormExport);
				window.location.hash = "/Crm.DynamicForms/DynamicForm/EditTemplate/" + this.dynamicForm().Id();
			} catch (e) {
				this.loading(false);
			}
		}

		try {
			await window.database.saveChanges();
			window.location.hash = "/Crm.DynamicForms/DynamicForm/EditTemplate/" + this.dynamicForm().Id();
		} catch (e) {
			this.loading(false);
		}
	}
}

namespace("Crm.DynamicForms.ViewModels").DynamicFormCreateViewModel = DynamicFormCreateViewModel;
