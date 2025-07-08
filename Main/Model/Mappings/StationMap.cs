namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	public class StationMap : EntityClassMapping<Station>
	{
		public StationMap()
		{
			Schema("CRM");
			Table("Station");

			Id(a => a.Id, m =>
			{
				m.Column("StationId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Name);
			Property(x => x.LegacyId);
			Property(x => x.IsEnabled);
		}
	}
}
