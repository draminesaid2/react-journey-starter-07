using Crm.Library.BaseModel.Mappings;
using NHibernate.Mapping.ByCode;
using System;

namespace Crm.DynamicForms.Model.Mappings
{
	public class DynamicFormFileResponseMap : EntityClassMapping<DynamicFormFileResponse>
	{
		public DynamicFormFileResponseMap()
		{
			Schema("CRM");
			Table("DynamicFormFileResponse");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("DynamicFormFileResponseId");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.DynamicFormReferenceKey);
			ManyToOne(x => x.DynamicFormReference, map =>
			{
				map.Column("DynamicFormReferenceKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			Property(x => x.LanguageKey, m => m.Column("Language"));

			Property(x => x.FileResourceId);
		}
	}
}