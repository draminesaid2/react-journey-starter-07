(function() {
	// this will be replaced by odata $metadata
	if (!window.Crm) {
		window.Crm = {};
	}
	if (!window.Crm.Article) {
		window.Crm.Article = {};
	}
	if (!window.Crm.Article.Model) {
		window.Crm.Article.Model = {};
	}
	if (!window.Crm.Article.Model.Enums) {
		window.Crm.Article.Model.Enums = {};
	}
	if (!window.Crm.Article.Model.Enums.DiscountType) {
		window.Crm.Article.Model.Enums.DiscountType = {
			NoDiscount: 0,
			Percentage: 1,
			Absolute: 2
		};
	}
})();