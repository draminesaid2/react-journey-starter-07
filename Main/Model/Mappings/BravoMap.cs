namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class BravoMap : EntityClassMapping<Bravo>
	{
		public BravoMap()
		{
			Schema("CRM");
			Table("Bravo");

			Id(x => x.Id, m =>
				{
					m.Column("BravoId");
					m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					m.UnsavedValue(Guid.Empty);
				});

			Property(x => x.CategoryKey, m => m.Column("BravoCategoryTypeKey"));
			Property(x => x.ContactId, m => m.Column("CompanyKey"));
			Property(x => x.Issue);
			Property(x => x.FinishedByUser);
			Property(x => x.IsOnlyVisibleForCreateUser);
			Property(x => x.IsEnabled);
			
			ManyToOne(x => x.Contact, map =>
			{
				map.Column("CompanyKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}
	}
}