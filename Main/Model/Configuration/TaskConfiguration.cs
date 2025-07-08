namespace Crm.Model.Configuration
{
	using System;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Extensions;

	public class TaskConfiguration : EntityConfiguration<Task>
	{
		public TaskConfiguration(IEntityConfigurationHolder<Task> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{

		    Property(x => x.CreateDate, c =>
		    {
		        c.Filterable(f => f.Definition(new DateFilterDefinition { AllowPastDates = true, AllowFutureDates = false }));
		        c.Sortable();
		    });
		    NestedProperty(x => x.Contact.Name, m => m.Filterable(c => c.Caption("ContactName")));
		    Property(x => x.ContactId, m => m.Filterable(
			    f =>
			    {
				    f.Definition(new AutoCompleterFilterDefinition<Contact>("ContactAutocomplete", new { Plugin = "Main" }, null, x => x.Name, x => x.Id, x => x.LegacyId, x => x.Name));
					f.Caption("ParentName");

			    }));
            Property(x => x.Text, m => m.Filterable());
			Property(x => x.DueDate, m =>
			{
				m.Filterable(f => f.Definition(new DateFilterDefinition { AllowPastDates = true, AllowFutureDates = true }));
				m.Sortable();
			});
			Property(x => x.TypeKey, m => m.Sortable(s => s.SortCaption("TaskType")));
			Property(x => x.Type, m => m.Filterable());
			Property(x => x.ResponsibleUser, c => c.Filterable(f => f.Definition(new UserFilterDefinition { CustomFilterExpression = true, WithGroups = true, FilterForGroup = true })));
		}
	}
	public static class TaskExtension
	{
		public static string GetIcsSummary(this Task x)
		{
			return String.Format("{0}{1} {2}", x.Type.IsNull() ? x.Type.Value + ": " : String.Empty, x.Text, x.ResponsibleUserObject != null ? $"({x.ResponsibleUserObject.DisplayName})" : string.Empty);
		}
	}
}