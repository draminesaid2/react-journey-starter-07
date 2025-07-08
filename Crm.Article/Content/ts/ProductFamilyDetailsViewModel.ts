///<reference path="../../../../Content/@types/index.d.ts" />
import { Breadcrumb } from "../../../../Content/ts/breadcrumbs";
import { namespace } from "../../../../Content/ts/namespace";

export class ProductFamilyDetailsViewModel extends window.Main.ViewModels.ContactDetailsViewModel {

	lookups: LookupType = {
		productFamilyStatuses: { $tableName: "CrmArticle_ProductFamilyStatus" },
		languages: { $tableName: "Main_Language", $filterExpression: "it.IsSystemLanguage === true" }
	};

	tabs = ko.observable<any>({});
	loading = ko.observable<boolean>(true);
	productfamily = ko.observable<Crm.Article.Rest.Model.ObservableCrmArticle_ProductFamily>(null);
	descriptions = ko.observableArray<any>([]);
	productfamilyDescriptions = ko.observableArray<Crm.Article.Model.Lookups.CrmArticle_ProductFamilyDescription>([]);

	constructor() {
		super();
		this.contactType("ProductFamily");
	}

	settableStatuses: KnockoutComputed<any> = window.ko.pureComputed(() => {
		return this.lookups.productFamilyStatuses.$array.filter(function (status) {
			return status !== null
		});
	});
	canSetStatus: KnockoutComputed<boolean> = window.ko.pureComputed(() => {
		return this.settableStatuses().length > 1;
	});

	async init(id): Promise<void> {
		this.contactId(id);
		await super.init(this.contactId());
		const productfamily = await window.database.CrmArticle_ProductFamily
			.include("ParentProductFamily")
			.include("ResponsibleUserUser")
			.include2("ChildProductFamilies.orderBy(function(x) { return x.Name; })")
			.find(id);
		this.productfamily(productfamily.asKoObservable());
		this.contact(this.productfamily());
		this.contactName(this.productfamily().Name());
		this.loading(false);

		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		const productfamilyDescriptions = await window.database.CrmArticle_ProductFamilyDescription
			.filter("it.Key === this.Key", { Key: this.productfamily().Id() })
			.toArray();
		this.productfamilyDescriptions(productfamilyDescriptions);
		this.descriptions(this.lookups.languages.$array.slice(1).map(function (x) {
			const productfamilyDescription = productfamilyDescriptions.find(function (it) {
				return it.Language === x.Key;
			});
			return {
				Value: window.ko.observable(productfamilyDescription ? productfamilyDescription.Value : "").extend({
					maxLength: {
						params: 150,
						message: window.Helper.String.getTranslatedString("RuleViolation.MaxLength").replace("{0}", window.Helper.String.getTranslatedString("Value")),
					}
				}),
				Language: x.Key, 
				Name: x.Value,
				ProductFamilyDescription: productfamilyDescription
			};
		}));
		await this.setVisibilityAlertText();
		await this.setBreadcrumbs(id);
	}

	async setBreadcrumbs(id) {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ProductFamily"), "#/Crm.Article/ProductFamilyList/IndexTemplate"),
			new Breadcrumb(this.productfamily().Name(), window.location.hash, null, id)
		]);
	};

	async onAfterSavePmbBlock() {
		await this.init(this.contactId());
	};

	onCancelPmbBlock(): void {
		this.descriptions().forEach(description => {
			if (description.ProductFamilyDescription){
				description.Value(description.ProductFamilyDescription.Value)
			} else {
				description.Value("");
			}
		});
	};

	onSaveDescriptions(): void {
		const self = this;
		this.loading(true);
		
		let errors = ko.validation.group(this.descriptions(), { deep: true });
		if (errors().length > 0) {
			errors.showAllMessages();
			errors.scrollToError();
			throw errors();
		}
		
		this.descriptions().forEach(function (it) {
			if (self.productfamilyDescriptions()
				.map(function (x) {
					return x.Language;
				})
				.includes(it.Language)) {
				const productfamilyDescription = self.productfamilyDescriptions().find(function (x) {
					return x.Language === it.Language;
				});
				window.database.attachOrGet(productfamilyDescription);
				productfamilyDescription.Key = self.productfamily().Id();
				productfamilyDescription.Value = window.ko.unwrap(it.Value);
				if (!window.ko.unwrap(it.Value)) {
					window.database.remove(productfamilyDescription);
				}
			} else if (window.ko.unwrap(it.Value)) {
				const productfamilyDescription = window.database.CrmArticle_ProductFamilyDescription.defaultType.create();
				productfamilyDescription.Key = self.productfamily().Id();
				productfamilyDescription.Language = it.Language;
				productfamilyDescription.Value = window.ko.unwrap(it.Value);
				window.database.add(productfamilyDescription);
			}
		});
	}

	async setStatus(status): Promise<void> {
		const currentStatus = this.productfamily().StatusKey();
		if (status && status !== currentStatus) {
			window.database.attachOrGet(this.productfamily().innerInstance);
			this.productfamily().StatusKey(status.Key);
			try {
				this.loading(true);
				await window.database.saveChanges();
				this.loading(false);
			} catch (e) {
				this.loading(false);
				window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			}
		}
	}

}
namespace("Crm.Article.ViewModels").ProductFamilyDetailsViewModel = ProductFamilyDetailsViewModel;

