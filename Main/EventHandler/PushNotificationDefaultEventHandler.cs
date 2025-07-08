namespace Crm.EventHandler
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class PushNotificationDefaultEventHandler : 
		IEventHandler<EntityModifiedEvent<Contact>>,
		IEventHandler<EntityCreatedEvent<UserNote>>,
		IEventHandler<EntityModifiedEvent<UserNote>>
	{
		
		private readonly IPushNotificationService pushNotificationService;
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<UserSubscription, Guid> userSubscriptionRepository;
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		private readonly IAppSettingsProvider appSettingsProvider;

		public PushNotificationDefaultEventHandler(IPushNotificationService pushNotificationService, IUserService userService, IRepositoryWithTypedId<UserSubscription, Guid> userSubscriptionRepository, IRepositoryWithTypedId<Contact, Guid> contactRepository, IAppSettingsProvider appSettingsProvider)
		{
			this.pushNotificationService = pushNotificationService;
			this.userService = userService;
			this.userSubscriptionRepository = userSubscriptionRepository;
			this.contactRepository = contactRepository;
			this.appSettingsProvider = appSettingsProvider;
		}
		public virtual void Handle(EntityModifiedEvent<Contact> e)
		{
			if (!IsEnabled())
			{
				return;
			}
			var notification = new PushNotification
			{
				Usernames = GetSubscribedUsernamesToContact(e.Entity.Id, e.Entity.ModifyUser),
				TitleResourceKey = "ContactModifiedNotificationTitle",
				TitleResourceParams = new List<string> {e.Entity.ContactType, e.Entity.Name },
				BodyResourceKey = "EntityModifiedNotificationBody",
				BodyResourceParams = new List<string> {e.Entity.ContactType, userService.GetDisplayName(e.Entity.ModifyUser) },
				Url = pushNotificationService.GetUrlForContact(e.Entity)
			};
			pushNotificationService.SendPushNotification(notification);
		}
		
		public virtual void Handle(EntityCreatedEvent<UserNote> e)
		{
			if (!IsEnabled())
			{
				return;
			}
			
			if (e.Entity.ContactId == null)
				return;

			var contact = contactRepository.Get(e.Entity.ContactId.Value);

			var notification = new PushNotification
			{
				Usernames = GetSubscribedUsernamesToContact(e.Entity.ContactId.Value, e.Entity.CreateUser),
				TitleResourceKey = "ContactModifiedNotificationTitle",
				TitleResourceParams = new List<string> {contact.ContactType, contact.Name },
				BodyResourceKey = "EntityCreatedNotificationBody",
				BodyResourceParams = new List<string> {"Note", userService.GetDisplayName(e.Entity.CreateUser) },
				Url = pushNotificationService.GetUrlForContact(contact)
			};
			pushNotificationService.SendPushNotification(notification);
		}
		
		public virtual void Handle(EntityModifiedEvent<UserNote> e)
		{
			if (!IsEnabled())
			{
				return;
			}
			
			if (e.Entity.ContactId == null)
				return;

			var contact = contactRepository.Get(e.Entity.ContactId.Value);

			var notification = new PushNotification
			{
				Usernames = GetSubscribedUsernamesToContact(e.Entity.ContactId.Value, e.Entity.ModifyUser),
				TitleResourceKey = "ContactModifiedNotificationTitle",
				TitleResourceParams = new List<string> {contact.ContactType, contact.Name },
				BodyResourceKey = "EntityModifiedNotificationBody",
				BodyResourceParams = new List<string> {"Note", userService.GetDisplayName(e.Entity.ModifyUser) },
				Url = pushNotificationService.GetUrlForContact(contact)
			};
			pushNotificationService.SendPushNotification(notification);
		}

		protected virtual IEnumerable<string> GetSubscribedUsernamesToContact(Guid contactKey, string modifyUsername)
		{
			return userSubscriptionRepository.GetAll()
				.Where(x => x.EntityKey == contactKey && x.IsSubscribed)
				.Where(x => x.Username != modifyUsername)
				.Select(x => x.Username)
				.ToList();
			;
		}

		protected virtual bool IsEnabled()
		{
			return appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.Enabled) && !string.IsNullOrWhiteSpace(appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.FCMServerKey));
		}
	}
}
