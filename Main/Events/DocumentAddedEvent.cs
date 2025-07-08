namespace Crm.Events
{
	using System;

	using Crm.Library.Modularization.Events;
	using Crm.Model.Enums;

	public class DocumentAddedEvent : IEvent
	{
		public string FileName { get; set; }
		public Guid ReferenceKey { get; set; }
		public ReferenceType ReferenceType { get; set; }
		public string Username { get; set; }
	}
}