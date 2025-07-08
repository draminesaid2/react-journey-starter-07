namespace Crm.DynamicForms.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Cfg.MappingSchema;
	using NHibernate.Mapping.ByCode;

	public class DynamicFormReferenceMap : EntityClassMapping<DynamicFormReference>
	{
		public DynamicFormReferenceMap()
		{
			Schema("CRM");
			Table("DynamicFormReference");
			Discriminator(m => m.Column("DynamicFormReferenceType"));

			Id(x => x.Id, map =>
			{
				map.Column("DynamicFormReferenceId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(HbmUnsavedValueType.Undefined.ToNullValue());
			});
			Property(x => x.ReferenceType, map =>
			{
				map.Column("DynamicFormReferenceType");
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.LegacyId);

			Property(x => x.DynamicFormKey);
			ManyToOne(x => x.DynamicForm, map =>
			{
				map.Column("DynamicFormKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			this.EntitySet(x => x.Responses, map =>
			{
				map.Schema("CRM");
				map.Table("DynamicFormResponse");
				map.Key(km => km.Column("DynamicFormReferenceKey"));
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.All);
				map.BatchSize(25);
			    map.Inverse(true);
            }, a => a.OneToMany());

			Property(x => x.ReferenceKey);
			Property(x => x.Completed);
		}
	}
}