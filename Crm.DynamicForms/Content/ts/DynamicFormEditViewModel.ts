///<reference path="../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace";
import { Breadcrumb } from "../../../../Content/ts/breadcrumbs";

export class DynamicFormEditViewModel extends window.Main.ViewModels.ViewModelBase {
    conditions = ko.observableArray<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicFormElementRuleCondition>([]);
    disableRules = ko.observable<boolean>(false);
    disableSizeSelection = ko.observable<boolean>(false);
    disableRowSizeSelection = ko.observable<boolean>(false);
    form = ko.observable<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicForm>(null);
    alreadySavedDynamicForm = ko.observable<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicForm>(null);
    formElements = ko.observableArray<Crm.DynamicForms.Rest.Model.ObservableDynamicFormElementRest>([]);
    formElementTypes = ko.observableArray<string>([]);
    groupedElements: KnockoutComputed<any>;
    languages = ko.observableArray<string>([]);
    localizationEditorViewModel = ko.observable(null);
    localizations = ko.observableArray<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicFormLocalization>([]);
    lookups: any = {};
    newLanguageKey = ko.observable<string>(null);
    ruleEditorViewModel = ko.observable(null);
    rules = ko.observableArray<Crm.DynamicForms.Rest.Model.ObservableCrmDynamicForms_DynamicFormElementRule>([]);
    selectedFormElement = ko.observable<Crm.DynamicForms.Rest.Model.ObservableDynamicFormElementRest>(null);
    selectedLanguage =ko.observable<string>(null);
    statuses = ko.observableArray<Crm.DynamicForms.Model.Lookups.ObservableCrmDynamicForms_DynamicFormStatus>([]);
    tabs = ko.observable({});
    hasFormElementOrderChange = ko.observable<boolean>(false);

    // pdf feauture
    pageNum = ko.observable<number>(1);
    pdfDoc = ko.observable(null);
    pageRendering = ko.observable<boolean>(false);
    pageNumPending = ko.observable(null);
    pageCount = ko.observable<number>(0);
    currentPage = ko.observable();
    fileResource = ko.observable<Crm.Rest.Model.Main_FileResource>(null);
    currentFileResource = ko.observable<Crm.Rest.Model.ObservableMain_FileResource>(null);
    fileResourcesToRemove = ko.observableArray<string>([]);
    fileResources = ko.observableArray<any>([]);


    constructor() {
        super();
    }

    addLanguage() {
        const languageKey = this.newLanguageKey();
        if (!languageKey) {
            return;
        }
        if (this.form().Languages().some((existingLanguage) => {
            return existingLanguage.LanguageKey() === languageKey;
        })) {
            window.swal(window.Helper.String.getTranslatedString("Warning"), window.Helper.String.getTranslatedString("DynamicFormLanguageAlreadyExists"), "warning");
            return;
        }
        let language = window.database.CrmDynamicForms_DynamicFormLanguage.defaultType.create();
        language.DynamicFormKey = this.form().Id();
        language.LanguageKey = languageKey;
        language.StatusKey = "Draft";
        window.database.add(language);
        const koLanguage = language.asKoObservable();
        this.form().Languages.push(koLanguage);
    }

    afterFormElementMove(arg) {
        const isNew = arg.sourceParent === undefined;
        if (isNew) {
            arg.item.Size.subscribe(() => {
                this.formElements.valueHasMutated();
            });
            window.database.add(arg.item.innerInstance);
            arg.item.Localizations(arg.item.Localizations() || [])
            this.initFormElement(arg.item);
        }
        const itemBefore = arg.targetParent[arg.targetIndex - 1];
        const itemAfter = arg.targetParent[arg.targetIndex + 1];
        const oldIndex = this.formElements().indexOf(arg.item);
        let newIndex;
        if (itemBefore) {
            const indexItemBefore = this.formElements().indexOf(itemBefore);
            newIndex = oldIndex < indexItemBefore && !isNew ? indexItemBefore : indexItemBefore + 1;
        } else if (itemAfter) {
            newIndex = this.formElements().indexOf(itemAfter);
        } else {
            newIndex = 0;
        }
        if (oldIndex !== -1) {
            this.formElements.splice(oldIndex, 1);
        }
        this.formElements.splice(newIndex, 0, arg.item);
        this.formElements().forEach((item: any) => {
            const formElementIndex = this.formElements.indexOf(item);
            if (formElementIndex !== item.SortOrder()) {
                item.SortOrder(formElementIndex);
                this.hasFormElementOrderChange(true);
            }
        });
    }

    attachToDatabase() {
        window.database.attachOrGet(this.form().innerInstance);
        this.form().Languages().forEach(function (language) {
            window.database.attachOrGet(language.innerInstance);
        });
        this.formElements().forEach(function (formElement) {
            window.database.attachOrGet(formElement.innerInstance);
        });
        this.localizations().forEach(function (localization) {
            window.database.attachOrGet(localization.innerInstance);
        });
        this.rules().forEach(function (rule) {
            window.database.attachOrGet(rule.innerInstance);
        });
    }

    cancel() {
        window.history.back();
    }

    editFormElementRules(formElement) {
        this.ruleEditorViewModel(new window.Crm.DynamicForms.ViewModels.DynamicFormElementRuleEditorViewModel(this, formElement));
        const $modal = $("#dynamic-form-element-rule-editor");
        $modal.modal();
        $modal.one("hidden.bs.modal", () => this.ruleEditorViewModel(null));
    }

    async editLocalizations(dynamicFormElement, choiceIndex, isMultiline, isHint = false) {
        choiceIndex = isNaN(choiceIndex) ? null : choiceIndex;
        const modalViewModel = new window.Crm.DynamicForms.ViewModels.DynamicFormLocalizationEditorViewModel(this.form(), dynamicFormElement, choiceIndex, isMultiline, isHint, this.languages(), this.localizations());
        await modalViewModel.init();
        modalViewModel.save = () => {
            modalViewModel.localizations().forEach((localizationToSave) => {
                const localization = this.getLocalization(dynamicFormElement, choiceIndex, ko.unwrap(localizationToSave.Language()));
                localization.Hint(ko.unwrap(localizationToSave.Hint));
                localization.Value(ko.unwrap(localizationToSave.Value));
            });
            $("#dynamic-form-localization-editor").modal("hide");
        };
        this.localizationEditorViewModel(modalViewModel);
        const $modal = $("#dynamic-form-localization-editor");
        $modal.modal();
        $modal.one("hidden.bs.modal", () => this.localizationEditorViewModel(null));
    }

    async exportForm() {
        await this.save();
        window.location.assign(window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicForm/Export/" + this.form().Id()));
    }

    getCondition(dynamicFormElementRuleCondition) {
        return window.Helper.DynamicFormDesigner.getCondition(this.form, this.rules, this.conditions, dynamicFormElementRuleCondition);
    }

    getLocalization(dynamicFormElement, choiceIndex, language) {
        language = language || this.selectedLanguage();
        choiceIndex = isNaN(choiceIndex) ? null : choiceIndex;
        return window.Helper.DynamicFormDesigner.getLocalization(this.form, dynamicFormElement, choiceIndex, language, this.localizations());
    }

    getLocalizationText(dynamicFormElement, choiceIndex?, hint?) {
        choiceIndex = isNaN(choiceIndex) ? null : choiceIndex;
        hint = hint === true;
        return window.Helper.DynamicFormDesigner.getLocalizationText(dynamicFormElement, choiceIndex, hint, this.localizations(), this.selectedLanguage());
    }

    getReferencingElements(dynamicFormElement) {
        return window.Helper.DynamicFormDesigner.getReferencingElements(dynamicFormElement, this.conditions(), this.rules(), this.formElements());
    }

    getRule(dynamicFormElement, dynamicFormElementRule) {
        return window.Helper.DynamicFormDesigner.getRule(this.form, dynamicFormElement, dynamicFormElementRule, this.rules);
    }

    hasRules(dynamicFormElement) {
        return window.Helper.DynamicFormDesigner.hasRules(dynamicFormElement, this.rules);
    }

    async getDynamicForm(id: string) {
        return window.database.CrmDynamicForms_DynamicForm
            .include("Elements")
            .include("Elements.Localizations")
            .include("Languages")
            .include2("Localizations.filter(function(it2) { return it2.DynamicFormElementId === null; })")
            .find(id);
    }

    async init(id): Promise<void> {
        const form = await this.getDynamicForm(id);
        const rules = await window.database.CrmDynamicForms_DynamicFormElementRule
            .include("Conditions")
            .filter("it.DynamicFormId === this.dynamicFormId", { dynamicFormId: id })
            .toArray();
        rules.forEach((rule) => {
            this.rules().push(rule.asKoObservable());
            rule.Conditions.forEach((condition) => {
                this.conditions().push(condition.asKoObservable());
            });
        });

        const formElementTypeTemplates = await fetch(window.Helper.Url.resolveUrl("~/Crm.DynamicForms/FormElementTypes.json")).then(r => r.json());
        // @ts-ignore
        this.formElementTypes(window.database.DynamicFormElementRest.elementType.inheritedTo.map(dynamicFormElement => {
            const dynamicFormElementTypeName = dynamicFormElement.name.split("_")[1];
            const template = formElementTypeTemplates.find(x => x.$type == "Crm.DynamicForms.Rest.Model." + dynamicFormElementTypeName + "Rest, Crm.DynamicForms");
            const koDynamicFormElement = dynamicFormElement.create(template).asKoObservable();
            koDynamicFormElement.clone = function () {
                const clone = dynamicFormElement.create(template).asKoObservable();
                clone.Id(window.$data.createGuid().toString().toLowerCase());
                return clone;
            };
            return koDynamicFormElement;
        }));

        const statuses = await window.Helper.Lookup.getLocalized("CrmDynamicForms_DynamicFormStatus");
        this.statuses(statuses.filter(x => x.Key !== null));

        await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);

        const languages = await window.Helper.Lookup.getLocalizedArrayMap("Main_Language");
        this.languages(languages);

        const koForm = form.asKoObservable();
        koForm.Elements().forEach(formElement => this.initFormElement(formElement));
        this.form(koForm);
        this.alreadySavedDynamicForm((await this.getDynamicForm(id)).asKoObservable());
        this.localizations(this.form().Localizations());
        this.formElements(this.form().Elements().sort(function (a, b) {
            return a.SortOrder() - b.SortOrder();
        }));
        this.groupedElements = ko.pureComputed(() => {
            return window.Helper.DynamicFormDesigner.groupFormElements(this.formElements());
        });
        this.selectedFormElement.subscribe(function (formElement) {
            let margin = 0;
            if (formElement) {
                const $formElement = $("#sortable-item-" + formElement.Id());
                margin = $formElement.offset().top - 217;
            }
            $("#card-form-designer-sidebar").css("margin-top", margin + "px");
            $("a[href='#tab-edit-field']").trigger("click");
        });

        const currentLanguage = await window.Helper.Culture.languageCulture();
        if (this.form().Languages().some(x => x.LanguageKey() === currentLanguage)) {
            this.selectedLanguage(currentLanguage);
        } else {
            this.selectedLanguage(this.form().DefaultLanguageKey());
        }


        // pdf feauture
        if (this.form().CategoryKey() == "PDF-Checklist") {
            const viewModel = this;
            let fileResourceId = null;

            this.form().Languages().forEach(async function (language) {
                if (language.FileResourceId() !== null) {
                    fileResourceId = language.FileResourceId();
                    const fileResource = await window.database.Main_FileResource.find(fileResourceId);
                    const data = {
                        language: language.LanguageKey(),
                        fileResource: fileResource.asKoObservable()
                    };
                    viewModel.fileResources.push(data);
                    if (data.language == viewModel.selectedLanguage()) {
                        viewModel.currentFileResource(fileResource.asKoObservable());
                    }
                }
            });
        }
        await this.setBreadcrumbs();
        this.attachToDatabase();

    }


    initFormElement(formElement: any) {

        if (formElement.DefaultResponseValue) {
            formElement.Response = ko.observable(formElement.DefaultResponseValue() !== undefined ? formElement.DefaultResponseValue() : null);
        }

        if (formElement.FormElementType() === "FileAttachmentDynamicFormElement") {
            formElement.Files = ko.observableArray([]);
            formElement.FilesRemoved = ko.observableArray([]);
        }

        if (formElement.MinChoices && formElement.MaxChoices) {
            formElement.MinChoices.subscribe(() => {
                if (formElement.MinChoices() > formElement.Choices()) {
                    formElement.MinChoices(formElement.Choices());
                }
                if (formElement.MinChoices() > formElement.MaxChoices()) {
                    formElement.MaxChoices(formElement.MinChoices());
                }
            });
            formElement.MaxChoices.subscribe(() => {
                if (formElement.MaxChoices() > formElement.Choices()) {
                    formElement.MaxChoices(formElement.Choices());
                }
                if (formElement.MaxChoices() < formElement.MinChoices()) {
                    formElement.MinChoices(formElement.MaxChoices());
                }
            });
        }

        if (formElement.MinLength && formElement.MaxLength) {
            formElement.MinLength.subscribe(() => {
                if (formElement.MinLength() > formElement.MaxLength() && formElement.MaxLength() !== 0) {
                    formElement.MaxLength(formElement.MinLength());
                }
            });
            formElement.MaxLength.subscribe(() => {
                if (formElement.MaxLength() < formElement.MinLength() && formElement.MaxLength() !== 0) {
                    formElement.MinLength(formElement.MaxLength());
                }
            });
        }

        if (formElement.MinValue && formElement.MaxValue) {
            formElement.MinValue.subscribe(() => {
                if (formElement.MinValue() > formElement.MaxValue() && formElement.MaxValue() !== 0) {
                    formElement.MaxValue(formElement.MinValue());
                }
            });
            formElement.MaxValue.subscribe(() => {
                if (formElement.MaxValue() < formElement.MinValue() && formElement.MaxValue() !== 0) {
                    formElement.MinValue(formElement.MaxValue());
                }
            });
        }

    }

    preview() {
        const url = window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicForm/DynamicFormResponsePreview/" + this.form().Id() + "?output=HTML");
        const width = 1200;
        const height = 900;
        const left = (screen.width / 2) - (width / 2);
        const top = (screen.height / 2) - (height / 2);
        return window.open(url, "", "scrollbars=1, width=" + width + ", height=" + height + ", top=" + top + ", left=" + left);
    }

    async removeFormElement(formElement) {
        this.loading(true);
        const usage = await fetch(window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicForm/CheckDynamicFormElementUsage?dynamicFormElementId=" + formElement.Id())).then(r => r.json());
        if (usage.hasResponse) {
            try {
                await window.Helper.Confirm.genericConfirm({
                    text: window.Helper.String.getTranslatedString("ConfirmDeleteDynamicFormElement"),
                    type: "warning"
                });
            } catch {
                this.loading(false);
                return;
            }
        }
        this.selectedFormElement(null);
        window.Helper.DynamicFormDesigner.removeFormElement(formElement, this.formElements, this.rules, this.conditions);
        this.loading(false);
    }

    async removeLanguage(language) {
        const viewModel = this;
        try {
            await window.Helper.Confirm.genericConfirm({
                text: window.Helper.String.getTranslatedString("ConfirmDelete"),
                type: "warning"
            });
        } catch {
            return false;
        }
        const languageKey = language.LanguageKey();
        this.form().Localizations().filter(x => x.Language() === languageKey).forEach((localization) => window.database.remove(localization.innerInstance));
        this.form().Elements().forEach((element) => {
            element.Localizations().filter(x => x.Language() === languageKey).forEach((localization) => window.database.remove(localization.innerInstance));
        });
        if(language.FileResourceId() !== null && language.FileResourceId() !== undefined) {
            const fileResourceToRemove = await window.database.Main_FileResource.find(language.FileResourceId());
            window.database.remove(fileResourceToRemove);
        }
        const removeFileResource = viewModel.fileResources().find(function (data) {
               return data.language == languageKey;
        });
        if (removeFileResource) {
            viewModel.fileResources.remove(removeFileResource);
        }
        window.database.remove(language.innerInstance);
        this.form().Languages.remove(language);
        return true;
    }

    async save() {
        const formElementErrors = ko.validation.group(this.formElements(), { deep: true });
        if (formElementErrors().length > 0) {
            const invalidFormElement = this.formElements().find(function (formElement) {
                return ko.validation.group(formElement)().length > 0;
            });
            this.selectedFormElement(invalidFormElement);
            formElementErrors.scrollToError();
            formElementErrors.showAllMessages();
            return;
        }
        const formErrors = ko.validation.group(this.form(), {deep: true});
        if (formErrors().length > 0) {
            $("a[href='#tab-edit-form']").trigger("click");
            formErrors.scrollToError();
            formErrors.showAllMessages();
            return;
        }
        let setLoading = false;
        if (!this.loading()) {
            this.loading(true);
            setLoading = true;
        }

        this.formElements().forEach((item) => {
            item.DynamicFormKey(this.form().Id());
            if (item.SortOrder() !== this.formElements.indexOf(item)) {
                this.hasFormElementOrderChange(true);
            }
            item.SortOrder(this.formElements.indexOf(item));
        });
        if (this.hasFormElementOrderChange()) {
            this.form().ModifyDate(new Date());
        }
        await window.database.saveChanges();
        this.attachToDatabase();
        this.alreadySavedDynamicForm((await this.getDynamicForm(this.form().Id())).asKoObservable());
        if (setLoading) {
            this.loading(false);
        }
    }

    async setBreadcrumbs() {
        await window.breadcrumbsViewModel.setBreadcrumbs([
            new Breadcrumb(window.Helper.String.getTranslatedString("DynamicForm"), "#/Crm.DynamicForms/DynamicFormList/IndexTemplate"),
            new Breadcrumb(this.getLocalizationText(null), window.location.hash)
        ]);
    }

    async submit() {
        if (this.form().CategoryKey() == "PDF-Checklist") {
            await this.savePdf();
        } else {
            await this.save();

        }
    }

    async removeCurrentFileResource() {
        const viewModel = this;
        this.loading(true);
        try {
            await window.Helper.Confirm.genericConfirm({
                text: window.Helper.String.getTranslatedString("ConfirmDelete"),
                type: "warning"
            });
        } catch {
            this.loading(false);
            return false;
        }
        const removeFileResource = viewModel.fileResources().find(function (data) {
            return data.language == viewModel.selectedLanguage();
        });
        if (removeFileResource) {
            viewModel.fileResources.remove(removeFileResource);
        }
        this.currentFileResource(null);
        this.loading(false);
        return true;
    }

    async savePdf() {
        const viewModel = this;

        if (this.form().Languages().length !== this.fileResources().length) {
            try {
                await window.Helper.Confirm.genericConfirm({
                    text: window.Helper.String.getTranslatedString("Warning_LanguageRequiresPdf"),
                    type: "warning"
                });
            } catch {
                return false;
            }
            return false;
        }
        const formErrors = ko.validation.group(this.form(), {deep: true});
        if (formErrors().length > 0) {
            $("a[href='#tab-edit-form']").trigger("click");
            formErrors.scrollToError();
            formErrors.showAllMessages();
            return false;
        }
        this.loading(true);

        await window.database.saveChanges();
        viewModel.form().Languages().forEach(function (language) {
            const data = viewModel.fileResources().find(function (data) {
                return data.language == language.LanguageKey();
            });
            if (data.fileResource.Id() !== language.FileResourceId()) {
                window.database.add(data.fileResource);
            }
            window.database.attachOrGet(language);
            if (data.fileResource.Id() !== language.FileResourceId() && language.FileResourceId() !== null) {
                viewModel.fileResourcesToRemove().push(language.FileResourceId());
            }
            language.FileResourceId(data.fileResource.Id());

        });

        await Promise.all(
            viewModel.fileResourcesToRemove().map((fileResourceId) => {
                return window.database.Main_FileResource
                    .find(fileResourceId)
                    .then(function (fileResource) {
                        window.database.remove(fileResource);
                    });
            })
        );
        await window.database.saveChanges();
        this.attachToDatabase();
        viewModel.fileResourcesToRemove([]);
        this.loading(false);
        return true;
    }

    languageChanged() {
        const viewModel = this;
        const changedFileResource = viewModel.fileResources().find(data => data.language === viewModel.selectedLanguage());
        if (changedFileResource) {
            viewModel.currentFileResource(changedFileResource.fileResource);
        } else {
            viewModel.currentFileResource(null);
        }
    }

    async uploadPdf(data, event) {
        const viewModel = this;
        if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
            const fileApiAlertString = window.Helper.getTranslatedString("M_FileApiNotSupported");
            alert(fileApiAlertString);
            return false;
        }
        this.loading(true);
        const files = event.target.files;
        if (files.length == 0) {
            viewModel.removeCurrentFileResource();
        } else {
            const file = files[0];
            if (file.type !== "application/pdf") {
                this.loading(false);
                window.swal(window.Helper.String.getTranslatedString("Warning"), window.Helper.String.getTranslatedString("PdfUploadRequired"), "warning");
                return false;
            }
            window.Log.debug("New uploaded file: ", file);
            const newFileResource = await this.getPdfFile(file);
            const oldFileResourceId = null;
            const data = {
                language: viewModel.selectedLanguage(),
                fileResource: newFileResource.asKoObservable()
            };
            const alreadyExists = viewModel.fileResources().find(function (data) {
                return data.language == viewModel.selectedLanguage();
            });
            if (alreadyExists) {
                viewModel.fileResources.remove(alreadyExists);
            }
            viewModel.fileResources.push(data);
            viewModel.currentFileResource(data.fileResource);
            event.target.value = "";
            viewModel.loading(false);
        }
        return true;
    }

    async getPdfFile(file) {
        const d = $.Deferred();
        const reader: FileReader = new FileReader();
        reader.onload = function (event) {
            const base64String = (event.target.result as string).split(",")[1];
            const fileResource = window.FormDesignerViewModel.prototype.createFileResource(file, base64String, file.size);
            d.resolve(fileResource);
        };
        reader.onerror = d.reject;
        reader.readAsDataURL(file);
        return d.promise();

    }

    createFileResource(file, content, size) {
        const currentUser = window.Helper.User.getCurrentUserName();
        var fileResource = window.database.Main_FileResource.defaultType.create();
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
    }

     async displayAlert() {
        const viewModel = this;
        this.loading(true);
        try {
            await window.Helper.Confirm.genericConfirm({
                text: window.Helper.String.getTranslatedString("PdfDynamicFormDesignerEmptySlate"),
                type: "warning"
            });
        } catch {
            this.loading(false);
            return false;
        }
        this.loading(false);
        return true;
    }

    showHintToast(): void {
        this.showSnackbar(window.Helper.String.getTranslatedString("DynamicFormDragDropToast"));
    }
}

namespace("Crm.DynamicForms.ViewModels").DynamicFormEditViewModel = DynamicFormEditViewModel;
namespace("Crm.DynamicForms.ViewModels").DynamicFormEditViewModel.prototype.createFileResource = window.FormDesignerViewModel.prototype.createFileResource;
namespace("Crm.DynamicForms.ViewModels").DynamicFormEditViewModel.prototype.handleFileSelect = window.FormDesignerViewModel.prototype.handleFileSelect;
namespace("Crm.DynamicForms.ViewModels").DynamicFormEditViewModel.prototype.processFile = window.FormDesignerViewModel.prototype.processFile;
