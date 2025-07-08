namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Model.Relationships;

	using NHibernate.Mapping.ByCode;
	public class CompanyPersonRelationshipMap : EntityClassMapping<CompanyPersonRelationship>
	{
		public CompanyPersonRelationshipMap()
		{
			Schema("CRM");
			Table("CompanyPersonRelationship");

			Id(x => x.Id, m =>
			{
				m.Column("CompanyPersonRelationshipId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.RelationshipTypeKey, m => m.Column("RelationshipType"));
			Property(x => x.ParentId, m => m.Column("CompanyKey"));
			ManyToOne(x => x.Parent, m =>
			{
				m.Column("CompanyKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.ChildId, m => m.Column("PersonKey"));
			ManyToOne(x => x.Child, m =>
			{
				m.Column("PersonKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.Information);

		}

	}
}
