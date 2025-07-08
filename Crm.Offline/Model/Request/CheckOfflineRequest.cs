namespace Crm.Offline.Model.Request
{
	using System;
	using System.Collections.Generic;

	public class CheckOfflineRequest
	{
		public CheckOfflineRequestDefinition[] Definitions { get; set; }
		public IDictionary<string, int?> ReplicationGroupSettings { get; set; }
	}

	public class CheckOfflineRequestDefinition
	{
		public string Plugin { get; set; }
		public string Model { get; set; }
		public Guid ClientId { get; set; }
		public int Index { get; set; }
	}
}
