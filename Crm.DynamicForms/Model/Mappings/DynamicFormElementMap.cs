namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using Crm.DynamicForms.Model.BaseModel;

	public class DynamicFormElementMap : EntityClassMapping<DynamicFormElement>
	{
		public DynamicFormElementMap()
		{
			Schema("CRM");
			Table("DynamicFormElement");
			Discriminator(x => x.Column("FormElementType"));

			Id(x => x.Id, map =>
				{
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
					map.Column("DynamicFormElementId");
				});
			Property(x => x.LegacyId);
			Property(x => x.DynamicFormKey);
			Property(x => x.SortOrder);
			Property(x => x.Size);
			Property(x => x.CssExtra);

			this.EntitySet(
				x => x.Localizations,
				map =>
				{
					map.Key(km => km.Column("DynamicFormElementId"));
					map.Inverse(true);
					map.Fetch(CollectionFetchMode.Select);
					map.Lazy(CollectionLazy.Lazy);
					map.BatchSize(25);
				},
				a => a.OneToMany());
			this.EntitySet(
				x => x.Rules,
				m =>
				{
					m.Key(
						km =>
						{
							km.Column("DynamicFormElementId");
							km.NotNullable(true);
						});
					m.Inverse(true);
					m.Fetch(CollectionFetchMode.Select);
					m.Lazy(CollectionLazy.Lazy);
					m.BatchSize(100);
				},
				a => a.OneToMany());
		}
	}
}
