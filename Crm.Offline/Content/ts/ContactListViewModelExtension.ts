(function () {
	const baseInit = window.Main.ViewModels.ContactListViewModel.prototype.init;
	window.Main.ViewModels.ContactListViewModel.prototype.init = function init(id) {
		const viewModel = this;
		const args = arguments;
		return $.Deferred().resolve().promise()
			.then(function () {
				return baseInit.apply(viewModel, args);
			})
			.then(function () {
				if (this.replicationHintInfo != null) {
					return window.Helper.Offline.initializeReplicationHint(this, this.replicationHintInfo.SettingName, this.replicationHintInfo.HintTranslationKey);
				} else {
					return $.Deferred().resolve().promise();
				}
			});
	};
})();
