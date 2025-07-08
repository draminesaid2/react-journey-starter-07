namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class DynamicFormResponseMap : EntityClassMapping<DynamicFormResponse>
	{
		public DynamicFormResponseMap()
		{
			Schema("CRM");
			Table("DynamicFormResponse");

			Id(x => x.Id, map => {
				map.Column("DynamicFormResponseId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.DynamicFormReferenceKey);
			Property(x => x.DynamicFormElementKey);
			Property(x => x.DynamicFormElementType);
			Property(x => x.ValueAsString, m =>
				{
					m.Column("Value");
					m.Length(Int32.MaxValue);
				});

			ManyToOne(x => x.DynamicFormElement,
				m =>
				{
					m.Column("DynamicFormElementKey");
					m.Fetch(FetchKind.Join);
					m.Insert(false);
					m.Update(false);
				});
		}
	}
}