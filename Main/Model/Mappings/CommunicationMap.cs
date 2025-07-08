namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class 
		CommunicationMap : EntityClassMapping<Communication>
	{
		public CommunicationMap()
		{
			Schema("CRM");
			Table("Communication");
			Discriminator(m => m.Column("GroupKey"));

			Id(x => x.Id, m => {
				m.Column("CommunicationId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});
			Property(x => x.LegacyId);
			Property(x => x.TypeKey);
			Property(x => x.ContactId, m => m.Column("ContactKey"));
			Property(x => x.AddressId, m => m.Column("AddressKey"));
			Property(x => x.Data);
			Property(x => x.DataOnlyNumbers, map =>
      {
				map.Insert(false);
				map.Update(false);
		  });
			Property(x => x.Comment);
			Property(x => x.CallingCode);
			Property(x => x.CountryKey);
			Property(x => x.AreaCode);

			ManyToOne(x => x.Contact, map =>
			{
				map.Column("ContactKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});

			ManyToOne(x => x.Address, map =>
			{
				map.Column("AddressKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
		}

		public class PhoneMap : SubclassMapping<Phone>
		{
			public PhoneMap()
			{
				DiscriminatorValue(typeof(Phone).Name);
			}
		}

		public class EmailMap : SubclassMapping<Email>
		{
			public EmailMap()
			{
				DiscriminatorValue(typeof(Email).Name);
			}
		}

		public class FaxMap : SubclassMapping<Fax>
		{
			public FaxMap()
			{
				DiscriminatorValue(typeof(Fax).Name);
			}
		}

		public class WebsiteMap : SubclassMapping<Website>
		{
			public WebsiteMap()
			{
				DiscriminatorValue(typeof(Website).Name);
			}
		}
	}
}
