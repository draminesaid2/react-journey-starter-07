namespace Crm.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Model;

	public class StationConfiguration : EntityConfiguration<Station>
	{
		public StationConfiguration(IEntityConfigurationHolder<Station> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.Name, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.LegacyId, m =>
			{
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.ModifyDate, m => m.Sortable());
			Property(x => x.CreateDate, m => m.Sortable());
		}
	}
}