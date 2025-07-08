namespace("Crm.Article.OfflineModel").Article = function () {
	return window.ko.observable().config({ storage: "CrmArticle_Article", model: "Article", pluginName: "Crm.Article" });
};
namespace("Crm.Article.OfflineModel").ProductFamily = function () {
	return window.ko.observable().config({ storage: "CrmArticle_ProductFamily", model: "ProductFamily", pluginName: "Crm.Article" });
};
namespace("Crm.Article.OfflineModel").Articles = function () {
    return window.ko.observableArray([]).config({ storage: "CrmArticle_Article", model: "Article", pluginName: "Crm.Article" });
};
namespace("Crm.Article.OfflineModel").QuantityUnits = function () {
	return window.ko.observableArray([]).config({ storage: "CrmArticle_QuantityUnit", model: "QuantityUnit", pluginName: "Crm.Article" });
};
namespace("Crm.Article.OfflineModel").ArticleDescription = function () {
	return window.ko.observableArray([]).config({ storage: "CrmArticle_ArticleDescription", model: "ArticleDescription", pluginName: "Crm.Article" });
};
window.Helper.Database.registerTable("CrmArticle_Article", {
	DocumentAttributes: { type: "Array", elementType: "Crm.Offline.DatabaseModel.Main_DocumentAttribute", inverseProperty: "$$unbound", defaultValue: [], keys: ["ReferenceKey"] },
	ProductFamily: { type: "Crm.Offline.DatabaseModel.CrmArticle_ProductFamily", inverseProperty: "$$unbound", defaultValue: null, keys: ["ProductFamilyKey"] },
	Tags: { type: "Array", elementType: "Crm.Offline.DatabaseModel.Main_Tag", inverseProperty: "$$unbound", defaultValue: [], keys: ["ContactKey"] }
});

window.Helper.Database.registerConverter("ProductFamily", "CrmArticle_ProductFamily");
window.Helper.Database.registerConverter("ParentProductFamily", "CrmArticle_ProductFamily");
window.Helper.Database.registerConverter("Child", "CrmArticle_ProductFamily");
window.Helper.Database.registerTable("CrmArticle_ProductFamily", {
	ParentProductFamily: { type: "ParentProductFamily", inverseProperty: "ChildProductFamilies", defaultValue: null, keys: ["ParentId"] },
	ChildProductFamilies: { type: "Array", elementType: "Child", inverseProperty: "ParentProductFamily", defaultValue: [], keys: ["ParentId"] },
	ResponsibleUserUser: { type: "Crm.Offline.DatabaseModel.Main_User", inverseProperty: "$$unbound", defaultValue: null, keys: ["ResponsibleUser"] }

});

window.Helper.Database.addIndex("CrmArticle_Article", ["ArticleTypeKey"]);
window.Helper.Database.addIndex("CrmArticle_Article", ["ArticleTypeKey", "ItemNo", "Description"]);

window.Helper.Database.setTransactionId("CrmArticle_ProductFamily",
	function (productfamily) {
		return new $.Deferred().resolve([productfamily.Id, productfamily.ParentId]).promise();
});

window.Helper.Database.registerTable("CrmArticle_ArticleCompanyRelationship", {
	Parent: { type: "Crm.Offline.DatabaseModel.CrmArticle_Article", inverseProperty: "$$unbound", defaultValue: null, keys: ["ParentId"] },
	Child: { type: "Crm.Offline.DatabaseModel.Main_Company", inverseProperty: "$$unbound", defaultValue: null, keys: ["ChildId"] },

});

window.Helper.Database.registerTable("CrmArticle_ArticleRelationship", {
	Parent: { type: "Crm.Offline.DatabaseModel.CrmArticle_Article", inverseProperty: "$$unbound", defaultValue: null, keys: ["ParentId"] },
	Child: { type: "Crm.Offline.DatabaseModel.CrmArticle_Article", inverseProperty: "$$unbound", defaultValue: null, keys: ["ChildId"] },

});

window.Helper.Database.addIndex("CrmArticle_ArticleCompanyRelationship", ["ChildId"]);

window.Helper.Database.setTransactionId("CrmArticle_ArticleCompanyRelationship",
	function (relationship) {
		return new $.Deferred().resolve([relationship.ParentId, relationship.ChildId]).promise();
	});