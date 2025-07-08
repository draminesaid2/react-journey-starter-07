namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain;

	public class Tag : EntityBase<Guid>, IComparable, ISoftDelete, INoAuthorisedObject
	{
		[DomainSignature]
		public virtual string Name { get; set; }
		public virtual Contact Contact { get; set; }

		public virtual Guid ContactKey { get; set; }
		public virtual int CompareTo(object obj)
		{
			var other = obj as Tag;
			if (other == null)
				return -1;

			return String.CompareOrdinal(Name, other.Name);
		}
		public override string ToString()
		{
			return Name;
		}
	}
}
