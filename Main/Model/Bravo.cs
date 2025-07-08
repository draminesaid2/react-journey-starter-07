namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model.Lookups;

	public class Bravo : EntityBase<Guid>, ISoftDelete
	{
		public virtual Guid ContactId { get; set; }
		public virtual Contact Contact { get; set; }
		public virtual string Issue { get; set; }
		public virtual string CategoryKey { get; set; }
		public virtual string FinishedByUser { get; set; }
		public virtual bool IsOnlyVisibleForCreateUser { get; set; }
		public virtual bool IsEnabled { get; set; }

		public virtual BravoCategory Category
		{
			get { return CategoryKey != null ? LookupManager.Get<BravoCategory>(CategoryKey) : null; }
		}

		public Bravo()
		{
			IsEnabled = true;
			CategoryKey = BravoCategory.None.Key;
		}
	}
}
