/// <reference path="formdesignerviewmodel.js" />
;
(function($, ko) {
	window.FormDesignerViewModel = window.FormDesignerViewModel || function(){};
	window.FormDesignerViewModel.prototype.createFileResource = function(file, content, size) {
		var currentUser = window.Helper.User.getCurrentUserName();
		var fileResource = window.database.Main_FileResource.Main_FileResource.create();
		fileResource.Content = content;
		fileResource.ContentType = file.type;
		fileResource.CreateDate = new Date();
		fileResource.CreateUser = currentUser;
		fileResource.Filename = file.name;
		fileResource.Length = size;
		fileResource.ModifyDate = new Date();
		fileResource.ModifyUser = currentUser;
		fileResource.OfflineRelevant = true;
		return fileResource;
	};
	window.FormDesignerViewModel.prototype.handleFileSelect = function(data, event) {
		var viewModel = this;
		if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
			var fileApiAlertString = window.Helper.getTranslatedString("M_FileApiNotSupported");
			alert(fileApiAlertString);
			return false;
		}
		viewModel.loading(true);
		var file = event.target.files[0];
		viewModel.processFile(file).then(function(fileResource) {
			window.database.add(fileResource);
			window.Helper.Database.registerDependency(viewModel.selectedFormElement(), fileResource);
			if (data.FileResourceId()) {
				var oldFileResource = window.database.Main_FileResource.attachOrGet({ Id: data.FileResourceId() });
				window.database.Main_FileResource.remove(oldFileResource);
				window.Helper.Database.registerDependency(oldFileResource, viewModel.selectedFormElement());
				data.FileResourceId(null);
			}
			data.FileResource = fileResource.asKoObservable();
			data.FileResourceId(fileResource.Id);
			event.target.value = null;
			viewModel.loading(false);
		}).fail(function(error) {
			window.Log.error("Processing file failed: " + error);
		});
		return true;
	};
	window.FormDesignerViewModel.prototype.processFile = function(file) {
		var viewModel = this;
		var d = new $.Deferred();
		var reader = new FileReader();
		reader.onload = function(event) {
			var base64String = event.target.result.split(",")[1];
			var fileResource = viewModel.createFileResource(file, base64String, file.size);
			d.resolve(fileResource);
		};
		reader.onerror = d.reject;
		reader.readAsDataURL(file);
		return d.promise();
	};
	var maxFileSize = null;
	function getMaxFileSize() {
		if (maxFileSize) {
			return new $.Deferred().resolve(maxFileSize).promise();
		}
		return $.get(window.Helper.Url.resolveUrl("~/Setting/Main.Settings.MaxFileLengthInKb.json")).then(function(result) {
			maxFileSize = parseInt(result);
			return maxFileSize;
		});
	}
	ko.validationRules.add("CrmDynamicForms_Image",
		function(entity) {
			ko.validation.addRule(entity.FileResourceId,
				{
					rule: "required",
					message: window.Helper.String.getTranslatedString("RuleViolation.Required")
						.replace("{0}", window.Helper.String.getTranslatedString("File")),
					params: true
				});
			ko.validation.addRule(entity.FileResourceId,
				{
					async: true,
					onlyIf: function() {
						return entity.FileResource;
					},
					validator: function(val, params, callback) {
						getMaxFileSize().then(function(maxFileLengthInKb) {
							if (entity.FileResource && entity.FileResource.Length() > maxFileLengthInKb * 1000) {
								var fileSizeString = maxFileLengthInKb / 1000 + " MB";
								callback({ isValid: false, message: window.Helper.String.getTranslatedString("MaximumAllowedFilesize") + " " + fileSizeString });
							} else {
								callback({ isValid: true });
							}
						});
					}
				});
		});
})(window.jQuery, window.ko);