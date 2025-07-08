namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.DynamicForms.Model.Enums;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class DynamicFormElementRuleMap : EntityClassMapping<DynamicFormElementRule>
	{
		public DynamicFormElementRuleMap()
		{
			Schema("CRM");
			Table("DynamicFormElementRule");

			Id(x => x.Id, map =>
			{
				map.Column("DynamicFormElementRuleId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DynamicFormId);
			Property(x => x.DynamicFormElementId);
			Property(x => x.MatchType, m => m.Type<EnumStringType<DynamicFormElementRuleMatchType>>());
			Property(x => x.Type, m => m.Type<EnumStringType<DynamicFormElementRuleType>>());
			
			ManyToOne(x => x.DynamicForm, m =>
			{
				m.Column("DynamicFormId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.DynamicFormElement, m =>
			{
				m.Column("DynamicFormElementId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			this.EntitySet(
				x => x.Conditions,
				m =>
				{
					m.Key(
						km =>
						{
							km.Column("DynamicFormElementRuleId");
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
