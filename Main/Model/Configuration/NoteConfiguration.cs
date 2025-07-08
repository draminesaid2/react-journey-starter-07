namespace Crm.Model.Configuration
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.EntityConfiguration;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization;
	using Crm.Model.Notes;
	using Crm.Services.Interfaces;

	public class NoteConfiguration : EntityConfiguration<Note>
	{
		private readonly IEnumerable<Plugin> activePlugins;
		private readonly IContactTypeProvider contactTypeProvider;
		public override void Initialize()
		{
			Property(x => x.CreateDate, c =>
				{
					c.Filterable(f => f.Definition(new DateFilterDefinition { AllowPastDates = true, AllowFutureDates = false }));
					c.Sortable();
				});
			Property(x => x.Text, c => c.Filterable());
			Property(x => x.ContactName, c => c.Filterable());
			Property(x => x.CreateUser, c => c.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = false, Caption = "CreatedBy" })));
			var dropDownFilter = new DropDownFilterDefinition(activePlugins.SelectMany(x => x.Assembly.GetTypesInheriting<Note>()).ToDictionary(x => x.Name, y => y.Name));
			Property(x => x.NoteType, c => c.Filterable(f => f.Definition(dropDownFilter)));
			var contactTypeFilterDefinition = new DropDownFilterDefinition(contactTypeProvider.ContactTypes.ToDictionary(x => x, x => x));
			Property(x => x.ContactType, c => c.Filterable(f => f.Definition(contactTypeFilterDefinition)));
			Property(x => x.Subject, c => c.Filterable());
		}

		public NoteConfiguration(IEntityConfigurationHolder<Note> entityConfigurationHolder, IEnumerable<Plugin> activePlugins, IContactTypeProvider contactTypeProvider)
			: base(entityConfigurationHolder)
		{
			this.activePlugins = activePlugins;
			this.contactTypeProvider = contactTypeProvider;
		}
	}
}