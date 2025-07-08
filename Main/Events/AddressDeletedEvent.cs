namespace Crm.Events
{
	using Crm.Library.Modularization.Events;
	using Crm.Model;

	public class AddressDeletedEvent : IEvent
	{
		public Address Address { get; set; }

		// Constructor
		public AddressDeletedEvent(Address address)
		{
			Address = address;
		}
	}
}