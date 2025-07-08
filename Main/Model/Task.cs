namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;

	public class Task : EntityBase<Guid>, IExportable, ISoftDelete, IEntityWithContactId
	{
		public virtual bool IsCompleted { get; set; }
		public virtual bool IsExported { get; set; }
		public virtual string LegacyId { get; set; }
		public virtual Note Note { get; set; }
		public virtual Guid? NoteId { get; set; }
		public virtual string Text { get; set; }
		public virtual string TypeKey { get; set; }
		public virtual DateTime? DueDate { get; set; }
		public virtual DateTime? DueTime { get; set; }
		public virtual string ResponsibleUser { get; set; }
		public virtual User ResponsibleUserObject { get; set; }
		public virtual Usergroup ResponsibleGroup { get;set; }
		public virtual Guid? ResponsibleGroupKey { get; set; }

		public virtual Contact Contact { get; set; }
		public virtual Guid? ContactId { get; set; }
		public virtual string TaskCreateUser{ get; set; }
		
		public virtual TaskType Type
		{
			get { return TypeKey != null ? LookupManager.Get<TaskType>(TypeKey) : null; }
		}

		public virtual bool IsAssigned
		{
			get { return CreateUser != ResponsibleUser; }
		}

		// Methods
		public override bool Equals(object obj)
		{
			var other = obj as Task;
			if (other == null)
			{
				return false;
			}

			return Id == other.Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		// Constructor
		public Task()
		{
			TypeKey = TaskType.Default.Key;
			IsActive = true;
		}
	}
}