;
(function(ko, helper, moment) {
	const init = window.Main.ViewModels.ContactDetailsNotesTabViewModel.prototype.init;
	window.Main.ViewModels.ContactDetailsNotesTabViewModel.prototype.init = async function() {
		const viewModel = this;
		await init.apply(viewModel, arguments);
		await window.Helper.Offline.initializeReplicationHint(viewModel, "NoteHistory", "IncompleteNoteHistoryHint");
	};
})(window.ko, window.Helper, window.moment);