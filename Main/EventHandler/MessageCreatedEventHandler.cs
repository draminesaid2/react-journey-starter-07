namespace Crm.EventHandler
{
	using Crm.BackgroundServices;
	using Crm.Library.Modularization.Events;
	using Crm.Model;

	using Quartz;

	public class MessageCreatedEventHandler : IEventHandler<EntityCreatedEvent<Message>>, ISeparateTransactionEventHandler
	{
		private readonly IScheduler scheduler;
		public MessageCreatedEventHandler(IScheduler scheduler)
		{
			this.scheduler = scheduler;
		}
		public virtual void Handle(EntityCreatedEvent<Message> e)
		{
			EmailAgent.Trigger(scheduler);
		}
	}
}
