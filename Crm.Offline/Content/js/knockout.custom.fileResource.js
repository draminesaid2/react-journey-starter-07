;
(function(ko) {
	var baseUpdate = ko.bindingHandlers.fileResource.update;
	ko.bindingHandlers.fileResource.update = function(element, valueAccessor) {
		if (window.Helper.Offline.status === "online") {
			baseUpdate.apply(this, arguments);
			return;
		}
		var self = this;
		var args = arguments;
		var fileResource = ko.unwrap(valueAccessor());
		if (!fileResource) {
			return;
		}
		var id = ko.unwrap(fileResource.Id);
		var fileRepository = new window.FileRepository();
		fileRepository.init().then(function() {
			var filename = ko.unwrap(fileResource.Filename);
			fileRepository.getFile(id, filename)
				.then(function(file) {
					var objectUrl = window.URL.createObjectURL(file.File);
					ko.bindingHandlers.fileResource.updateElement(element, objectUrl);
					ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
						window.URL.revokeObjectURL(objectUrl);
					});
				})
				.fail(function() {
					baseUpdate.apply(self, args);
				});
		});
	}
})(ko);