namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Model.Bravo))]
	public class BravoRest : RestEntityWithExtensionValues
	{
		public virtual Guid ContactId { get; set; }
		public virtual string Issue { get; set; }
		public virtual string CategoryKey { get; set; }
		public virtual string FinishedByUser { get; set; }
		public virtual bool IsOnlyVisibleForCreateUser { get; set; }
		public bool IsEnabled { get; set; }
	}
}
