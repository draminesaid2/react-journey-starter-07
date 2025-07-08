;
(function (ko) {
	ko.validationRules.add("CrmArticle_Article",
		function (entity) {
			entity.ArticleTypeKey.extend({
				required: {
					params: true,
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ArticleType"))
				}
			});
			entity.QuantityUnitKey.extend({
				required: {
					params: true,
					onlyIf: () => ko.unwrap(entity.ArticleTypeKey) === "Material" || ko.unwrap(entity.ArticleTypeKey) === "Cost",
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("QuantityUnit"))
				}
			});
			entity.ValidTo.extend({
				validation: {
					validator: function (val) {
						if (val && ko.unwrap(entity.ValidFrom)){
							return val > ko.unwrap(entity.ValidFrom)
						}
						return true;
					},
					message: window.Helper.String.getTranslatedString("RuleViolation.Greater").replace("{0}", window.Helper.String.getTranslatedString("ValidTo")).replace("{1}", window.Helper.String.getTranslatedString("ValidFrom"))
				}
			});
		});
})(window.ko);
