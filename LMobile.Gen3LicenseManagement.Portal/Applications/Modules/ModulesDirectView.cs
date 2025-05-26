
﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMobile.MiniForms;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.MiniForms.Classic;
using System.Drawing;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Modules {
	class ModulesDirectView : ApplicationView<BaseView, ModulesDirectApplication> {
		public readonly BindingSource<Module> AllModules;
		
		public ModulesDirectView() {
			this.AllModules = this.CreateCollectionBindingSource(Application, app => app.AllModules);
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

			#region Modules List
			var scroll = main.AddScrollPanel(Scrolling.Vertical).SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.HRemainder +
				new Style { RightPadding = new Length(25, In.Pixels) });

			var modulesTable = scroll.Layout.AddTableLayout().SetStyle(ClassicStyleSheet.W100 + ClassicStyleSheet.SectionBody +
				new Style { VerticalSpacing = new Length(4, In.Points) });

			// Header row
			modulesTable.AddRow(row => {
				row.SetChildStyle(ClassicStyleSheet.Bold);
				row.AddLabel().SetCaption("ID").SetStyle(new Style { Width = new Length(50, In.Pixels) });
				row.AddLabel().SetCaption(Resources.ProjectType()).SetStyle(new Style { Width = new Length(120, In.Pixels) });
				row.AddLabel().SetCaption(Resources.Name()).SetStyle(new Style { Width = new Length(150, In.Pixels) });
				row.AddLabel().SetCaption(Resources.Description());
				row.AddLabel().SetCaption("Actions").SetStyle(new Style { Width = new Length(120, In.Pixels) });
			});

			// Module rows
			this.AddIteration(AllModules, () => {
				modulesTable.AddRow(row => {
					row.AddLabel()
						.BindCaption(AllModules, module => module.ID.ToString())
						.SetStyle(new Style { Width = new Length(50, In.Pixels) });

					row.AddLabel()
						.BindCaption(AllModules, module => module.ProjectType ?? "")
						.SetStyle(new Style { Width = new Length(120, In.Pixels) });

					row.AddLabel()
						.BindCaption(AllModules, module => module.ModuleName ?? "")
						.SetStyle(new Style { Width = new Length(150, In.Pixels) });

					row.AddLabel()
						.BindCaption(AllModules, module => module.Description ?? "")
						.SetStyle(ClassicStyleSheet.WRemainder);

					// Action buttons
					var actionLayout = row.AddColumnsLayout().SetStyle(new Style { Width = new Length(120, In.Pixels) });
					
					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Pencil) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindAction(Application, AllModules, (app, module) => app.NavigateEditModule(module.ID));

					actionLayout.AddActionButton()
						.SetStyle(ClassicStyleSheet.ContentIconButton(MonoIcon.Bin) +
							new Style {
								Width = new Length(35, In.Pixels),
								RightMargin = new Length(5, In.Pixels)
							})
						.BindDisplayed(Application, app => app.CanUserDeleteModule)
						.BindAction(Application, AllModules, (app, module) => app.DeleteModule(module.ID));

				}).SetStyle(new Style { Border = new Length(1, In.Pixels), BorderColor = Color.LightGray });
			});
			#endregion

			// Navigation buttons
			master.Navigation.AddActionButton()
						.SetCaption("New Module")
						.BindAction(Application, app => app.NavigateEditModule(0));
		}
	}
}
