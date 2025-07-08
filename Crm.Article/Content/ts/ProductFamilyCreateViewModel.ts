///<reference path="../../../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace";

export class ProductFamilyCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	lookups: LookupType = {
		productFamilyStatuses: { $tableName: "CrmArticle_ProductFamilyStatus" }
	}
	productfamily = ko.observable<Crm.Article.Rest.Model.ObservableCrmArticle_ProductFamily>(null);
	errors: any;

	constructor() {
		super();
		this.productfamily = window.ko.observable(null);
	}

	async init(id?, params?): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		const productfamily = window.database.CrmArticle_ProductFamily.defaultType.create();
		//@ts-ignore
		const currentUserName = document.getElementById("meta.CurrentUser").content;
		productfamily.ResponsibleUser = currentUserName;
		productfamily.StatusKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.productFamilyStatuses, productfamily.StatusKey);
		window.database.add(productfamily);
		this.productfamily(productfamily.asKoObservable());

		if (params) {
			if (params.parentId) {
				this.productfamily().ParentId(params.parentId);
			}
			if (params.parentName) {
				this.productfamily().ParentName(params.parentName);
			}
		}

		this.productfamily().Name.extend({
			validation: {
				async: true,
				validator: function (name, params, callback) {
					if (!name) {
						callback(true);
						return;
					}
					window.database.CrmArticle_ProductFamily
						.filter(function (productFamily) { return productFamily.Name === this.name.trim(); }, { name: name })
						.count()
						.then(function (productFamily) {
							callback(productFamily === 0);
						});
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("ProductFamily"))
			}
		});
	}

	cancel(): void {
		window.database.detach(this.productfamily().innerInstance);
		window.history.back();
	}

	async submit(): Promise<void> {
		this.loading(true);
		this.errors = window.ko.validation.group(this.productfamily, { deep: true });
		await this.errors.awaitValidation();

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			return;
		}

		try {
			await window.database.saveChanges();
			window.location.hash = "/Crm.Article/ProductFamily/DetailsTemplate/" + this.productfamily().Id();

			if (ko.unwrap(this.productfamily().ParentId) !== null) {
				this.showSnackbar(window.Helper.String.getTranslatedString("ChildProductFamilyCreated"));
			} else {
				this.showSnackbar(window.Helper.String.getTranslatedString("ProductFamilyCreated"));
			}
			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	}
}
namespace("Crm.Article.ViewModels").ProductFamilyCreateViewModel = ProductFamilyCreateViewModel;
