namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	public class LinkResourceMap : EntityClassMapping<LinkResource>
	{
		public LinkResourceMap()
		{
			Schema("CRM");
			Table("LinkResource");

			Id(x => x.Id, map =>
				{
					map.Column("Id");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ParentId, map => map.Column("ElementKey"));

			Property(x => x.Url);
			Property(x => x.Description);
		}
	}
}