namespace Crm.Events
{
	using Crm.Library.Modularization.Events;
	using Crm.Model;

	public class TaskCompletedEvent : IEvent
	{
		public Task Task { get; protected set; }

		public TaskCompletedEvent(Task task)
		{
			Task = task;
		}
	}
}