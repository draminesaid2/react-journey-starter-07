/// <reference path="OfflineViewModel.js" />
(function () {
	window.Helper.Database.registerTable("Main_FileResource", { Content: { type: "string", defaultValue: "" } });
})();
$(function () {
	$(document).one("Initialized", function() {
		var init = function(viewModel, indicatorElement) {
			viewModel.init().then(function() {
				window.ko.applyBindings(viewModel, indicatorElement);
			});
		};
		var indicatorElements = $("#online-indicator, #online-indicator-desktop, #online-indicator-mobile");
		for (var i = 0; i < indicatorElements.length; i++) {
			var viewModel = new window.Crm.Offline.ViewModels.OfflineViewModel();
			init(viewModel, indicatorElements[i]);
		}
	});
});