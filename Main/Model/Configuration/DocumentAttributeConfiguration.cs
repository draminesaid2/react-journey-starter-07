namespace Crm.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class DocumentAttributeConfiguration : EntityConfiguration<DocumentAttribute>
	{
		public override void Initialize()
		{
			Property(x => x.Description, m => m.Filterable());
			NestedProperty(x => x.FileResource.CreateDate, m =>
			{
				m.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				m.Sortable();
			});
		}
		public DocumentAttributeConfiguration(IEntityConfigurationHolder<DocumentAttribute> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}