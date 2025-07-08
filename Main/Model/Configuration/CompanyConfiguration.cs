namespace Crm.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class CompanyConfiguration : EntityConfiguration<Company>
	{
		public override void Initialize()
		{
			Property(x => x.Name, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.LegacyId, m => m.Filterable());
			Property(x => x.ShortText, m => m.Filterable());
			Property(x => x.CompanyNo, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.CompanyType, m => m.Filterable());
			Property(x => x.ModifyDate, m => m.Sortable());
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition())));
			Property(x => x.AreaSalesManager, m => m.Filterable(f => f.Definition(new UserFilterDefinition())));
			NestedProperty(x => x.StandardAddress.Country, m => m.Filterable());
			NestedProperty(x => x.StandardAddress.Street, m => m.Filterable());
			NestedProperty(x => x.StandardAddress.ZipCode, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			NestedProperty(x => x.StandardAddress.City, m => m.Filterable());
			NestedProperty(x => x.StandardAddress.Latitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			NestedProperty(x => x.StandardAddress.Longitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			Property(x => x.Communications, m => m.Filterable(f => f.Definition(new CollectionFilterDefinition<Communication>(c => c.Data, c => c.DataOnlyNumbers))));
			Property(x => x.CompanyBranches, m => m.Filterable(f => f.Definition(new DependantLookupsFilterDefinition<CompanyBranch>(c => c.Branch1, c=>c.Branch2, c=>c.Branch3, c=> c.Branch4))));
			Property(x => x.CompanyGroupFlag1, c => c.Filterable());
			Property(x => x.CompanyGroupFlag2, c => c.Filterable());
			Property(x => x.CompanyGroupFlag3, c => c.Filterable());
			Property(x => x.CompanyGroupFlag4, c => c.Filterable());
			Property(x => x.CompanyGroupFlag5, c => c.Filterable());
			Property(x => x.CreateDate, m => m.Sortable());
			Property(x => x.IsEnabled, m =>
			{
				m.Filterable(f => f.Caption("Enabled"));
				m.Sortable(s => s.SortCaption("Enabled"));
			});

		}
		public CompanyConfiguration(IEntityConfigurationHolder<Company> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
