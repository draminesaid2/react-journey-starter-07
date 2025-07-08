namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class TagMap : EntityClassMapping<Tag>
	{
		public TagMap()
		{
			Schema("CRM");
			Table("ContactTags");

			Id(x => x.Id, map =>
			{
				map.Column("Id");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.ContactKey, map =>
			{
				map.Insert(true);
				map.Update(true);
			});
			Property(x => x.Name, map => map.Column("TagName"));
			ManyToOne(x => x.Contact, map =>
			{
				map.Column("ContactKey");

				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
				map.NotNullable(true);
			});
		}
	}
}
