namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class TaskMap : EntityClassMapping<Task>
	{
		public TaskMap()
		{
			Schema("CRM");
			Table("Task");
			Discriminator(x => x.Column("TaskType"));
			DiscriminatorValue("Task");

			Id(x => x.Id,
				m =>
					{
						m.Column("TaskId");
						m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
						m.UnsavedValue(Guid.Empty);
					});

			Property(x => x.IsCompleted);
			Property(x => x.LegacyId);
			Property(x => x.NoteId, m => m.Column("NoteKey"));
			Property(x => x.Text);
			Property(x => x.TypeKey, m => m.Column("TaskTypeKey"));
			Property(x => x.DueDate);
			Property(x => x.DueTime);
			Property(x => x.ResponsibleUser);
			Property(x => x.ResponsibleGroupKey, map => map.Column("ResponsibleGroup"));
			Property(x => x.TaskCreateUser);
			Property(x => x.ContactId,
				m =>
					{
						m.Column("ContactKey");
						m.NotNullable(false);
					});

			ManyToOne(x => x.ResponsibleUserObject, map =>
			{
				map.Column("ResponsibleUser");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			ManyToOne(x => x.ResponsibleGroup, map =>
			{
				map.Column("ResponsibleGroup");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			ManyToOne(x => x.Contact, map =>
			{
				map.Column("ContactKey");

				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			ManyToOne(x => x.Note, map =>
			{
				map.Column("NoteKey");

				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
			ManyToOne(x => x.ResponsibleGroup, map =>
			{
				map.Column("ResponsibleGroup");

				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Update(false);
				map.Insert(false);
			});
		}
	}
}