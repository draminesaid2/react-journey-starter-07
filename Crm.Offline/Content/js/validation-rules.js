/// <reference path="../../../../Content/js/knockout.custom.material.js" />
/// <reference path="../../../../Content/js/knockout.custom.validation.js" />
;
(function (ko) {
	ko.validationRules.add("MainReplication_ReplicationGroupSetting", function (entity) {
		entity.Parameter.extend({
			validation:
				[
					{
						rule: "min",
						params: 0,
						message: window.Helper.String.getTranslatedString("RuleViolation.NotNegative").replace("{0}", window.Helper.String.getTranslatedString("Value")),
						onlyIf: function () {
							return entity.Name() === "NoteHistory";
						}
					},
					{
						rule: "max",
						params: 36500,
						message: window.Helper.String.getTranslatedString("RuleViolation.LessOrEqual").replace("{0}", window.Helper.String.getTranslatedString("Value")).replace("{1}", 36500),
						onlyIf: function () {
							return entity.Name() === "NoteHistory";
						}
					},
					{
						rule: "required",
						params: true,
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Value")),
						onlyIf: function () {
							return entity.IsEnabled();
						}
					}
				]
		});
	});
})(window.ko);