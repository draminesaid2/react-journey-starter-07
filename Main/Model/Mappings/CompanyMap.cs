namespace Crm.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class CompanyMap : SubclassMapping<Company>
	{
		public CompanyMap()
		{
			DiscriminatorValue("Company");

			ManyToOne(x => x.StandardAddress,
				m =>
				{
					m.Formula("(SELECT TOP 1 a.AddressId FROM Crm.Address a WHERE a.CompanyKey = ContactId AND a.IsCompanyStandardAddress = 1 AND a.IsActive = 1)");
					m.Insert(false);
					m.Update(false);
				});
			ManyToOne(x => x.ParentCompany,
				m =>
				{
					m.Column("ParentKey");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});

			Join("Company", join =>
				{
					join.Schema("CRM");
					join.Key(x => x.Column("ContactKey"));

					join.Property(x => x.SalesRepresentative);
					join.Property(x => x.ShortText);
					join.Property(x => x.IsOwnCompany);
					join.Property(x => x.NumberOfEmployeesKey);
					join.Property(x => x.TurnoverKey);
					join.Property(x => x.CompanyGroupFlag1Key, m => m.Column("GroupFlag1Key"));
					join.Property(x => x.CompanyGroupFlag2Key, m => m.Column("GroupFlag2Key"));
					join.Property(x => x.CompanyGroupFlag3Key, m => m.Column("GroupFlag3Key"));
					join.Property(x => x.CompanyGroupFlag4Key, m => m.Column("GroupFlag4Key"));
					join.Property(x => x.CompanyGroupFlag5Key, m => m.Column("GroupFlag5Key"));
					join.Property(x => x.CompanyNo);
					// TODO: Include Company related field from ContactMap but ensure advanced contact search is working
					join.Property(x => x.CompanyTypeKey);
					join.Property(x => x.AreaSalesManager);
					join.Property(x => x.IsEnabled);
					join.ManyToOne(x => x.AreaSalesManagerObject,
						m =>
						{
							m.Column("AreaSalesManager");
							m.Insert(false);
							m.Update(false);
						});
					join.Property(x => x.StationKey);
					join.ManyToOne(x => x.Station, map =>
					{
						map.Column("StationKey");
						map.Fetch(FetchKind.Select);
						map.Lazy(LazyRelation.Proxy);
						map.Cascade(Cascade.None);
						map.Insert(false);
						map.Update(false);
					});

					join.EntityBag(x => x.ClientCompanies, m =>
						{
							m.Schema("CRM");
							m.Table("Contact");
							m.Key(km => km.Column("ParentKey"));
							m.Where("ContactType = 'Company' AND IsActive = 1");
						}, action => action.OneToMany());

					join.EntityBag(x => x.BusinessRelationships, m =>
						{
							m.Schema("CRM");
							m.Table("BusinessRelationship");
							m.Key(km => km.Column("ParentCompanyKey"));
							m.Where("IsActive = 1");
							m.Inverse(true);
						}, action => action.OneToMany());

					join.EntityBag(x => x.CompanyPersonRelationships, m =>
					{
						m.Schema("CRM");
						m.Table("CompanyPersonRelationship");
						m.Key(km => km.Column("CompanyKey"));
						m.Where("IsActive = 1");
						m.Inverse(true);
					}, action => action.OneToMany());

					this.EntitySet(x => x.CompanyBranches,
						m =>
						{
							m.Key(km =>
							{
								km.Column("CompanyKey");
								km.NotNullable(true);
							});
							m.Cascade(Cascade.Persist.Include(Cascade.Remove));
							m.Inverse(true);
						},
						a => a.OneToMany());

				});

		}
	}
}
