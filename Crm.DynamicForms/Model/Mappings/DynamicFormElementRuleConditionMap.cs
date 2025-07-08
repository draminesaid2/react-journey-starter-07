namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class DynamicFormElementRuleConditionMap : EntityClassMapping<DynamicFormElementRuleCondition>
	{
		public DynamicFormElementRuleConditionMap()
		{
			Schema("CRM");
			Table("DynamicFormElementRuleCondition");

			Id(x => x.Id, map =>
			{
				map.Column("DynamicFormElementRuleConditionId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DynamicFormElementId);
			Property(x => x.DynamicFormElementRuleId);
			Property(x => x.Filter);
			Property(x => x.Value);
			
			ManyToOne(x => x.DynamicFormElement, m =>
			{
				m.Column("DynamicFormElementId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
			ManyToOne(x => x.DynamicFormElementRule, m =>
			{
				m.Column("DynamicFormElementRuleId");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}
