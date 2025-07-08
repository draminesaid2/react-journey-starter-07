namespace Crm.Services
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;

	using Microsoft.Extensions.Caching.Distributed;
	
	public interface IPushNotificationCache : ISingletonDependency
	{
		IEnumerable<PushNotification> GetPushNotifications();
		void Add(PushNotification pushNotification);
		void Remove(PushNotification pushNotification);
		PushNotification PopFirst();
	}

	public class PushNotificationCache : Cache<PushNotification>, IPushNotificationCache
	{
		private const string PushNotificationsCacheKey = "PushNotifications";
		
		private readonly IRestSerializer serializer;
		public PushNotificationCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(PushNotificationCache), cache, serializer)
		{
			this.serializer = serializer;
		}
		public virtual IEnumerable<PushNotification> GetPushNotifications()
		{
			return DictGetAll(PushNotificationsCacheKey).Values;
		}
		public virtual void Add(PushNotification pushNotification)
		{
			DictSet(PushNotificationsCacheKey, pushNotification.Id.ToString(), pushNotification);
		}
		public virtual PushNotification PopFirst()
		{
			var pushNotification = GetPushNotifications().FirstOrDefault();
			
			if (pushNotification != null)
			{
				Remove(pushNotification);
			}

			return pushNotification;
		}
		public virtual void Remove(PushNotification pushNotification)
		{
			DictDelete(PushNotificationsCacheKey, pushNotification.Id.ToString());
		}
	}
}
