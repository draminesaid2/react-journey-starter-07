
using System;
using System.Collections.Generic;
using System.Linq;
using LMobile.MiniForms;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Portal.BusinessObjects;
using LMobile.MiniForms.Classic;
using System.Drawing;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Modules {
	class ModulesDirectView : ApplicationView<BaseView, ModulesDirectApplication> {
		public readonly BindingSource<ModuleProperty> Packages;
		
		public ModulesDirectView() {
			this.Packages = this.CreateCollectionBindingSource(Application, app => app.AllPackages);
		}

		protected override void Render(BaseView master) {
			master.BackButton.BindAction(Application, app => app.ExitApplication());

			var main = master.Content.AddLinesLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.H100);

			#region Package Editor
			var editor = main.AddColumnsLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody + new Style() { HorizontalSpacing = new Length(5, In.Points) });
			var editorFields = editor.AddTableLayout().SetStyle(ClassicStyleSheet.WRemainder)
					.BindDisplayed(Application, app => app.CurrentPackage != null);
					
			editorFields.AddRow(row => {
				row.AddLabel().SetCaption("Package Name:");
				row.AddTextBox()
					.SetStyle(ClassicStyleSheet.WRemainder + ClassicStyleSheet.W1in2)
					.BindReadOnly(Application, app => !app.CanUserEditPackage)
					.BindText(Application, app => app.CurrentPackage == null ? null : app.CurrentPackage.PropertyName, false, (app, value) => app.CurrentPackage.PropertyName = value);
			});
			
			editorFields.AddRow(row => {
				row.AddLabel().SetCaption("Description:");
				row.AddTextBox()
					.SetStyle(ClassicStyleSheet.WRemainder + ClassicStyleSheet.W1in2)
					.BindReadOnly(Application, app => !app.CanUserEditPackage)
					.BindText(Application, app => app.CurrentPackage == null ? null : app.CurrentPackage.Description, false, (app, value) => app.CurrentPackage.Description = value);
			});

			var editorButtons = editor.AddLinesLayout().BindDisplayed(Application, app => app.CurrentPackage != null);
			editorButtons.AddActionButton()
					.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Disk))
					.BindDisplayed(Application, app => app.CanUserEditPackage)
					.BindAction(Application, app => app.SaveCurrentPackage());
			editorButtons.AddActionButton()
					.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.ArrowLeft))
					.BindDisplayed(Application, app => app.CanUserEditPackage)
					.BindAction(Application, app => app.ResetCurrentPackage());
			#endregion

			#region Packages List
			var scroll = main.AddScrollPanel(Scrolling.Vertical).SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.HRemainder +
				new Style { RightPadding = new Length(25, In.Pixels) });

			var packagesTable = scroll.Layout.AddTableLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody +
				new Style { VerticalSpacing = new Length(4, In.Points) });

			// Header row
			packagesTable.AddRow(row => {
				row.SetChildStyle(ClassicStyleSheet.Bold);
				row.AddLabel().SetCaption("ID").SetStyle(new Style { Width = new Length(50, In.Pixels) });
				row.AddLabel().SetCaption("Package Name").SetStyle(new Style { Width = new Length(200, In.Pixels) });
				row.AddLabel().SetCaption("Description");
				row.AddLabel().SetCaption("Actions").SetStyle(new Style { Width = new Length(120, In.Pixels) });
			});

			// Package rows
			this.AddIteration(Packages, () => {
				packagesTable.AddRow(row => {
					row.AddLabel()
						.BindCaption(Packages, package => package.ID.ToString())
						.SetStyle(new Style { Width = new Length(50, In.Pixels) });

					row.AddLabel()
						.BindCaption(Packages, package => package.PropertyName ?? "")
						.SetStyle(new Style { Width = new Length(200, In.Pixels) });

					row.AddLabel()
						.BindCaption(Packages, package => package.Description ?? "")
						.SetStyle(ClassicStyleSheet.WRemainder);

					// Action buttons
					var actionLayout = row.AddColumnsLayout().SetStyle(new Style { Width = new Length(120, In.Pixels) });
					
					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Pencil) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindAction(Application, Packages, (app, package) => app.NavigateEditPackage(package.ID));

					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Bin) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindDisplayed(Application, app => app.CanUserDeletePackage)
						.BindAction(Application, Packages, (app, package) => app.DeletePackage(package.ID));

				}).SetStyle(new Style { Border = new Length(1, In.Pixels), BorderColor = Color.LightGray });
			});
			#endregion

			// Navigation buttons
			master.Navigation.AddActionButton()
						.SetCaption("New Package")
						.BindAction(Application, app => app.NavigateEditPackage(0));
		}
	}
}
