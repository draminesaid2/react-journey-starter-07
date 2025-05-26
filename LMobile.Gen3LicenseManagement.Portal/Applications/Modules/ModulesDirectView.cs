
﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMobile.MiniForms;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Portal.BusinessObjects;
using LMobile.MiniForms.Classic;
using System.Drawing;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Modules {
	class ModulesDirectView : ApplicationView<BaseView, ModulesDirectApplication> {
		public readonly BindingSource<TreeWrapper<Module, ModuleProperty>> Modules;
		public readonly BindingSource<ModuleProperty> Properties;
		
		public ModulesDirectView() {
			this.Modules = this.CreateCollectionBindingSource(Application, app => app.Modules);
			this.Properties = CreateCollectionBindingSource(Modules, module => module.Children);
		}

		protected override void Render(BaseView master) {
			master.BackButton.BindAction(Application, app => app.ExitApplication());

			var main = master.Content.AddLinesLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.H100);

			#region Module Editor
			var editor = main.AddColumnsLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody + new Style() { HorizontalSpacing = new Length(5, In.Points) });
			var editorFields = editor.AddTableLayout().SetStyle(ClassicStyleSheet.WRemainder)
					.BindDisplayed(Application, app => app.CurrentModule != null);
					
			editorFields.AddRow(row => {
				row.AddLabel().SetCaption(Resources.ProjectType());
				row.AddDropDownBox<StoredProjectType>(pt => pt.ProjectType)
					.SetStyle(ClassicStyleSheet.WRemainder + ClassicStyleSheet.W1in7)
					.BindEnabled(Application, app => app.CanUserEditModule)
					.BindItems(Application, app => app.ProjectTypes)
					.BindSelectedItem(Application, app => app.CurrentModule == null ? null : app.ProjectTypes.FirstOrDefault(pt => pt.ProjectType == app.CurrentModule.ProjectType), false,
										 (app, value) => app.CurrentModule.ProjectType = value.ProjectType);
			});
			
			editorFields.AddRow(row => {
				row.AddLabel().SetCaption(Resources.Name());
				row.AddTextBox()
					.SetStyle(ClassicStyleSheet.WRemainder + ClassicStyleSheet.W1in2)
					.BindReadOnly(Application, app => !app.CanUserEditModule)
					.BindText(Application, app => app.CurrentModule == null ? null : app.CurrentModule.ModuleName, false, (app, value) => app.CurrentModule.ModuleName = value);
			});
			
			editorFields.AddRow(row => {
				row.AddLabel().SetCaption(Resources.Description());
				row.AddTextBox()
					.SetStyle(ClassicStyleSheet.WRemainder + ClassicStyleSheet.W1in2)
					.BindReadOnly(Application, app => !app.CanUserEditModule)
					.BindText(Application, app => app.CurrentModule == null ? null : app.CurrentModule.Description, false, (app, value) => app.CurrentModule.Description = value);
			});

			// GUID display
			editorFields.AddRow(row => {
				var guidRow = row.AddLinesLayout().Span(2, 1);
				guidRow.AddLabel()
					.SetCaption(string.Format("{0}: ", Resources.Guid()))
					.SetStyle(new Style { FontSize = new Length(8, In.Points) });
				guidRow.AddLabel()
					.SetStyle(new Style { FontSize = new Length(8, In.Points) })
					.BindCaption(Application, app => app.CurrentModule == null ? null : app.CurrentModule.ModuleGuid.ToString());
			});

			var editorButtons = editor.AddLinesLayout().BindDisplayed(Application, app => app.CurrentModule != null);
			editorButtons.AddActionButton()
					.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Disk))
					.BindDisplayed(Application, app => app.CanUserEditModule)
					.BindAction(Application, app => app.SaveCurrentModule());
			editorButtons.AddActionButton()
					.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.ArrowLeft))
					.BindDisplayed(Application, app => app.CanUserEditModule)
					.BindAction(Application, app => app.ResetCurrentModule());
			#endregion

			#region Modules List with Packages
			var scroll = main.AddScrollPanel(Scrolling.Vertical).SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.HRemainder +
				new Style { RightPadding = new Length(25, In.Pixels) });

			var modulesTable = scroll.Layout.AddTableLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody +
				new Style { VerticalSpacing = new Length(4, In.Points) });

			// Header row
			modulesTable.AddRow(row => {
				row.SetChildStyle(ClassicStyleSheet.Bold);
				row.AddLabel().SetStyle(new Style { Width = new Length(25, In.Pixels) }); // Space for expand/collapse button
				row.AddLabel().SetCaption("ID").SetStyle(new Style { Width = new Length(50, In.Pixels) });
				row.AddLabel().SetCaption(Resources.ProjectType()).SetStyle(new Style { Width = new Length(120, In.Pixels) });
				row.AddLabel().SetCaption(Resources.Name()).SetStyle(new Style { Width = new Length(150, In.Pixels) });
				row.AddLabel().SetCaption(Resources.Description());
				row.AddLabel().SetCaption("Actions").SetStyle(new Style { Width = new Length(120, In.Pixels) });
			});

			// Module rows with expandable packages
			this.AddIteration(Modules, () => {
				modulesTable.AddRow(row => {
					row.AddToggleButton().SetStyle(ClassicStyleSheet.ExpandCollapseSmall)
						.BindChecked(Modules, module => module.Expanded, true, (module, value) => module.Expanded = value);

					row.AddLabel()
						.BindCaption(Modules, module => module.Node.ID.ToString())
						.SetStyle(new Style { Width = new Length(50, In.Pixels) });

					// Combined ProjectType with package count
					var typeCell = row.AddColumnsLayout().SetStyle(new Style { Width = new Length(120, In.Pixels) });
					typeCell.AddLabel()
						.BindCaption(Modules, module => module.Node.ProjectType ?? "");
					typeCell.AddLabel()
						.BindCaption(Modules, module => string.Format(" ({0})", module.Children.Count))
						.SetStyle(new Style { FontSize = new Length(9, In.Points), FontStyle = System.Drawing.FontStyle.Italic });

					row.AddLabel()
						.BindCaption(Modules, module => module.Node.ModuleName ?? "")
						.SetStyle(new Style { Width = new Length(150, In.Pixels) });

					row.AddLabel()
						.BindCaption(Modules, module => module.Node.Description ?? "")
						.SetStyle(ClassicStyleSheet.WRemainder);

					// Action buttons
					var actionLayout = row.AddColumnsLayout().SetStyle(new Style { Width = new Length(120, In.Pixels) });
					
					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Pencil) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindAction(Application, Modules, (app, module) => app.NavigateEditModule(module.Node.ID));

					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Bin) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindDisplayed(Application, app => app.CanUserDeleteModule)
						.BindAction(Application, Modules, (app, module) => app.DeleteModule(module.Node.ID));

				}).SetStyle(new Style { Border = new Length(1, In.Pixels), BorderColor = Color.LightGray });

				// Expandable packages section
				modulesTable.AddRow(row => {
					row.BindDisplayed(Modules, module => module.Expanded);
					row.AddLabel(); // Empty cell for expand button column
					
					var packageTable = row.AddTableLayout().Span(5, 1).SetStyle(new Style {
						LeftMargin = new Length(20, In.Pixels),
						BackgroundColor = Color.LightGray,
						RightMargin = new Length(25, In.Pixels)
					});

					packageTable.AddRow(propRow => {
						propRow.SetChildStyle(ClassicStyleSheet.Bold);
						propRow.AddLabel().SetCaption("Package ID").SetStyle(new Style { Width = new Length(80, In.Pixels) });
						propRow.AddLabel().SetCaption("Package Name").SetStyle(new Style { Width = new Length(120, In.Pixels) });
						propRow.AddLabel().SetCaption("Package Description").SetStyle(ClassicStyleSheet.WRemainder);
					});

					packageTable.AddIteration(Properties, i => {
						packageTable.AddRow(propRow => {
							propRow.AddLabel()
								 .BindCaption(Properties, prop => prop.ID.ToString())
								 .SetStyle(new Style { Width = new Length(80, In.Pixels) });

							propRow.AddLabel()
								 .BindCaption(Properties, prop => prop.PropertyName ?? "")
								 .SetStyle(new Style { Width = new Length(120, In.Pixels) });

							propRow.AddLabel()
								 .BindCaption(Properties, prop => prop.Description ?? "")
								 .SetStyle(ClassicStyleSheet.WRemainder);
						});
					});
				});
			});
			#endregion

			// Navigation buttons
			master.Navigation.AddActionButton()
						.SetCaption("New Module")
						.BindAction(Application, app => app.NavigateEditModule(0));
		}
	}
}
