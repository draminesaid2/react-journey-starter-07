namespace Crm.Services.Interfaces
{
	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface IPushNotificationService : ISingletonDependency
	{
		void SendPushNotification(PushNotification notification);
		bool QueueHasElement();
		PushNotification QueueNext();
		string GetUrlForContact(Contact contact);
	}
}