namespace Crm.Article.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ProductFamilyConfiguration : EntityConfiguration<ProductFamily>
	{
		public ProductFamilyConfiguration(IEntityConfigurationHolder<ProductFamily> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.Descriptions,m =>
			{
				m.Filterable(f => f.Caption("Description"));
				m.Sortable();
			});
			Property(x => x.Status, m => m.Filterable());
			Property(x => x.StatusKey, m => m.Sortable());
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = false })));
			Property(x => x.ModifyDate, m => m.Sortable());
			Property(x => x.CreateDate, m =>
			{
				m.Sortable();
				m.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
			});
		}
	}
}
