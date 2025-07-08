namespace("Crm.DynamicForms.OfflineModel").DynamicForm = function () {
    return window.ko.observable().config({ storage: "CrmDynamicForms_DynamicForm", model: "DynamicForm", pluginName: "Crm.DynamicForms" });
};
namespace("Crm.DynamicForms.OfflineModel").DynamicForms = function () {
    return window.ko.observableArray([]).config({ storage: "CrmDynamicForms_DynamicForm", model: "DynamicForm", pluginName: "Crm.DynamicForms" });
};
window.Helper.Database.addIndex("CrmDynamicForms_DynamicForm", ["CategoryKey"]);
window.Helper.Database.registerTable("CrmDynamicForms_DynamicForm", {
    Languages: { type: Array, elementType: "Crm.Offline.DatabaseModel.CrmDynamicForms_DynamicFormLanguage", inverseProperty: "$$unbound", defaultValue: [], keys: ["DynamicFormKey"] },
    Localizations: { type: Array, elementType: "Crm.Offline.DatabaseModel.CrmDynamicForms_DynamicFormLocalization", inverseProperty: "$$unbound", defaultValue: [], keys: ["DynamicFormId"] }
});
window.Helper.Database.registerTable("CrmDynamicForms_DynamicFormElementRule", {
    Conditions: { type: Array, elementType: "Crm.Offline.DatabaseModel.CrmDynamicForms_DynamicFormElementRuleCondition", inverseProperty: "$$unbound", defaultValue: [], keys: ["DynamicFormElementRuleId"] },
});
window.Helper.Database.addIndex("CrmDynamicForms_DynamicFormLanguage", ["DynamicFormKey"]);
window.Helper.Database.addIndex("CrmDynamicForms_DynamicFormLocalization", ["DynamicFormId"]);

window.Helper.Database.setTransactionId("CrmDynamicForms_DynamicFormResponse",
	function (response) {
		let ids = [response.DynamicFormReferenceKey];
		if (response.DynamicFormElementType === 'FileAttachmentDynamicFormElement') {
			const fileAttachmentIds = JSON.parse(response.Value);
			ids = ids.concat(fileAttachmentIds);
		}
		return new $.Deferred().resolve(ids).promise();
	});

window.Helper.Database.addIndex("CrmDynamicForms_DynamicFormFileResponse", ["DynamicFormReferenceKey"]);


window.Helper.Database.setTransactionId("CrmDynamicForms_DynamicFormFileResponse",
	function (fileResponse) {
		return fileResponse.DynamicFormReferenceKey;
	});

window.Helper.Database.setTransactionId("CrmDynamicForms_DynamicFormFileResponse",
	function (fileResponse) {
		return fileResponse.FileResourceId;
	});