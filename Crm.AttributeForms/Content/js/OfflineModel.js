window.Helper.Database.registerTable("CrmAttributeForms_AttributeForm", {
	DynamicForm: { type: "Crm.Offline.DatabaseModel.CrmDynamicForms_DynamicForm", inverseProperty: "$$unbound", keys: ["DynamicFormKey"] },
	Responses: { type: "Array", elementType: "Crm.Offline.DatabaseModel.CrmDynamicForms_DynamicFormResponse", inverseProperty: "$unbound", defaultValue: [], keys: ["DynamicFormReferenceKey"] }
});

window.Helper.Database.setTransactionId("CrmAttributeForms_AttributeForm",
	function (attributeForm) {
		return new $.Deferred().resolve([attributeForm.Id, attributeForm.ReferenceKey]).promise();
	});