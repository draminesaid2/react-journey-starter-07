namespace Crm.Model
{
	using System;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class Device : EntityBase<Guid>, ISoftDelete
	{
		public virtual string Fingerprint { get; set; }
		public virtual string Token { get; set; }
		public virtual string Username { get; set; }
		public virtual string DeviceInfo { get; set; }
		public virtual bool IsTrusted { get; set; }
	}
}
