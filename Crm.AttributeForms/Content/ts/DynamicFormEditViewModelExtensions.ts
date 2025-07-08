import { DynamicFormEditViewModel } from "../../../Crm.DynamicForms/Content/ts/DynamicFormEditViewModel"
(function () {
	let baseInit = DynamicFormEditViewModel.prototype.init;
	DynamicFormEditViewModel.prototype.init = async function init(id) {
		await baseInit.call(this, id);
		if (this.form().CategoryKey() === "AttributeForm") {
			this.disableRules(true);
			this.disableSizeSelection(true);
			this.disableRowSizeSelection(true);
			let hiddenFormElementTypes = ["FileAttachmentDynamicFormElement", "Image", "SignaturePad", "SignaturePadWithPrivacyPolicy", "Literal", "PageSeparator", "SectionSeparator"];
			this.formElementTypes.remove(formElementType => {
				return hiddenFormElementTypes.indexOf(formElementType.FormElementType()) !== -1;
			});
			this.formElementTypes().forEach(formElementType => {
				let baseClone = formElementType.clone;
				formElementType.clone = () => {
					let clone = baseClone();
					clone.Size(1);
					return clone;
				}
			})
		}
	};
})();