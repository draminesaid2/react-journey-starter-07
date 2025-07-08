namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class AddressMap : EntityClassMapping<Address>
	{
		public AddressMap()
		{
			Schema("CRM");
			Table("Address");

			Id(a => a.Id, m =>
				{
					m.Column("AddressId");
					m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					m.UnsavedValue(Guid.Empty);
				});

			Property(a => a.LegacyId);
			Property(a => a.Name1);
			Property(a => a.Name2);
			Property(a => a.Name3);
			Property(a => a.Street);
			Property(a => a.City);
			Property(a => a.ZipCode);
			Property(a => a.ZipCodePOBox);
			Property(a => a.POBox);
			Property(a => a.CountryKey);
			Property(a => a.RegionKey);
			Property(a => a.AddressTypeKey);
			Property(a => a.LanguageKey);
			Property(a => a.Latitude);
			Property(a => a.Longitude);
			Property(a => a.GeocodingRetryCounter);
			Property(a => a.CompanyId, m => m.Column("CompanyKey"));
			Property(a => a.IsCompanyStandardAddress);

			Property(a => a.AddressString, m =>
				{
					m.Formula("COALESCE(Name1 + ', ','') + COALESCE(Street + ', ','') + COALESCE(ZipCode,'') + ' ' + COALESCE(City,'')");
					m.Insert(false);
					m.Update(false);
				});

			ManyToOne(x => x.Contact, map =>
			{
				map.Column("CompanyKey");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			this.EntitySet(x => x.Emails, m =>
			{
				m.Key(km =>
				{
					km.Column("AddressKey");
					km.NotNullable(true);
				});
				m.Lazy(CollectionLazy.Lazy);
				m.Cascade(Cascade.None);
				m.Inverse(true);
				m.BatchSize(100);
				m.Where($"GroupKey = '{nameof(Email)}'");
				m.Fetch(CollectionFetchMode.Select);
			}, a => a.OneToMany());
			this.EntitySet(x => x.Faxes, m =>
			{
				m.Key(km =>
				{
					km.Column("AddressKey");
					km.NotNullable(true);
				});
				m.Lazy(CollectionLazy.Lazy);
				m.Cascade(Cascade.None);
				m.Inverse(true);
				m.BatchSize(100);
				m.Where($"GroupKey = '{nameof(Fax)}'");
				m.Fetch(CollectionFetchMode.Select);
			}, a => a.OneToMany());
			this.EntitySet(x => x.Phones, m =>
			{
				m.Key(km =>
				{
					km.Column("AddressKey");
					km.NotNullable(true);
				});
				m.Lazy(CollectionLazy.Lazy);
				m.Cascade(Cascade.None);
				m.Inverse(true);
				m.BatchSize(100);
				m.Where($"GroupKey = '{nameof(Phone)}'");
				m.Fetch(CollectionFetchMode.Select);
			}, a => a.OneToMany());
			this.EntitySet(x => x.Websites, m =>
			{
				m.Key(km =>
				{
					km.Column("AddressKey");
					km.NotNullable(true);
				});
				m.Lazy(CollectionLazy.Lazy);
				m.Cascade(Cascade.None);
				m.Inverse(true);
				m.BatchSize(100);
				m.Where($"GroupKey = '{nameof(Website)}'");
				m.Fetch(CollectionFetchMode.Select);
			}, a => a.OneToMany());
		}
	}
}