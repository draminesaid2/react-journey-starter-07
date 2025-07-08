(function ($, ko) {
	var baseFormDesignerViewModel = window.FormDesignerViewModel;
	window.FormDesignerViewModel = function () {
		var self = this;
		baseFormDesignerViewModel.call(self);

		var baseInit = self.init;
		self.init = function (id) {
			return baseInit(id)
				.then(function () {
					self.pageNum = ko.observable(1);
					self.pdfDoc = ko.observable(null);
					self.pageRendering = ko.observable(false);
					self.pageNumPending = ko.observable(null);
					self.pageCount = ko.observable(0);
					self.currentPage = ko.observable();
					self.fileResource = ko.observable(null);
					self.filename = ko.observable();
					self.currentFileResource = ko.observable(null);
					self.fileResourcesToRemove = ko.observableArray([]);
					return window.Helper.Culture.languageCulture()
				})
				.then(function (cultureLanguage) {
					var fileResourceId = null;

					self.form().Languages().forEach(function (language) {
						if (language.LanguageKey() == cultureLanguage && language.FileResourceId() !== null) {
							fileResourceId = language.FileResourceId();
						}

					})

					if (fileResourceId == null && self.form().Languages()[0].FileResourceId() == null) {
						return null;
					} else if(fileResourceId == null) {
						fileResourceId = self.form().Languages()[0].FileResourceId();
					}

					return window.database.Main_FileResource
						.find(fileResourceId);
				})
				.then(function (fileResource) {
					if (fileResource !== null) {
						self.currentFileResource(fileResource.asKoObservable());
						self.filename(fileResource.Filename);
						return fetch(window.Helper.resolveUrl("~/File/File/" + fileResource.Id))
							.then(response => response.blob())
							.then(blob => window.Helper.String.blobToBase64(blob))
							.then(function (base64data) {
								self.showPdf(base64data)
							})
					}
				})
				.fail(function (error) {
					window.Log.error("Cannot show pdf: " + error);
				})
		};

		self.isPdf = function () {

			if (self.form().CategoryKey() === "PDF-Checklist") {
				return true;
			}
			return false;
		}

		self.getPdfFile = function (file) {
			var viewModel = this;
			var d = new $.Deferred();
			var reader = new FileReader();
			reader.onload = function (event) {
				var base64String = event.target.result.split(",")[1];
				self.showPdf(base64String);
				var fileResource = viewModel.createFileResource(file, base64String, file.size);
				d.resolve(fileResource);
			};
			reader.onerror = d.reject;
			reader.readAsDataURL(file);
			return d.promise();

		}

		self.handlePdfUpload = function (data, event) {
			var viewModel = this;

			if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
				var fileApiAlertString = window.Helper.getTranslatedString("M_FileApiNotSupported");
				alert(fileApiAlertString);
				return false;
			}
			self.loading(true);
			var file = event.target.files[0];
			window.Log.debug("New uploaded file: ", file)

			self.getPdfFile(file)
				.then(function (fileResource) {
					var oldFileResourceId = null;
					window.database.add(fileResource)
					self.currentFileResource(fileResource.asKoObservable())
					self.filename(fileResource.Filename);
					event.target.value = null;

					self.form().Languages().forEach(function (language) {
						if (language.LanguageKey() == self.selectedLanguage()) {
							oldFileResourceId = language.FileResourceId();
							language.FileResourceId(fileResource.Id);
						}
					})

					var removeFileResourceFromDB = true;
					for (let i = window.database.stateManager.trackedEntities.length - 1; i >= 0; i--) {
						if (database.stateManager.trackedEntities[i].data.Id == oldFileResourceId && window.database.stateManager.trackedEntities[i].entitySet.collectionName == "Main_FileResource") {
							window.database.stateManager.trackedEntities.splice(i, 1);
							removeFileResourceFromDB = false;
						}
					}
					if (removeFileResourceFromDB == true && oldFileResourceId !== null) {
						window.database.Main_FileResource
							.find(oldFileResourceId)
							.then(function (fileResource) {
								self.fileResourcesToRemove().push(fileResource)
								self.loading(false);
							})
					} else {
						self.loading(false);

					}
				})

				.fail(function (error) {
					window.Log.error("Processing file failed: " + error);
					viewModel.loading(false)

				});
			return true;
		}

		self.showPdf = function (formPdfBytes) {
			pdfjsLib.GlobalWorkerOptions.workerSrc = window.Helper.Url.resolveUrl("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/build/pdf.worker.js");
			self.pageNum(1);
			var pdfData = atob(formPdfBytes)
			pdfjsLib.getDocument({ data: pdfData }).promise.then(function (pdfDoc_) {
				self.pdfDoc(pdfDoc_);
				self.renderPage(self.pageNum());
				self.pageCount(self.pdfDoc().numPages);
			});
		}

		self.renderPage = function (num) {
			self.pageRendering(true);
			var scale = 1.25,
				canvas = document.getElementById("the-canvas"),
				ctx = canvas.getContext('2d');

			// Using promise to fetch the page
			self.pdfDoc().getPage(num).then(function (page) {
				var viewport = page.getViewport({ scale: scale });
				canvas.height = viewport.height;
				canvas.width = viewport.width;

				// Render PDF page into canvas context
				var renderContext = {
					canvasContext: ctx,
					viewport: viewport
				};
				var renderTask = page.render(renderContext);

				// Wait for rendering to finish
				renderTask.promise.then(function () {
					self.pageRendering(false);
					if (self.pageNumPending() !== null) {
						// New page rendering is pending
						self.renderPage(self.pageNumPending());
						self.pageNumPending(null);
					}
				});
			});
			// Update page counters
			self.currentPage(num);
		}

		self.downloadPdf = function () {

			if (self.currentFileResource() == null) {
				alert(window.Helper.getTranslatedString("Warning_Download"));
			} else {
				var id = self.currentFileResource().Id();
				window.Log.info("Download PDF with ID: ", id)
				if (self.currentFileResource().Content() !== null) {

					download(atob(self.currentFileResource().Content()), self.currentFileResource().Filename(), "application/pdf");
				} else {
					fetch(window.Helper.resolveUrl("~/File/File/" + id))
						.then(response => response.blob())
						.then(blob => window.Helper.String.blobToBase64(blob))
						.then(function (base64data) {
							download(atob(base64data), self.currentFileResource().Filename(), "application/pdf");
						})
				}


			}
		}

		self.savePdf = function () {
			var setLoading = false;
			if (!self.loading()) {
				self.loading(true);
				setLoading = true;
			}

			var validation = true;
			self.form().Languages().forEach(function (language) {
				if (language.FileResourceId() == null) {
					validation = false;
				}
			})

			if (!validation) {
				self.loading(false);
				// you have to upload a pdf for Each Language before you can save the Checklist
				alert(window.Helper.getTranslatedString("Warning_LanguageRequiresPdf"));
				return false;
			}

			window.database.saveChanges()
				.then(function () {
					return Promise.all(
						self.form().Languages().map((language) => {
							return $.ajax({
								type: "POST",
								url: window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicFormRest/SaveLanguageFileResource?format=json"),
								dataType: "json",
								data: language
							});
						}))
				})
				.then(function () {
					self.form().ModifyDate(new Date());
					return $.when(
						$.ajax({
							url: window.Helper.resolveUrl("~/Crm.DynamicForms/SaveForm.json"),
							type: "post",
							contentType: "application/json",
							data: JSON.stringify(window.ko.toJS(self.form)),
							success: function (formResult) {
								self.form().Id(formResult.Id);
							}
						}))
				})
				.then(function () {
					self.fileResourcesToRemove().forEach(function (fileResource) {
						window.database.remove(fileResource)
					})
					self.fileResourcesToRemove.removeAll();
					return window.database.saveChanges();
				})
				.then(function () {
					if (setLoading) {
						self.loading(false);
					}

				})
				.fail(function (e) {
					window.Log.error(e);
					window.Helper.Message.Error(e.toString());
				})

		}


		self.languageChanged = function () {

			var isModified = false;
			window.database.stateManager.trackedEntities.forEach(trackedEntitie => {
				self.form().Languages().forEach(function (language) {

					if (language.FileResourceId() == trackedEntitie.data.Id && language.LanguageKey() == self.selectedLanguage()) {
						self.showPdf(trackedEntitie.data.Content);
						self.filename(trackedEntitie.data.Filename)
						self.currentFileResource(trackedEntitie.data.asKoObservable())
						isModified = true;
					}
				})
			})

			if (isModified == false) {
				self.showOldPdf()
			}

		};


		self.showOldPdf = function () {
			self.form().Languages().forEach(function (language) {
				if (language.LanguageKey() == self.selectedLanguage()) {
					fileResourceId = language.FileResourceId();
				}
			})

			if (fileResourceId == null) {
				var canvas = document.getElementById("the-canvas")
				var ctx = canvas.getContext("2d");
				ctx.clearRect(0, 0, canvas.width, canvas.height);
				self.pageCount(0);
			}

			window.database.Main_FileResource
				.find(fileResourceId)
				.then(function (fileResource) {
					self.filename(fileResource.Filename)
					self.currentFileResource(fileResource.asKoObservable())
					fetch(window.Helper.resolveUrl("~/File/File/" + fileResource.Id))
						.then(response => response.blob())
						.then(blob => window.Helper.String.blobToBase64(blob))
						.then(function (base64data) {
							self.showPdf(base64data)

						})
				})

		}

		/* Pagination for PDFs larger than one page */
		self.queueRenderPage = function (num) {
			var viewModel = this;
			if (viewModel.pageRendering()) {
				viewModel.pageNumPending(num);
			} else {
				viewModel.renderPage(num);
			}
		};

		self.onNextPage = function () {
			var viewModel = this;
			if (viewModel.pageNum() >= viewModel.pdfDoc().numPages) {
				return;
			}
			viewModel.pageNum(viewModel.pageNum() + 1);
			viewModel.queueRenderPage(viewModel.pageNum());
		};

		self.onPrevPage = function () {
			var viewModel = this;
			if (viewModel.pageNum() <= 1) {
				return;
			}
			viewModel.pageNum(viewModel.pageNum() - 1);
			viewModel.queueRenderPage(viewModel.pageNum());
		};

		self.hasNextPage = function () {
			var viewModel = this;
			return viewModel.pageNum() !== viewModel.pageCount();
		};


		self.hasPrevPage = function () {
			var viewModel = this;
			return viewModel.pageNum() !== 1;
		};

		return self;


	};
	window.FormDesignerViewModel.prototype = baseFormDesignerViewModel.prototype;
})(window.jQuery, window.ko);
