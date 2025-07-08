namespace Crm.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using System;
	using System.Collections.Generic;

	public class FileResource : EntityBase<Guid>, ISoftDelete
	{
		// Properties
		public virtual Guid? ParentId { get; set; }
		public virtual string Filename { get; set; }
		public virtual string Path { get; set; }
		public virtual string ContentType { get; set; }
		public virtual long Length { get; set; }
		public virtual bool OfflineRelevant { get; set; }
		public virtual byte[] Content { get; set; }
		public virtual ICollection<DocumentAttribute> DocumentAttributes { get; set; } = new List<DocumentAttribute>();


		// Methods
		public override int GetHashCode()
		{
			unchecked
			{
				return (Id.GetHashCode() * 397) ^ (Filename != null ? Filename.GetHashCode() : 0);
			}
		}
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(FileResource)) return false;
			return Equals((FileResource)obj);
		}
		public virtual bool Equals(FileResource obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj.Id, Id);
		}
	}
}
