namespace Crm.Services
{
	using System.Linq;

	using Crm.BackgroundServices;
	using Crm.Model;
	using Crm.Services.Interfaces;

	using Quartz;
	
	public class PushNotificationService : IPushNotificationService
	{
		private readonly IScheduler scheduler;
		private readonly IPushNotificationCache pushNotificationCache;

		public PushNotificationService(IScheduler scheduler, IPushNotificationCache pushNotificationCache)
		{
			this.scheduler = scheduler;
			this.pushNotificationCache = pushNotificationCache;
		}

		public virtual void SendPushNotification(PushNotification notification)
		{
			pushNotificationCache.Add(notification);
			PushNotificationQueueDispatcher.Trigger(scheduler);
		}
		public virtual bool QueueHasElement()
		{
			return pushNotificationCache.GetPushNotifications().Any();
		}
		public virtual PushNotification QueueNext()
		{
			if (!QueueHasElement())
				return null;
			return pushNotificationCache.PopFirst();
		}
		
		public virtual string GetUrlForContact(Contact contact)
		{
			var pluginName = contact.AssemblyQualifiedName.Split(',')[1].Trim();
			return $"~/Home/MaterialIndex#/{pluginName}/{contact.ContactType}/DetailsTemplate/{contact.Id}";
		}
	}
}
