///<reference path="../../../../Content/@types/index.d.ts"/>

import {DynamicFormEditViewModel} from "../ts/DynamicFormEditViewModel";

declare global {
	namespace Crm {
		namespace DynamicForms {
			namespace Settings {
				namespace DynamicFormElement {
					namespace SignaturePad {
						namespace Show {
							let PrivacyPolicy: boolean;
						}
					}
				}
			}
			namespace ViewModels {
				let DynamicFormEditViewModel: DynamicFormEditViewModel;
				let DynamicFormElementRuleEditorViewModel: any;
				let DynamicFormLocalizationEditorViewModel: any;
			}
		}
	}

	interface Window {
		FormDesignerViewModel: any;
	}
}