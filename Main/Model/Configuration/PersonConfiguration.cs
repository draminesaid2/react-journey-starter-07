namespace Crm.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Helper;

	public class PersonConfiguration : EntityConfiguration<Person>
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		public PersonConfiguration(IEntityConfigurationHolder<Person> entityConfigurationHolder, IAppSettingsProvider appSettingsProvider)
			: base(entityConfigurationHolder)
		{
			this.appSettingsProvider = appSettingsProvider;
		}
		public override void Initialize()
		{
			Property(x => x.Surname, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.Firstname, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.Name, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.PersonNo, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.LegacyId, m => m.Filterable());
			Property(x => x.CreateDate, m => m.Sortable());
			Property(x => x.ModifyDate, m => m.Sortable());
			Property(x => x.ParentId, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete", new { Plugin = "Main" }, "Main_Company", x => x.Name, x => x.Id, x => x.LegacyId, x => x.Name) { ShowOnMaterialTab = false })));
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition())));
			Property(x => x.Communications, m => m.Filterable(f => f.Definition(new CollectionFilterDefinition<Communication>(c => c.Data, c => c.DataOnlyNumbers))));
			Property(x => x.IsRetired, m =>
			{
				m.Filterable(f => f.Caption("Retired"));
				m.Sortable(s => s.SortCaption("Retired"));
			});

			if (appSettingsProvider.GetValue(MainPlugin.Settings.Person.BusinessTitleIsLookup))
			{
				Property(x => x.BusinessTitle, c => c.Filterable());
			}
			else
			{
				Property(x => x.BusinessTitleKey, c => c.Filterable(f => f.Caption("BusinessTitle")));
			}
			if (appSettingsProvider.GetValue(MainPlugin.Settings.Person.DepartmentIsLookup))
			{
				Property(x => x.DepartmentType, c => c.Filterable());
			}
			else
			{
				Property(x => x.DepartmentTypeKey, c => c.Filterable(f => f.Caption("Department")));
			}
		}
		public PersonConfiguration(IEntityConfigurationHolder<Person> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}