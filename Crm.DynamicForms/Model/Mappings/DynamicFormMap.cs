namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class DynamicFormMap : EntityClassMapping<DynamicForm>
	{
		public DynamicFormMap()
		{
			Schema("CRM");
			Table("DynamicForm");

			Id(
				x => x.Id,
				map =>
				{
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
					map.Column("DynamicFormId");
				});

			Property(x => x.CategoryKey);
			Property(x => x.DefaultLanguageKey);
			Property(x => x.HideEmptyOptional);

			this.EntityBag(
				x => x.Elements,
				m =>
				{
					m.Key(
						km =>
						{
							km.Column("DynamicFormKey");
							km.NotNullable(true);
						});
					//m.Cascade(Cascade.All);
					m.Inverse(true);
					m.Fetch(CollectionFetchMode.Select);
					m.Lazy(CollectionLazy.Lazy);
				},
				a => a.OneToMany());
			this.EntitySet(
				x => x.Languages,
				map =>
				{
					map.Key(km => km.Column("DynamicFormKey"));
					map.Inverse(true);
					map.Fetch(CollectionFetchMode.Select);
					map.Lazy(CollectionLazy.Lazy);
					map.BatchSize(100);
				},
				a => a.OneToMany());
			this.EntitySet(
				x => x.Localizations,
				map =>
				{
					map.Key(km => km.Column("DynamicFormId"));
					map.Inverse(true);
					map.Fetch(CollectionFetchMode.Select);
					map.Lazy(CollectionLazy.Lazy);
					map.BatchSize(25);
					map.Filter(DynamicFormLocalizationFilter.Name, DynamicFormLocalizationFilter.FilterMapping);
				},
				a => a.OneToMany());
		}
	}
}