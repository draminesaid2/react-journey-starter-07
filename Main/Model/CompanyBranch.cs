namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model.Lookups;

	public class CompanyBranch : EntityBase<Guid>, ISoftDelete
	{
		public virtual Guid CompanyKey { get; set; }
		public virtual string Branch1Key { get; set; }
		public virtual Branch1 Branch1 { get; set; }
		public virtual string Branch2Key { get; set; }
		public virtual Branch2 Branch2 { get; set; }
		public virtual string Branch3Key { get; set; }
		public virtual Branch3 Branch3 { get; set; }
		public virtual string Branch4Key { get; set; }
		public virtual Branch4 Branch4 { get; set; }
	}
}