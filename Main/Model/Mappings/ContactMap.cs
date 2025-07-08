namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Data.Domain;

	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode;

	public class ContactMap : EntityClassMapping<Contact>
	{
		public ContactMap()
		{
			Schema("CRM");
			Table("Contact");
			Discriminator(x => x.Column("ContactType"));

			Id(x => x.Id, map =>
			{
				map.Column("ContactId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Name);
			Property(x => x.ContactType, map =>
			{
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.LegacyId);
			Property(x => x.LegacyName, map =>
			{
				map.Update(false);
				map.Insert(false);
			});
			Property(x => x.LegacyVersion, map =>
			{
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.ParentId, map => map.Column("ParentKey"));
			Property(x => x.ResponsibleUser);
			Property(x => x.BackgroundInfo);
			Property(x => x.SourceTypeKey, m => m.Column("Source"));
			Property(x => x.Visibility);
			Property(x => x.CampaignSource, m => m.Column("SourceKey"));
			Property(x => x.InactiveDate);
			Property(x => x.InactiveUser);
			Property(x => x.LanguageKey, m => m.Column("ContactLanguage"));

			this.EntitySet(x => x.Addresses,
				m =>
				{
					m.Key(km =>
					{
						km.Column("CompanyKey");
						km.NotNullable(true);
					});
					m.Inverse(true);
					m.Cascade(Cascade.None);
					m.BatchSize(100);
				},
				a => a.OneToMany()
			);
			this.EntitySet(x => x.DocumentAttributes,
				m =>
				{
					m.Key(km =>
					{
						km.Column("ReferenceKey");
						km.NotNullable(true);
					});
					m.Inverse(true);
					m.Fetch(CollectionFetchMode.Select);
					m.Lazy(CollectionLazy.Lazy);
					m.Cascade(Cascade.Persist);
					m.BatchSize(100);
				},
				a => a.OneToMany()
			);
			this.EntitySet(x => x.Staff, m =>
			{
				m.Schema("CRM");
				m.Table("Contact");
				m.Key(km => km.Column("ParentKey"));
				m.Where("ContactType = 'Person'");
				m.Inverse(true);
				m.Cascade(Cascade.None);
				m.BatchSize(100);
			}, action => action.OneToMany());

			Set(x => x.VisibleToUserIds, map =>
			{
				map.Schema("CRM");
				map.Table("ContactUser");
				map.Key(km => km.Column("ContactKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(100);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
				map.BatchSize(100);
			}, r => r.Element(m => m.Column("Username")));

			Set(x => x.VisibleToUsergroupIds, map =>
			{
				map.Schema("CRM");
				map.Table("ContactUserGroup");
				map.Key(km => km.Column("ContactKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.BatchSize(100);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist);
				map.BatchSize(100);
			}, r => r.Element(m => m.Column("UserGroupKey")));

			ManyToOne(x => x.Parent,
				m =>
				{
					m.Column("ParentKey");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
					m.Lazy(LazyRelation.Proxy);
				});

			ManyToOne(x => x.ResponsibleUserObject,
				m =>
				{
					m.Column("ResponsibleUser");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			this.EntitySet(x => x.Communications,
				m =>
				{
					m.Key(km =>
					{
						km.Column("ContactKey");
						km.NotNullable(true);
					});
					m.Cascade(Cascade.Persist.Include(Cascade.Remove));
					m.Inverse(true);
					m.BatchSize(100);
				},
				a => a.OneToMany());

			this.EntitySet(x => x.Tags, map =>
			{
				map.Schema("CRM");
				map.Table("ContactTags");
				map.Key(km =>
				{
					km.Column("ContactKey");
					km.NotNullable(true);
					km.Update(true);
				});
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.All);
				map.Inverse(true);
				map.BatchSize(25);
				map.Filter(IsActiveFilter.Name, IsActiveFilter.FilterMapping);
				map.Filter(AuthorisationFilter.Name, AuthorisationFilter.FilterMapping);
			}, m => m.OneToMany());

			this.EntitySet(x => x.Notes, map =>
			{
				map.Key(km =>
				{
					km.Column("ElementKey");
					km.NotNullable(true);
					km.Update(true);
				});
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Persist.Include(Cascade.Remove));
				map.Inverse(true);
			}, action => action.OneToMany());

			this.EntitySet(x => x.Bravos, m =>
			{
				m.Schema("CRM");
				m.Table("Bravo");
				m.Key(km => km.Column("CompanyKey"));
				m.Inverse(true);
			}, action => action.OneToMany());

			Set(x => x.UserRecentPages, map =>
			{
				map.Table("UserRecentPages");
				map.Key(km => km.Column("ContactId"));
				map.Lazy(CollectionLazy.Lazy);
				map.Inverse(true);
				map.Fetch(CollectionFetchMode.Select);
				map.Cascade(Cascade.None);
			}, action => action.OneToMany());
		}
	}
}