namespace Crm.Model.Mappings
{
	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class PersonMap : SubclassMapping<Person>
	{
		public PersonMap()
		{
			DiscriminatorValue("Person");

			Join("Person", join =>
				{
					join.Schema("CRM");
					join.Key(x => x.Column("ContactKey"));
					join.Property(x => x.PersonNo);
					join.Property(x => x.Firstname);
					join.Property(x => x.Surname);
					join.Property(x => x.DepartmentTypeKey, m => m.Column("Department"));
					join.Property(x => x.BusinessTitleKey, m => m.Column("BusinessTitle"));
					join.Property(x => x.TitleKey);
					join.Property(x => x.SalutationKey);
					join.Property(x => x.SalutationLetterKey, m => m.Column("LetterSalutationKey"));
					join.Property(x => x.Mima, m => m.Column("IsMima"));
					join.Property(x => x.IsRetired);
					join.Property(x => x.StandardAddressKey, m =>
						{
							m.Column("AddressKey");
							m.Insert(false);
							m.Update(false);
						});

					join.ManyToOne(x => x.Address,
						m =>
							{
								m.Column("AddressKey");
								m.NotNullable(true);
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
				});
		}
	}
}