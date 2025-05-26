
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

			#region Header Section
			var header = main.AddLinesLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionHeader);
			header.AddLabel()
				.SetCaption("Package Management")
				.SetStyle(ClassicStyleSheet.Bold + new Style { FontSize = 14 });
			#endregion

			#region Search Section
			var searchSection = main.AddColumnsLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody + 
				new Style() { BottomPadding = new Length(10, In.Points) });
			
			searchSection.AddLabel()
				.SetCaption("Search:")
				.SetStyle(new Style { Width = new Length(60, In.Pixels), TopMargin = new Length(5, In.Points) });
			
			searchSection.AddTextBox()
				.SetStyle(ClassicStyleSheet.W1in2 + new Style { RightMargin = new Length(10, In.Points) })
				.BindText(Application, app => app.SearchText, true, (app, value) => app.SearchText = value);
			
			searchSection.AddActionButton()
				.SetCaption("Search")
				.SetStyle(ClassicStyleSheet.ContentButton + new Style { Width = new Length(80, In.Pixels) })
				.BindAction(Application, app => app.FilterPackages());
			
			searchSection.AddActionButton()
				.SetCaption("Clear")
				.SetStyle(ClassicStyleSheet.ContentButton + new Style { Width = new Length(60, In.Pixels), LeftMargin = new Length(5, In.Points) })
				.BindAction(Application, app => app.ClearSearch());
			#endregion

			#region Package Editor
			var editor = main.AddColumnsLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody + 
				new Style() { HorizontalSpacing = new Length(5, In.Points), BottomPadding = new Length(10, In.Points) })
				.BindDisplayed(Application, app => app.CurrentPackage != null);

			var editorCard = editor.AddLinesLayout().SetStyle(ClassicStyleSheet.WRemainder + 
				new Style { Border = new Length(1, In.Pixels), BorderColor = Color.LightBlue, 
				LeftPadding = new Length(10, In.Points), RightPadding = new Length(10, In.Points),
				TopPadding = new Length(10, In.Points), BottomPadding = new Length(10, In.Points) });

			editorCard.AddLabel()
				.SetCaption("Edit Package")
				.SetStyle(ClassicStyleSheet.Bold + new Style { BottomMargin = new Length(8, In.Points) });
					
			var editorFields = editorCard.AddTableLayout().SetStyle(ClassicStyleSheet.WRemainder);
					
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

			var editorButtons = editorCard.AddColumnsLayout().SetStyle(new Style { TopMargin = new Length(10, In.Points) });
			editorButtons.AddActionButton()
					.SetCaption("Save")
					.SetStyle(ClassicStyleSheet.ContentButton + new Style { Width = new Length(80, In.Pixels), RightMargin = new Length(5, In.Points) })
					.BindDisplayed(Application, app => app.CanUserEditPackage)
					.BindAction(Application, app => app.SaveCurrentPackage());
			editorButtons.AddActionButton()
					.SetCaption("Cancel")
					.SetStyle(ClassicStyleSheet.ContentButton + new Style { Width = new Length(80, In.Pixels) })
					.BindAction(Application, app => app.ResetCurrentPackage());
			#endregion

			#region Packages List
			var listSection = main.AddLinesLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.HRemainder);
			
			listSection.AddLabel()
				.SetCaption("All Packages")
				.SetStyle(ClassicStyleSheet.Bold + new Style { BottomMargin = new Length(5, In.Points) });

			var scroll = listSection.AddScrollPanel(Scrolling.Vertical).SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.HRemainder +
				new Style { RightPadding = new Length(25, In.Pixels), Border = new Length(1, In.Pixels), BorderColor = Color.LightGray });

			var packagesTable = scroll.Layout.AddTableLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody +
				new Style { VerticalSpacing = new Length(2, In.Points) });

			// Header row
			packagesTable.AddRow(row => {
				row.SetChildStyle(ClassicStyleSheet.Bold + new Style { BackgroundColor = Color.LightGray });
				row.AddLabel().SetCaption("ID").SetStyle(new Style { Width = new Length(60, In.Pixels), LeftPadding = new Length(5, In.Points) });
				row.AddLabel().SetCaption("Package Name").SetStyle(new Style { Width = new Length(250, In.Pixels) });
				row.AddLabel().SetCaption("Description").SetStyle(ClassicStyleSheet.WRemainder);
				row.AddLabel().SetCaption("Actions").SetStyle(new Style { Width = new Length(100, In.Pixels) });
			});

			// Package rows
			this.AddIteration(Packages, () => {
				packagesTable.AddRow(row => {
					row.AddLabel()
						.BindCaption(Packages, package => package.ID.ToString())
						.SetStyle(new Style { Width = new Length(60, In.Pixels), LeftPadding = new Length(5, In.Points) });

					row.AddLabel()
						.BindCaption(Packages, package => package.PropertyName ?? "")
						.SetStyle(new Style { Width = new Length(250, In.Pixels), FontWeight = FontWeight.Bold });

					row.AddLabel()
						.BindCaption(Packages, package => package.Description ?? "")
						.SetStyle(ClassicStyleSheet.WRemainder);

					// Action buttons
					var actionLayout = row.AddColumnsLayout().SetStyle(new Style { Width = new Length(100, In.Pixels) });
					
					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Pencil) +
							new Style {
								Width = new Length(30, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindAction(Application, Packages, (app, package) => app.NavigateEditPackage(package.ID));

					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Bin) +
							new Style {
								Width = new Length(30, In.Pixels)
							})
						.BindDisplayed(Application, app => app.CanUserDeletePackage)
						.BindAction(Application, Packages, (app, package) => app.DeletePackage(package.ID));

				}).SetStyle(new Style { Border = new Length(1, In.Pixels), BorderColor = Color.LightGray });
			});
			#endregion

			// Navigation buttons
			master.Navigation.AddActionButton()
						.SetCaption("New Package")
						.SetStyle(ClassicStyleSheet.ContentButton)
						.BindAction(Application, app => app.NavigateEditPackage(0));
		}
	}
}
