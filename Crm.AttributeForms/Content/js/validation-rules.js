;
(function(ko) {
	ko.validationRules.add("CrmAttributeForms_AttributeForm",
		function(entity) {
			entity.DynamicFormKey.extend({
				required: {
					params: true,
					message: window.Helper.String.getTranslatedString("RuleViolation.Required")
						.replace("{0}", window.Helper.String.getTranslatedString("AttributeForm"))
				}
			});
		});
})(window.ko);