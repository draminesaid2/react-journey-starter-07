namespace Crm.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Mapping.ByCode;
	public class DocumentAttributeMap : EntityClassMapping<DocumentAttribute>
	{
		public DocumentAttributeMap()
		{
			Schema("CRM");
			Table("DocumentAttributes");

			Id(x => x.Id, map =>
			{
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(HbmUnsavedValueType.Undefined.ToNullValue());
			});

			Property(x => x.ReferenceType);
			Property(x => x.ReferenceKey);
			Property(x => x.Length);
			Property(x => x.DocumentCategoryKey);
			Property(x => x.Description);
			Property(x => x.LongText);
			Property(x => x.FileName);
			Property(x => x.OfflineRelevant);
			Property(x => x.FileResourceKey);
			Property(x => x.UseForThumbnail);

			ManyToOne(x => x.FileResource, map =>
				{
					map.Column("FileResourceKey");
					map.Fetch(FetchKind.Select);
					map.Lazy(LazyRelation.Proxy);
					map.Cascade(Cascade.None);
					map.Insert(false);
					map.Update(false);
				});

			ManyToOne(x => x.Contact, map =>
			{
				map.Column("ReferenceKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}