;(function (ko, Helper) {
	ko.validationRules.add("CrmDynamicForms_DynamicFormLocalization", function (entity) {
		entity.Value.extend({
			required: {
				message: Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", Helper.String.getTranslatedString("Title")),
				onlyIf: function () {
					return entity.DynamicFormElementId() === null;
				},
				params: true
			}
		});
	});
})(window.ko, window.Helper);