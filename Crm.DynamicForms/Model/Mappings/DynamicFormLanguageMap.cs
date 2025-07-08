namespace Crm.DynamicForms.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class DynamicFormLanguageMap : EntityClassMapping<DynamicFormLanguage>
	{
		public DynamicFormLanguageMap()
		{
			Schema("CRM");
			Table("DynamicFormLanguage");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("DynamicFormLanguageId");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.DynamicFormKey);
			Property(x => x.LanguageKey, m => m.Column("Language"));
			Property(x => x.StatusKey, m => m.Column("Status"));
			
			ManyToOne(x => x.DynamicForm, map =>
			{
				map.Column("DynamicFormKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			Property(x => x.FileResourceId);

		}
	}
}