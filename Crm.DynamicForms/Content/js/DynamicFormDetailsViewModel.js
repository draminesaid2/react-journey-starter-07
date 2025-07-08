$(function() {
	window.ko.validation.rules["minUploadCount"] = {
		validator: function(response, element) {
			return element.Files().length >= element.MinUploadCount();
		},
		message: ''
	};
	window.ko.validation.registerExtenders();
});
namespace("Crm.DynamicForms.ViewModels").DynamicFormDetailsViewModel = function () {
	var self = this;
	self.loading = window.ko.observable(true);
	self.loadingFiles = window.ko.observable(false);
	self.loadingText = window.ko.observable("");
	self.elements = window.ko.observableArray([]);
	self.invalidElements = window.ko.observableArray([]);
	self.formReference = window.ko.observable().config({ restSave: window.Helper.resolveUrl("~/Crm.DynamicForms/SaveFormReference.json") });
	self.languages = window.ko.observableArray([]);
	self.categories = window.ko.observable();
	self.localizations = window.ko.observableArray([]);
	self.page = window.ko.observable(1);
	self.pageElements = window.ko.observableArray([]);
	self.selectedLanguage = window.ko.observable(null);
	self.IsPdfViewModel = window.ko.observable(false);

	self.footerHeight = window.ko.observable(null);
	self.headerHeight = window.ko.observable(null);
	self.site = self.Site = window.ko.observable(null);

	self.showSubPage = window.ko.observable(false);
	self.mainPageVisible = window.ko.observable(true).extend({ rateLimit: 500 });
	self.subPageVisible = window.ko.observable(false).extend({ rateLimit: 500 });
	self.fadeInSubPage = ko.observable(false).extend({ rateLimit: 500 });
	self.fadeOutSubPage = ko.observable(false);
	self.fadeInMainPage = ko.observable(false).extend({ rateLimit: 500 });
	self.fadeOutMainPage = ko.observable(false);
	self.showSubPage.subscribe(function (value) {
		self.fadeOutSubPage(!value);
		self.fadeInMainPage(!value);
		self.fadeOutMainPage(value);
		self.mainPageVisible(!value);
		self.fadeInSubPage(value);
		self.subPageVisible(value);
	});
	self.toggleSubPage = function() {
		self.showSubPage(!self.showSubPage());
	};
	self.subPageDynamicFormElement = window.ko.observable(null);
	self.subPageTitle = window.ko.observable(null);

	self.validationRulesAdded = window.ko.observable(false);
	self.requiredValidationRulesAdded = window.ko.observable(false);
	self.DynamicForm = window.ko.observable(null);
	self.DynamicFormElementRules = {}
	self.ResponsesOutput = window.ko.observable({});
	self.errors = window.ko.validation.group(self);
	self.dataProtectionInfoVisibility = window.ko.observable(false);

	$(document).on("change", "input[type=file].fileAttachment", function (e) {
		if (window.client.isBackend()) {
			return false;
		}
		if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
			var fileApiAlertString = window.Helper.String.getTranslatedString("M_FileApiNotSupported");
			alert(fileApiAlertString);
			return false;
		}
		if (e.target.files.length) {
			var files = Object.keys(e.target.files)
				.sort()
				.map(function (key) { return e.target.files[key]; });
			self.readFiles(files, this);
			Helper.DOM.resetFileInput(this);
		}
		return false;
	});

	function elementGroupsAreEqual(elementGroup1, elementGroup2) {
		if (elementGroup1.length !== elementGroup2.length) {
			return false;
		}
		for (var i = 0; i < elementGroup1.length; i++) {
			if (window.ko.unwrap(elementGroup1[i].Id) !== window.ko.unwrap(elementGroup2[i].Id)) {
				return false;
			}
		}
		return true;
	}

	function groupElements(elements) {
		var groupedElements = window.ko.pureComputed(function() {
			var sections = [];
			var elementGroups = window.ko.observableArray([]);
			var currentGroup = window.ko.observableArray([]);
			var currentSection = null;
			var currentSize = 0;
			if (window.ko.unwrap(elements)) {
				for (var i = 0; i < window.ko.unwrap(elements).length; i++) {
					var element = window.ko.unwrap(elements)[i];
					if (element.FormElementType() == 'PageSeparator') {
						continue;
					}
					//if (!element.isVisible || element.isVisible() === false) {
					//	continue;
					//}
					if (element.Section() !== currentSection) {
						if (currentGroup().length > 0) {
							elementGroups.push(currentGroup);
							currentGroup = window.ko.observableArray([]);
							currentSize = 0;
						}
						if (elementGroups().length > 0) {
							sections.push(elementGroups);
							elementGroups = window.ko.observableArray([]);
						}
						currentSection = element.Section();
					}
					if ((12 / window.ko.unwrap(element.Size)) + currentSize > 12) {
						elementGroups.push(currentGroup);
						currentGroup = window.ko.observableArray([]);
						currentSize = 0;
					}
					currentGroup.push(element);
					currentSize += (12 / window.ko.unwrap(element.Size));
				}
			}
			if (currentGroup().length > 0) {
				elementGroups.push(currentGroup);
			}
			if (elementGroups().length > 0) {
				sections.push(elementGroups);
			}
			return sections;
		}).extend({ rateLimit: { method: "notifyWhenChangesStop" } });
		var result = window.ko.observableArray([]);
		groupedElements.subscribe(function (sections) {
			if (result().length === 0) {
				result(sections);
			} else {
				for (var sectionI = 0; sectionI < sections.length; sectionI++) {
					if (result().length <= sectionI) {
						result.push(sections[sectionI]);
					} else {
						for (var elementGroupI = 0; elementGroupI < sections[sectionI]().length; elementGroupI++) {
							if (result()[sectionI]().length <= elementGroupI) {
								result()[sectionI].push(sections[sectionI]()[elementGroupI]);
							} else if (!elementGroupsAreEqual(result()[sectionI]()[elementGroupI](), sections[sectionI]()[elementGroupI]())) {
								result()[sectionI]()[elementGroupI](sections[sectionI]()[elementGroupI]());
							}
						}
						var elementGroupDeleteCount = Math.max(0, result()[sectionI]().length - sections[sectionI]().length);
						if (elementGroupDeleteCount > 0) {
							result()[sectionI].splice(sections[sectionI]().length, elementGroupDeleteCount);
						}
					}
				}
				var sectionDeleteCount = Math.max(0, result().length - sections.length);
				if (sectionDeleteCount > 0) {
					result.splice(sections.length, sectionDeleteCount);
				}
			}
		});
		return result;
	}
	self.groupedElements = groupElements(self.elements);
	self.groupedPageElements = groupElements(self.pageElements);
	self.removeDescription = window.ko.pureComputed(function() {
		return window.Helper.String.getTranslatedString("ReallyDelete?").replace("{0}", self.getLocalizationText(null));
	});
	self.hasPendingChanges = window.ko.pureComputed(function () {
		if (!self.formReference()) {
			return false;
		}
		window.ko.unwrap(self.formReference().ModifyDate);
		var hasFormChanges = self.formReference().innerInstance.entityState === window.$data.EntityState.Added || (self.formReference().innerInstance.entityState === window.$data.EntityState.Modified && self.formReference().innerInstance.changedProperties.some(x => x.name === "ModifyDate"));
		if (hasFormChanges) {
			return hasFormChanges;
		}
		var hasFileChanges = self.elements().some(e => e.FormElementType() === "FileAttachmentDynamicFormElement" && (e.FilesRemoved().length || e.Files().some(f => f.innerInstance.changedProperties)));
		return hasFileChanges;
	});
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype = new Crm.ViewModels.DefaultViewModel();
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.onFileAttachmentChanged = function(e) {
	var self = this;
	if (window.client.isBackend()) {
		return false;
	}
	if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
		var fileApiAlertString = window.Helper.String.getTranslatedString("M_FileApiNotSupported");
		alert(fileApiAlertString);
		return false;
	}
	var input = e.target;
	if (input.files.length) {
		var files = Object.keys(input.files)
			.sort()
			.map(function(key) { return input.files[key]; });
		input.setAttribute("disabled", true);
		self.readFiles(files, input);
		var $input = $(input);
		$input.hide();
		var clone = $input.clone();
		clone.attr("disabled", false);
		clone.val("");
		clone.show();
		clone.insertAfter($input);
	}
	return false;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.findResponseObject = function (formElement) {
	var self = this;
	return window.ko.utils.arrayFirst(window.ko.unwrap(self.formReference().Responses), function (response) {
		return window.ko.unwrap(response.DynamicFormElementKey) === window.ko.unwrap(formElement.Id);
		}) || null;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.updateResponseValue = function (formElement, newValue) {
	var self = this;
	var response = self.findResponseObject(formElement) || {};
	var elementType = formElement.FormElementType();
	if (elementType === "Number") {
		var numberFormElement = document.querySelector("#formelement-" + formElement.Id() + " input[type=number]");
		if (numberFormElement) {
			numberFormElement.value = newValue;
		}
	}
	if (["FileAttachmentDynamicFormElement", "SignaturePadWithPrivacyPolicy", "CheckBoxList"].indexOf(elementType) >= 0) {
		newValue = JSON.stringify(newValue);
	} else if ("RadioButtonList" === elementType) {
		newValue = newValue === null || newValue === "" ? null : newValue.toString();
	}
	if (elementType === "FileAttachmentDynamicFormElement" && window.ko.unwrap(response.Value) === newValue) {
		return;
	}
	if (ko.isObservable(response.Value)) {
		response.Value(newValue);
	} else {
		response.Value = newValue;
	}
	if (!response.DynamicFormReferenceKey) {

		var value = response.Value;
		response = window.database.CrmDynamicForms_DynamicFormResponse.CrmDynamicForms_DynamicFormResponse.create();
		response.DynamicFormReferenceKey = self.formReference().Id();
		response.DynamicFormElementKey = formElement.Id();
		response.DynamicFormElementType = elementType;
		response.DynamicFormKey = self.DynamicForm().Id();
		response.Value = value;
		window.database.add(response);
		self.formReference().Responses.push(response.asKoObservable());
	}
	self.formReference().ModifyDate(new Date());
	self.formReference().ModifyUser(Helper.User.getCurrentUserName());
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.initFormElement = function(element) {
	var self = this;
	if (element.DefaultResponseValue) {
		var response = self.findResponseObject(element) || {};
		var rawValue = ko.unwrap(response.Value);
		if (rawValue === undefined) {
			rawValue = null;
		}
		var defaultValue = JSON.parse(ko.unwrap(element.DefaultResponseValue));
		var value;
		var elementType = element.FormElementType();
		if (elementType === "FileAttachmentDynamicFormElement") {
			element.Files = ko.observableArray();
			element.FilesRemoved = ko.observableArray();
			value = rawValue !== null ? JSON.parse(rawValue) : [];
			if (!!window.Main && !!window.Main.Settings && (!element.MaxFileSize() || element.MaxFileSize() > window.Main.Settings.MaxFileLengthInKb * 1000))
				element.MaxFileSize(window.Main.Settings.MaxFileLengthInKb * 1000);
		} else if (elementType === "SignaturePadWithPrivacyPolicy") {
			value = JSON.parse(rawValue);
			value = ko.wrap.fromJS(value || defaultValue);
		} else if (elementType === "CheckBoxList") {
			value = rawValue !== null ? JSON.parse(rawValue) : [];
		} else if (elementType === "Date") {
			value = rawValue ? new Date(rawValue) : null;
		} else if (elementType === "RadioButtonList") {
			value = rawValue ? parseInt(rawValue) : null;
		} else {
			value = rawValue;
		}
		element.Response = ko.observable(value !== undefined ? value : defaultValue);
	}
	if (!!window.Crm.Offline) {
		var rules = self.DynamicFormElementRules[ko.unwrap(element.Id)] || [];
		if (!ko.isObservable(element.Rules)) {
			element.Rules = ko.observableArray([]);
		}
		element.Rules(rules);
	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.setupResponseSubscriptions = function(element) {
	var self = this;
	var queue = [];
	if (ko.isObservable(element.Response)) {
		queue.push(element.Response);
		var response = ko.unwrap(element.Response);
		if (response && typeof response === "object") {
			Object.keys(response).forEach(function(key) {
				if (ko.isObservable(response[key])) {
					queue.push(response[key]);
				}
			});
		}
	}
	queue.forEach(function(x) {
		x.subscribe(function() {
			self.updateResponseValue(element, ko.wrap.toJS(element.Response()));
		});
	});
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.initFormElements = function() {
	var self = this;
	self.elements(window.ko.utils.arrayMap(window.ko.utils.arrayFilter(window.ko.unwrap(self.DynamicForm().Elements), function(element) {
		return !!window.ko.unwrap(element.IsActive);
	}), function(element) {
		if (window.ko.isObservable(element.Id)) {
			return element;
		}
		return window.ko.wrap.fromJS(element);
	}).sort(function(a, b) { return a.SortOrder() - b.SortOrder(); }));
	var currentPage = 1;
		var currentSection = null;
		window.ko.utils.arrayForEach(self.elements(),
			function(element) {
				if (element.FormElementType() == 'PageSeparator') {
					currentPage++;
					currentSection = null;
				}
				if (element.FormElementType() == 'SectionSeparator') {
					currentSection = element;
				}
				element.Page = window.ko.observable(currentPage);
				element.Section = window.ko.observable(currentSection);
				self.initFormElement(element);
				self.setupResponseSubscriptions(element);
			});
		window.ko.utils.arrayForEach(self.elements(), function(element) {
			element.isVisible = window.ko.pureComputed(function() {
				if (element.FormElementType() === "PageSeparator") {
					return false;
				}
				var matchesAnyRule = false;
				element.Rules().forEach(function(rule) {
					if (matchesAnyRule) {
						return;
					}
					var matchType = rule.MatchType();
					var matchesRule = matchType === "All";
					rule.Conditions().forEach(function(condition) {
						var conditionElement = window.ko.utils.arrayFirst(self.elements(), function(x) {
							return x.Id() === condition.DynamicFormElementId();
						});
						var matchesCondition = self.matchesCondition(conditionElement, condition);
						if (matchType === "All") {
							matchesRule = matchesRule && matchesCondition;
						} else if (matchType === "Any") {
							matchesRule = matchesRule || matchesCondition;
						} else {
							throw "Unknown Rule MatchType " + matchType;
						}
					});
					matchesAnyRule = matchesAnyRule || matchesRule;
				});
				var ruleType = element.Rules().length === 0 ? null : element.Rules()[0].Type();
				var isVisible = ruleType === null || (ruleType === "Hide" && !matchesAnyRule) || (ruleType === "Show" && matchesAnyRule);
				if (element.Section() && element !== element.Section()) {
					isVisible = isVisible && element.Section().isVisible();
				}
				return isVisible;
			});
			element.isVisible.subscribe(function(isVisible) {
				if (isVisible === false) {
					var defaultValue = self.getDefaultValue(element.FormElementType());
					if (element.Response && element.Response() !== defaultValue) {
						element.Response(defaultValue);
					}
				}
			});
	});
	self.elements.distinct('Page');
	self.elements.distinct('Section');
		
		self.visibleElements = window.ko.pureComputed(function() {
			return self.elements().filter(function(x) {
				return x.isVisible();
			});
		}).distinct("Page");
		self.pages = window.ko.pureComputed(function() {
			return self.visibleElements.indexKeys.Page().length;
		});
		if (window.ko.custom && window.ko.custom.paging) {
			self.pager = window.ko.custom.paging(null, null, self.page, self.pages);
			self.pager.totalItemCount(self.visibleElements().length);
			self.visibleElements.subscribe(function(values) {
				self.pager.totalItemCount(values ? values.length : 0);
			});
		}
		self.visibleElements.subscribe(function(values) {
			var currentVisiblePage = self.pageElements().length > 0 ? self.visibleElements.indexKeys.Page().indexOf(self.pageElements()[0].Page().toString()) + 1 : 1;
			if (self.page() !== currentVisiblePage) {
				self.page(currentVisiblePage);
			}
		});
	self.addValidationRules();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.loadFileResources = function() {
	var self = this;
	var loadFileResourcesDeferred = new $.Deferred();
	if (!window.Crm.Offline) {
		return loadFileResourcesDeferred.resolve().promise();
	}
	var fileAttachmentDynamicFormElements = self.elements()
		.filter(function(e) { return e.IsActive() && e.FormElementType() === "FileAttachmentDynamicFormElement"; });
	var imageElements = self.elements()
		.filter(function(e) { return e.IsActive() && e.FormElementType() === "Image"; });
	if (!fileAttachmentDynamicFormElements.length && !imageElements.length) {
		return loadFileResourcesDeferred.resolve().promise();
	}
	var fileResourceIds = fileAttachmentDynamicFormElements
		.map(function(element) { return window.ko.toJS(element.Response); })
		.reduce(function(a, b) { return a.concat(b); }, [])
		.filter(function(id) { return id; })
		.concat(imageElements.map(function (element) { return element.FileResourceId(); }));
	if (fileResourceIds.length === 0) {
		return loadFileResourcesDeferred.resolve().promise();
	}

	var maxIdCount = 30;
	var promises = [];
	var fileResources = [];
	if (window.Helper.Offline && window.Helper.Offline.status === "offline" || fileResourceIds.length < maxIdCount) {
		promises.push(getFiles(fileResourceIds));
	} else {
		var start = 0;
		while (start < fileResourceIds.length) {
			var ids = fileResourceIds.slice(start, start + maxIdCount);
			promises.push(getFiles(ids));
			start += maxIdCount;
		}
	}

	function getFiles(ids) {
		return window.Crm.Offline.Database.Main_FileResource
			.filter(function (fileResource) {
				return fileResource.Id in this.ids;
			}, { ids: ids })
			.toArray().then(function (files) {
				files.forEach(function (it) {
					fileResources.push(it);
				});
			});
	}

	return $.when.apply($, promises).then(function () {
		var fileResourceMap = fileResources.reduce(function (map, file) {
			var fileObservable = window.Crm.Offline.Database.Main_FileResource.Main_FileResource.create(file).asKoObservable();
			map[file.Id] = fileObservable;
			return map;
		}, {});
		fileAttachmentDynamicFormElements.forEach(function (element) {
			window.ko.toJS(element.Response).forEach(function (id) {
				var fileResource = fileResourceMap[id];
				if (fileResource) {
					element.Files.push(fileResource);
				}
			});
			element.Files.sort(function (a, b) { return a.CreateDate() > b.CreateDate() });
			if (!self.ResponsesOutput()[element.Id()]) {
				self.ResponsesOutput()[element.Id()] = element.Files;
			}
		});
		imageElements.forEach(function (element) {
			var fileResource = fileResourceMap[element.FileResourceId()];
			if (fileResource) {
				element.FileResource = fileResource;
			}
			if (!self.ResponsesOutput()[element.Id()]) {
				self.ResponsesOutput()[element.Id()] = element.FileResource;
			}

		});
	}).then(loadFileResourcesDeferred.resolve);
	return loadFileResourcesDeferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.init = function (id, routeValues) {
	var self = this;
	if (typeof id === "object" && !routeValues) {
		routeValues = id;
	}

	var deferred = new $.Deferred();
	self.validationRulesAdded(false);
	self.requiredValidationRulesAdded(false);
	if (!!routeValues.formReference || !!routeValues.DynamicFormReference) {
		var routeValue = window.ko.utils.unwrapObservable(routeValues.formReference || routeValues.DynamicFormReference);
		if (!!routeValue && !!routeValue.innerInstance && typeof routeValue.innerInstance.getType === "function") {
			window.database.attachOrGet(routeValue.innerInstance);
			self.formReference(routeValue);
			window.ko.utils.arrayForEach(self.formReference().Responses(), function (response) {
				window.database.attachOrGet(response.innerInstance);
			});
		} else if (!!routeValue) {
			self.formReference(window.ko.wrap.fromJS(routeValue));
		}
	}
	Object.getOwnPropertyNames(routeValues).forEach(function (routeValue) {
		if (window.ko.isWriteableObservable(self[routeValue]) && self[routeValue]() == null) {
			self[routeValue](window.ko.unwrap(routeValues[routeValue]));
		}
	});
	if (!self.DynamicForm() && !!routeValues.formReference) {
		self.DynamicForm(window.ko.unwrap(routeValues.formReference.DynamicForm));
	}
	if (window.ko.custom.initEditor) {
		window.ko.custom.initEditor(self, routeValues);
	}
	if (!!routeValues.Elements) {
		self.DynamicForm().Elements(routeValues.Elements);
	}
	if (!!routeValues.ElementRules) {
		self.DynamicForm().Elements().forEach(element => {
			element.Rules = routeValues.ElementRules.filter(rule => rule.DynamicFormElementId === element.Id);
		})
	}
	if (!!routeValues.ElementRuleConditions) {
		self.DynamicForm().Elements().forEach(element => {
			element.Rules.forEach(rule => {
				rule.Conditions = routeValues.ElementRuleConditions.filter(condition => condition.DynamicFormElementRuleId === rule.Id); 
			});
		})
	}
	if (!!routeValues.Localizations) {
		self.localizations(routeValues.Localizations);
	}
	if (!!routeValues.Responses) {
		self.formReference().Responses = window.ko.wrap.fromJS(routeValues.Responses);
	}
	//if (!!window.Crm.Offline && !self.formReference()) {
	if (!!window.Crm.Offline) {
		var loadFromDb = new $.Deferred().resolve().promise();
		if (!self.DynamicForm()) {
			loadFromDb = window.Helper.Database.initialize()
				.then(function () {
					window.Crm.Offline.Database.CrmDynamicForms_DynamicForm
						.includeElements()
						.include("Languages")
						.find(self.formReference().DynamicFormKey())
						.then(function (result) {
							self.DynamicForm(result.asKoObservable());
							self.formReference.valueHasMutated();
						});
				});
		}
		window.Helper.Database.initialize()
			.then(function () {
				return window.Crm.Offline.Bootstrapper.initializeSettings();
			}).then(loadFromDb).then(function () {
				return window.database.CrmDynamicForms_DynamicFormElementRule
					.include("Conditions")
					.filter("it.DynamicFormId === this.dynamicFormId", {dynamicFormId: self.formReference().DynamicFormKey()})
					.toArray();
			}).then(function (rules) {
				self.DynamicFormElementRules = rules.reduce(function (map, rule) {
					var rules = map[rule.DynamicFormElementId];
					if (!rules) {
						rules = [];
						map[rule.DynamicFormElementId] = rules;
					}
					rules.push(rule.asKoObservable());
					return map;
				}, {});
				return window.Helper.Culture.languageCulture();
			}).then(function (language) {
				self.selectedLanguage(language);
				return window.Helper.Lookup.getLocalizedArrayMap("Main_Language", language).then(function (lookup) {
					self.languages(lookup);
					return language;
				});
			}).then(function (language) {
				return window.Helper.Lookup.getLocalizedArrayMap("CrmDynamicForms_DynamicFormCategory", language).then(function (lookup) {
					self.categories(lookup);
					return language;
				});
			}).then(function (language) {
				return self.loadLocalizations(self.formReference().DynamicFormKey(), language);
			}).then(function () {
				return window.database.Main_Site.GetCurrentSite().first();
			}).then(function (site) {
				self.site(site);
				if (window.Main &&
					window.Main.Settings &&
					window.Main.Settings.Report) {
					var headerHeight = +window.Main.Settings.Report.HeaderHeight +
						+window.Main.Settings.Report.HeaderSpacing;
					self.headerHeight(headerHeight);
					var footerHeight = +window.Main.Settings.Report.FooterHeight +
						+window.Main.Settings.Report.FooterSpacing;
					self.footerHeight(footerHeight);
				}
				self.initFormElements();
				self.loadFileResources().then(deferred.resolve);
			}).fail(function (error) {
				window.Log.error('Dynamic Form ' + self.formReference().DynamicFormKey() + ' not found: ' + error);
				deferred.resolve();
			});
	} else {
		self.initFormElements();
		deferred.resolve();
	}
	deferred.then(function() {
		var initRunning = true;
		self.page.subscribe(function(page) {
			if (!!self.elements && !!self.elements.index && !!self.elements.index.Page) {
				self.loading(true);
				try {
					var pageNumber = self.visibleElements.indexKeys.Page()[page - 1];
					var elements = self.elements.index.Page()[pageNumber];
					self.pageElements(elements);
				} catch (e) {
					window.Log.error("Error occured in DynamicFormDetailsViewModel: " + e.message);
					self.loading(false);
					window.alert(window.Helper.getTranslatedString("BindingError"));
				}
				if (initRunning === false) {
					self.loading(false);
				}
				$(window).scrollTop(0);
			} else {
				self.pageElements([]);
			}
		});
		self.page.valueHasMutated();
		initRunning = false;
		self.selectedLanguage.subscribe(function(x) {
			self.loading(true);
			self.loadLocalizations(self.formReference().DynamicFormKey(), x).then(function() {
				self.loading(false);
			});
		});
	});
	return deferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.addValidationRules = function () {
	var self = this;
	if (self.validationRulesAdded() === true) {
		return;
	}
	window.ko.utils.arrayForEach(self.elements(), function (element) {
		if (!window.ko.isObservable(element.Response)) {
			element.Response = window.ko.observable(element.Response);
		}
		if (element.FormElementType() === "CheckBoxList" && !!element.MaxChoices()) {
			element.Response.extend({
				maxArrayLength: {
					message: window.Helper.String.getTranslatedString("RuleViolation.MaxArrayLength"),
					params: element.MaxChoices(), 
					onlyIf: function () { return element.isVisible(); }
				}
			});
		}
		if (element.FormElementType() === "MultiLineText" && !!element.MaxLength()) {
			element.Response.extend({
				maxLength: {
					params: element.MaxLength(),
					onlyIf: function () { return element.isVisible(); }
				}
			});
		}
		if (element.FormElementType() === "Number") {
			element.Response.extend({
				number: {
					params: true,
					onlyIf: function () { return element.isVisible(); }
				}
		});
		}
		if (element.FormElementType() === "Number" && element.MinValue) {
			var minValue = parseFloat(element.MinValue());
			if (minValue) {
				element.Response.extend({
					min: {
						params: minValue,
						onlyIf: function () { return element.isVisible(); }
					}
				});
			}
		}
		if (element.FormElementType() === "Number" && element.MaxValue) {
			var maxValue = parseFloat(element.MaxValue());
			if (maxValue) {
				element.Response.extend({
					max: {
						params: maxValue,
						onlyIf: function() { return element.isVisible(); }
					}
				});
			}
		}
		if (element.FormElementType() === "SingleLineText" && !!element.MaxLength()) {
			element.Response.extend({
				maxLength: {
					params: element.MaxLength(),
					onlyIf: function () { return element.isVisible(); }
				}
		});
        } 
	});
	self.validationRulesAdded(true);
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.addRequiredValidationRules = function () {
	var self = this;
	if (self.requiredValidationRulesAdded() == true) {
		return;
	}
	window.ko.utils.arrayForEach(self.elements(), function (element) {
		if (!!element.Required && element.Required() == true && element.FormElementType() !== "SignaturePadWithPrivacyPolicy") {
			element.Response.extend({
				required: {
					message: window.Helper.String.getTranslatedString('ThisFieldIsRequired'),
					params: true,
					onlyIf: function() {
						return self.requiredValidationRulesAdded() && element.isVisible();
					}
				}
			});
		}
		if (element.FormElementType() == 'CheckBoxList' && (!!element.MinChoices() || !!element.MaxChoices())) {
			element.Response.extend({
				minArrayLength: {
					message: window.Helper.String.getTranslatedString("RuleViolation.MinArrayLength"),
					params: element.MinChoices(),
					onlyIf: function () { return element.isVisible(); }
				}
		});
		}
		if (element.FormElementType() == 'MultiLineText' && !!element.MinLength()) {
			element.Response.extend({
				minLength: {
					params: element.MinLength(),
					onlyIf: function () { return element.isVisible(); }
				}
		});
		}
		if (element.FormElementType() == 'SingleLineText' && !!element.MinLength()) {
			element.Response.extend({
				minLength: {
					params: element.MinLength(),
					onlyIf: function () { return element.isVisible(); }
				}
		});
		}
		if (element.FormElementType() === "FileAttachmentDynamicFormElement" && !!element.MinUploadCount()) {
			element.Response.extend({
				minUploadCount: {
					message: window.Helper.String.getTranslatedString("MinUploadCountNotReached"),
					params: element,
					onlyIf: function () { return element.isVisible() && self.requiredValidationRulesAdded() }
				}
			});
		}
		if (element.FormElementType() === "SignaturePadWithPrivacyPolicy") {
			element.Response().Signature.extend({
				required: {
					message: window.Helper.String.getTranslatedString('ThisFieldIsRequired'),
					params: true,
					onlyIf: function () {
						return self.requiredValidationRulesAdded() && element.isVisible() && !!element.Required && element.Required() == true;
					}
				}
			});
			element.Response().AcceptedPrivacyPolicy.extend({
				validation: {
					validator: function validator(val) {
						return val === true;
					},
					message: window.Helper.String.getTranslatedString("PleaseAcceptDataPrivacyPolicy"),
					onlyIf: function () {
						return element.isVisible() && window.Crm.DynamicForms.Settings.DynamicFormElement.SignaturePad.Show.PrivacyPolicy && !!element.Response().Signature();
					}
				}
			});
		} 
	});
	self.requiredValidationRulesAdded(true);
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.removeRequiredValidationRules = function () {
	var self = this;
	self.requiredValidationRulesAdded(false);
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.saveFileResources = function() {
	var self = this;
	var fileAttachmentElements = self.elements().filter(function(e) { return e.FormElementType() === "FileAttachmentDynamicFormElement" });
	var files = [];
	fileAttachmentElements.forEach(function(element) {
		element.Files().forEach(function(file) {
			if (file.$file) {
				files.push({ FileResource: file.innerInstance, File: file.$file, MaxImageWidth: element.MaxImageWidth(), MaxImageHeight: element.MaxImageHeight() });
			}
		});
	});
	var deferred = new $.Deferred();
	var i = 1;
	async.eachSeries(files, function(x, cb) {
		self.loadingText(Helper.String.getTranslatedString("Main_FileResource") + ": " + i + "/" + files.length);
		var promise;
		var file = x.File;
		var maxImageWidth = x.MaxImageWidth;
		var maxImageHeight = x.MaxImageHeight;
		if (self.isImage(file.type) && (maxImageWidth || maxImageHeight)) {
			promise = self.readImage(file, maxImageWidth, maxImageHeight);
		} else {
			promise = self.readFile(file);
		}
		promise.then(function(dataUrl, size) {
			var b64 = dataUrl.toString().split("base64,")[1];
			var fileResource = Helper.Database.createClone(x.FileResource);
			fileResource.Content = b64;
			fileResource.Length = size;
			window.database.Main_FileResource.add(fileResource);
			if ((self.formReference().IsCompletionChecklist && self.formReference().IsCompletionChecklist()) ||
					(self.formReference().IsCreationChecklist && self.formReference().IsCreationChecklist())) {
				self.formReference().Responses([]);
			}
			window.database.saveChanges().then(function() {
				i++;
				cb();
			});
		});
	}, deferred.resolve);
	return deferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.updateFileResources = function () {
	var fileAttachmentElements = this.elements().filter(function (e) { return e.FormElementType() === "FileAttachmentDynamicFormElement" });
	var removeFileIfPersisted = function (file) {
		if (file.Id() !== window.Helper.String.emptyGuid()) {
			var entity = file.innerInstance;
			window.database.Main_FileResource.remove(entity);
		}
	};
	var createGuidIfNotPersisted = function (file) {
		if (file.Id() === window.Helper.String.emptyGuid()) {
			var entity = file.innerInstance;
			entity.Id = window.$data.createGuid().toString().toLowerCase();
		}
	};
	var filesRemoved = fileAttachmentElements.reduce(function (array, element) { return array.concat(element.FilesRemoved()); }, []);
	var files = fileAttachmentElements.reduce(function(array, element) { return array.concat(element.Files()); }, []);
	filesRemoved.forEach(removeFileIfPersisted);
	files.forEach(createGuidIfNotPersisted);
	fileAttachmentElements.forEach(function (element) {
		var fileResourceIds = element.Files().map(function (file) { return file.Id(); });
		element.Response(fileResourceIds);
	});
	return new $.Deferred().resolve().promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.dispose = function() {
	var formReference = window.ko.unwrap(this.formReference);
	if (formReference && formReference.innerInstance) {
		formReference.innerInstance.resetChanges();
		if (formReference.innerInstance.Responses) {
			formReference.innerInstance.Responses.forEach(response => window.database.detach(response));
		}
		window.database.detach(formReference.innerInstance);
	}
	this.localizations([]);
	this.formReference(null);
	$(document).off("change", "input[type=file].fileAttachment");
	$(document).off(".FileAttachmentElement");
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.save = function () {
	var self = this;
	self.loading(true);

	return self.updateFileResources()
		.then(function () {
			if (!self.formReference().Completed() && self.requiredValidationRulesAdded()) {
				self.removeRequiredValidationRules();
			}
			if (self.errors().length > 0) {
				self.loading(false);
				self.showErrors();
				return new $.Deferred().reject();
			} else {
				var responses = self.formReference().Responses();
				self.formReference().Responses([]);
				return self.applyChanges().then(function() {
					responses.forEach(response => window.database.attachOrGet(response));
					self.formReference().Responses(responses);
				}).fail(function(e) {
					window.Log.error(e);
				});
			}
		})
		.then(function() {
			return self.saveFileResources();
		})
		.then(function() {
			if (window.$.mobile) {
				window.$.mobile.closeDialog(true);
			}
		});
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.showErrors = function () {
	var self = this;
	self.errors.showAllMessages();
	var firstInvalidRespone = window.ko.utils.arrayFirst(self.elements(),
		function (element) {
			return !!element.Response && !element.Response.isValid();
		});
	if (!!firstInvalidRespone && !!firstInvalidRespone.Page) {
		self.page(firstInvalidRespone.Page());
	}
	var offset = $('.field-validation-error').first().offset();
	if (!!offset) {
		$.mobile.silentScroll(offset.top);
	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.showInResponse = function (dynamicFormElement) {
	var self = this;
	return dynamicFormElement.FormElementType() === "Literal" ||
		dynamicFormElement.FormElementType() === "SectionSeparator" ||
		dynamicFormElement.FormElementType() === "PageSeparator" ||
		dynamicFormElement.FormElementType() === "Image" ||
		self.DynamicForm().HideEmptyOptional() === false ||
		(dynamicFormElement.Response() !== null && dynamicFormElement.Response() !== "" && window.ko.toJSON(dynamicFormElement.Response()) !== dynamicFormElement.DefaultResponseValue()) ||
		dynamicFormElement.Required();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.openCompleteDialog = function () {
	var self = this;
	self.loading(true);
	self.addValidationRules();

	self.addRequiredValidationRules();
	window.ko.utils.arrayForEach(self.elements(),
		function (element) {
			element.Response.valueHasMutated();
		});
	self.errors = window.ko.validation.group(self);
	self.loading(false);
	if (self.errors().length == 0) {
		var $confirmPopups = $("div[id=popup-confirm]");
		$confirmPopups.popup().popup("open");
	} else {
		self.showErrors();

	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.complete = function () {
	var self = this;
	self.addValidationRules();
	self.addRequiredValidationRules();
	self.formReference().Completed(true);
	if (self.errors().length > 0) {
		self.elements().forEach(element => {
			element.Response.isValid.subscribe(() => self.listInvalidElements())
		});
		setTimeout(function () {
			self.listInvalidElements();
			var parentModal = $(".modal").first();
			parentModal.scrollTop(10000);
		});
	}
	return self.save();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.listInvalidElements = function () {
	var self = this;
	let errorIndex = 0;
	let lastPage = null;
	self.invalidElements([]);
	self.elements().forEach(element => {
		if (!element.Response.isValid()) {
			errorIndex = !!lastPage && lastPage == element.Page() ? errorIndex + 1 : 0;
			self.invalidElements.push({ page: element.Page(), element: element, errorIndex: errorIndex });
			lastPage = element.Page();
		}
	});
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.cancel = function (triggerRefreshViewModel) {
	$.mobile.closeDialog(!!triggerRefreshViewModel);
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.formElementTemplateName = function (formElement) {
	return 'dynamicform-element-display-template-' + formElement.FormElementType();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.canRemove = function () {
	var self = this;
	return !!self.currentUser && self.currentUser().Id() === self.formReference().CreateUser();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.remove = function () {
	var self = this;
	var deferred = new $.Deferred();
	self.formReference().Responses().forEach(function (response) {
		window.database.remove(response);
	});
	self.formReference().Responses([]);
	window.database.remove(self.formReference());
	window.database.saveChanges().then(function () {
		$.mobile.closeDialog(true);
	});
	return deferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.loadLocalizations = function (formId, language) {
	var self = this;
	if (self.localizations().length > 0 && self.localizations()[0].Language === language) {
		return new $.Deferred().resolve().promise();
	}
	var defaultLanguage = window.ko.unwrap(self.DynamicForm().DefaultLanguageKey);
	return window.database.CrmDynamicForms_DynamicFormLanguage.filter(function(x) {
				return x.DynamicFormKey === this.dynamicFormId && x.LanguageKey === this.language && x.StatusKey === "Released";
			}, { dynamicFormId: formId, language: language })
		.count()
		.then(function(results) {
			return window.database.CrmDynamicForms_DynamicFormLocalization.filter(function (x) {
					return x.DynamicFormId === this.dynamicFormId && x.Language === this.language;
				}, { dynamicFormId: formId, language: results === 1 ? language : defaultLanguage })
				.toArray((localizations) => self.localizations(localizations));
		});

};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.readFiles = function (files, input) {
	var self = this;
	var element = window.ko.dataFor(input);
	if (element) {
		var errors = self.validateFiles(files, element, input);
		if (errors.length) {
			self.showErrorMessage(errors.join("\r\n"));
			return;
		}
		files.forEach(function (file) {
			var fileResource = self.createFileResource(file, null, file.size);
			fileResource.$file = file;
			self.addFileResource(fileResource, element);
		});
	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.validateFiles = function (files, element, input) {
	var self = this;
	var errors = [];
	if (element && element.MaxUploadCount()) {
		var diff = element.Files().length + files.length - element.MaxUploadCount();
		if (diff > 0) {
			var tooMany = files.splice(files.length - diff, diff)
				.map(function (file) { return file.name; });
			errors.push(window.Helper.String.getTranslatedString("MaxUploadCountExceeded") + "\r\n" + tooMany.join("; "));
		}
	}
	if (element && element.MaxFileSize()) {
		var tooBig = [];
		var hasNoContent = [];
		files.slice().forEach(function (file, i) {
			if (!self.isImage(file.type) && file.size > element.MaxFileSize()) {
				files.splice(i, 1);
				tooBig.push(file.name);
			} else if (!file.size) {
				files.splice(i, 1);
				hasNoContent.push(file.name);
			}
		});
		if (tooBig.length) {
			errors.push(window.Helper.String.getTranslatedString("MaxFileSizeExceeded") + "\r\n" + tooBig.join("; "));
		}
		if (hasNoContent.length) {
			errors.push(window.Helper.String.getTranslatedString("FileIsEmpty") + ":\r\n" + hasNoContent.join("; "));
		}
	}
	//check for size, type, whatever here
	return errors;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.isImage = function (contentType) {
	return contentType && contentType.split("/")[0] === "image";
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.filterFiles = function (files, onlyImages) {
	var self = this;
	return files.filter(function(file) {
		var result = self.isImage(file.ContentType());
		return onlyImages ? result : !result;
	});
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.readFile = function (file) {
	var deferred = new $.Deferred();
	var reader = new FileReader();
	reader.onload = function (event) {
		deferred.resolve(event.target.result, file.size);
	};
	reader.readAsDataURL(file);
	return deferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.readImage = function (file, maxImageWidth, maxImageHeight) {
	var self = this;
	var deferred = new $.Deferred();
	var reader = new FileReader();
	reader.onload = function (event) {
		var blob = new Blob([event.target.result]);
		window.URL = window.URL || window.webkitURL;
		var blobUrl = window.URL.createObjectURL(blob);

		var image = new Image();
		image.src = blobUrl;
		image.onload = function () {
			var resizedImage = window.Helper.Image.resizeImage(image, maxImageWidth, maxImageHeight, window.Helper.Image.CompressionQualityLevel.High);
			window.URL.revokeObjectURL(blobUrl);
			deferred.resolve(resizedImage, resizedImage.length * 3 / 4);
		}
	};
	reader.readAsArrayBuffer(file);
	return deferred.promise();
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.createFileResource = function (file, b64, size) {
	var currentFile = window.database.Main_FileResource.Main_FileResource.create().asKoObservable();
	var date = new Date();
	currentFile.CreateDate(date);
	currentFile.ModifyDate(date);
	currentFile.Filename(file.name);
	currentFile.ContentType(file.type);
	currentFile.Length(size);
	currentFile.Content(b64);
	var currentUser = $("#meta\\.CurrentUser").attr("content");
	currentFile.CreateUser(currentUser);
	currentFile.ModifyUser(currentUser);
	return currentFile;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.addFileResource = function (fileResource, element) {
	element.Files.push(fileResource);
	element.Files.sort(function (a, b) { return a.CreateDate() > b.CreateDate() });
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.removeFileResource = function (fileResource, element) {
	element.Files.remove(fileResource);
	element.FilesRemoved.push(fileResource);
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.removeAttachedFile = function (fileResource, event) {
	var self = this;
	if (confirm(window.Helper.String.getTranslatedString("ConfirmDeleteFile"))) {
		var element = window.ko.contextFor(event.target).$parent;
		self.removeFileResource(fileResource, element);
	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.showErrorMessage = function (parameter, message) {
	var self = this;
	if (parameter != null) {
		alert(message + ': ' + parameter);
	} else {
		alert(message);
	}
	return;
}
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.getFileSize = function (length) {
	var fileSizeText;
	if (length < 1024) {
		fileSizeText = Number(length / 1024).toFixed(1) + ' Byte';
	} else if (length < 1048576) {
		fileSizeText = Number(length / 1024).toFixed(1) + ' KB';
	} else {
		fileSizeText = Number(length / 1048576).toFixed(1) + ' MB';
	}
	return fileSizeText;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.getLocalizationText = function (dynamicFormElement, choiceIndex, hint) {
	var self = this;
	choiceIndex = isNaN(choiceIndex) ? null : choiceIndex;
	var dynamicFormElementId = dynamicFormElement ? window.ko.unwrap(dynamicFormElement.Id) : null;
	hint = hint === true;
	var localization = window.ko.utils.arrayFirst(self.localizations(), function(x) {
		return window.ko.unwrap(x.DynamicFormElementId) === dynamicFormElementId && window.ko.unwrap(x.ChoiceIndex) === choiceIndex;
	}) || null;
	if (localization === null && hint === false && dynamicFormElement && (isNaN(choiceIndex) || choiceIndex === null)) {
		return window.Helper.String.getTranslatedString(ko.unwrap(dynamicFormElement.FormElementType), ko.unwrap(dynamicFormElement.FormElementType));
	} 
	if (localization === null) {
		return "";
	}
	var result = ko.unwrap(hint ? localization.Hint : localization.Value);
	return result || "";
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.toogleDataProtectionInfo = function () {
	var self = this;
	self.dataProtectionInfoVisibility(!self.dataProtectionInfoVisibility());
	if (!!$('.dataPrivacyPolicy').listview) {
		$('.dataPrivacyPolicy').listview().listview('refresh');
	}
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.getDefaultValue = function (formElementType) {
	if (formElementType === "CheckBoxList" || formElementType === "FileAttachmentDynamicFormElement") {
		return [];
	}
	if (formElementType === "SignaturePadWithPrivacyPolicy") {
		return {
			Signature: null,
			AcceptedPrivacyPolicy: false
		};
	}
	return null;
};
Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.matchesCondition = function (formElement, condition) {
	var filter = condition.Filter();
	var value = condition.Value();
	var formElementType = formElement.FormElementType();
	var response = window.ko.unwrap(formElement.Response);
	if (formElementType === "Date") {
		if (filter === "Equals") {
			return response === value || window.moment(response).isSame(window.moment.utc(value));
		}
		if (filter === "Before") {
			return window.moment(response).isBefore(window.moment.utc(value));
		}
		if (filter === "After") {
			return window.moment(response).isAfter(window.moment.utc(value));
		}
		if (filter === "Empty") {
			return response instanceof Date === false || !window.moment(response).isValid();
		}
		if (filter === "NotEmpty") {
			return response instanceof Date === true && window.moment(response).isValid();
		}
	}
	if (formElementType === "Time") {
		var responseMinutes = response ? window.moment.duration(response).asMinutes() : null;
		if (filter === "Equals") {
			return response === value || responseMinutes === window.moment.duration(value).asMinutes();
		}
		if (filter === "Before") {
			return Number.isInteger(responseMinutes) && responseMinutes < window.moment.duration(value).asMinutes();
		}
		if (filter === "After") {
			return Number.isInteger(responseMinutes) && responseMinutes > window.moment.duration(value).asMinutes();
		}
		if (filter === "Empty") {
			return !Number.isInteger(responseMinutes);
		}
		if (filter === "NotEmpty") {
			return Number.isInteger(responseMinutes);
		}
	}
	if (formElementType === "CheckBoxList") {
		var valueArray = (value ? value.split(",") : []).map(function(x) { return parseInt(x); });
		var responseArray = (response || []).map(function (x) { return parseInt(x); });
		if (filter === "Equals") {
			return window._.difference(responseArray, valueArray).length === 0 && window._.difference(valueArray, responseArray).length === 0;
		}
		if (filter === "NotEquals") {
			return window._.difference(responseArray, valueArray).length > 0 || window._.difference(valueArray, responseArray).length > 0;
		}
		if (filter === "Contains") {
			return window._.intersection(responseArray, valueArray).length === valueArray.length;
		}
		if (filter === "DoesNotContain") {
			return window._.intersection(responseArray, valueArray).length === 0;
		}
		if (filter === "Empty") {
			return responseArray.length === 0;
		}
		if (filter === "NotEmpty") {
			return responseArray.length > 0;
		}
	}
	if (formElementType === "DropDown" || formElementType === "RadioButtonList") {
		if (filter === "Equals") {
			return parseInt(response) === parseInt(value);
		}
		if (filter === "NotEquals") {
			return parseInt(response) !== parseInt(value);
		}
		if (filter === "Empty") {
			return response === null || response === "";
		}
		if (filter === "NotEmpty") {
			return response !== null && response !== "";
		}
	}
	if (formElementType === "FileAttachmentDynamicFormElement") {
		var files = window.ko.unwrap(formElement.Files);
		if (filter === "FilesEqualTo") {
			return files.length === parseInt(value);
		}
		if (filter === "FilesMoreThan") {
			return files.length > parseInt(value);
		}
		if (filter === "FilesLessThan") {
			return files.length < parseInt(value);
		}
	}
	if (formElementType === "SignaturePad") {
		if (filter === "Signed") {
			return !!response;
		}
		if (filter === "NotSigned") {
			return !response;
		}
	}
	if (formElementType === "SignaturePadWithPrivacyPolicy") {
		if (filter === "Signed") {
			return !!window.ko.unwrap(response.Signature);
		}
		if (filter === "NotSigned") {
			return !window.ko.unwrap(response.Signature);
		}
	}
	if (formElementType === "Number") {
		if (filter === "Equals") {
			return parseFloat(response) === parseFloat(value) || (isNaN(parseFloat(response)) && isNaN(parseFloat(value)));
		}
		if (filter === "Greater") {
			return parseFloat(response) > parseFloat(value);
		}
		if (filter === "Less") {
			return parseFloat(response) < parseFloat(value);
		}
		if (filter === "Empty") {
			return isNaN(parseFloat(response));
		}
		if (filter === "NotEmpty") {
			return !isNaN(parseFloat(response));
		}
	}
	if (formElementType === "MultiLineText" || formElementType === "SingleLineText") {
		if (filter === "Equals") {
			return response === value;
		}
		if (filter === "NotEquals") {
			return response !== value;
		}
		if (filter === "Contains") {
			return !!response && response.indexOf(value) !== -1;
		}
		if (filter === "DoesNotContain") {
			return !response || response.indexOf(value) === -1;
		}
		if (filter === "BeginsWith") {
			return !!response && response.startsWith(value);
		}
		if (filter === "EndsWith") {
			return !!response && response.endsWith(value);
		}
		if (filter === "Empty") {
			return response === null || response === "";
		}
		if (filter === "NotEmpty") {
			return response !== null && response !== "";
		}
	}
	throw "Unknown combination of form element type and condition filter (" + formElementType + " / " + filter + ")";
};