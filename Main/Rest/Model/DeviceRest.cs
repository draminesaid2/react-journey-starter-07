namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Rest;
	using Crm.Model;

	[RestTypeFor(DomainType = typeof(Device))]
	public class DeviceRest : RestEntity
	{
		public Guid Id { get; set; }
		public string Fingerprint { get; set; }
		public string Token { get; set; }
		public string Username { get; set; }
		public string DeviceInfo { get; set; }
		public bool IsTrusted { get; set; }
	}
}
