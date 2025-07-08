namespace Crm.Model.Mappings.Notes
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Model.Notes;

	using NHibernate.Mapping.ByCode;

	public class NoteMap : EntityClassMapping<Note>
	{
		public NoteMap()
		{
			Schema("CRM");
			Table("Note");
			Discriminator(m => m.Column("NoteType"));

			Id(x => x.Id, map =>
			{
				map.Column("NoteId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.LegacyId);
			Property(x => x.NoteType, map =>
			{
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.ContactId, map => map.Column("ElementKey"));
			Property(x => x.Subject);
			Property(x => x.Text, m => m.Length(Int16.MaxValue));
			Property(x => x.Meta);
			Property(x => x.Plugin);
			Property(x => x.Title);
			Property(x => x.Link);
			Property(x => x.EntityId);
			Property(x => x.EntityName);
			Property(x => x.IsSystemGenerated);

			this.EntitySet(x => x.Files, map =>
			{
				map.Key(km => km.Column("ElementKey"));
				map.Inverse(true);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.None);
				map.BatchSize(100);
			}, action => action.OneToMany());

			this.EntitySet(x => x.Links, map =>
			{
				map.Key(km => km.Column("ElementKey"));
				map.Inverse(true);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.None);
				map.BatchSize(100);
			}, action => action.OneToMany());

			ManyToOne(x => x.Contact, map =>
			{
				map.Column("ElementKey");

				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			ManyToOne(x => x.CreateUserObject, map =>
			{
				map.Column("CreateUser");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
		}
	}
}