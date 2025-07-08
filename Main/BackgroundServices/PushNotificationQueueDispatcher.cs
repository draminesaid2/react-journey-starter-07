namespace Crm.BackgroundServices
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using Autofac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Services.Interfaces;

	using Geocoding;
	using log4net;
	using Microsoft.Extensions.Hosting;
	using Quartz;
	
	[DisallowConcurrentExecution]
	public class PushNotificationQueueDispatcher : JobBase
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly ILog logger;
		private readonly VirtualRequestHandler virtualRequestHandler;
		private readonly IPushNotificationService pushNotificationService;
		private readonly IResourceManager resourceManager;
		private readonly HttpClient httpClient;

		private const string JobGroup = nameof(PushNotificationQueueDispatcher);
		private const string JobName = nameof(PushNotificationQueueDispatcher);

		public PushNotificationQueueDispatcher(IAppSettingsProvider appSettingsProvider, IPushNotificationService pushNotificationService, IResourceManager resourceManager, ISessionProvider sessionProvider, ILog logger, IHostApplicationLifetime hostApplicationLifetime, VirtualRequestHandler virtualRequestHandler)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.appSettingsProvider = appSettingsProvider;
			this.pushNotificationService = pushNotificationService;
			this.resourceManager = resourceManager;
			this.logger = logger;
			this.virtualRequestHandler = virtualRequestHandler;
			httpClient = new HttpClient();
		}

		protected override void Run(IJobExecutionContext context)
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			if (!appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.Enabled))
			{
				return;
			}

			if (appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.FCMServerKey).IsNullOrWhiteSpace())
			{
				logger.Error("FCMServerKey is missing. Please check configuration.");
				return;
			}

			while (pushNotificationService.QueueHasElement())
			{
				var pushNotification = pushNotificationService.QueueNext();
				SendPushNotification(pushNotification);
			}
		}

		protected virtual void SendPushNotification(PushNotification pushNotification)
		{
			var (mobileTokens, webTokens) = GetTokensForLanguages(pushNotification.Usernames);
			var notificationObjects = new List<object>();

			if(pushNotification.Data == null) {
				foreach (var language in webTokens.Keys)
				{
					notificationObjects.Add(
						new
						{
							registration_ids = webTokens[language].ToArray(),
							ttl = "5s",
							data = new
							{
								title = GetTranslatedResource(pushNotification.TitleResourceKey, language, pushNotification.TitleResourceParams),
								body = GetTranslatedResource(pushNotification.BodyResourceKey, language, pushNotification.BodyResourceParams),
								url = pushNotification.Url
							}
						}
					);
				}

				foreach (var language in mobileTokens.Keys)
				{
					var title = GetTranslatedResource(pushNotification.TitleResourceKey, language, pushNotification.TitleResourceParams);
					var body = GetTranslatedResource(pushNotification.BodyResourceKey, language, pushNotification.BodyResourceParams);
					notificationObjects.Add(
						new
						{
							registration_ids = mobileTokens[language].ToArray(),
							ttl = "5s",
							priority = 10,
							android = new
							{
								priority = "high"
							},
							notification = new
							{
								title,
								body,
								priority = "high",
								sound = "default"
							},
							data = new
							{
								title,
								body,
								url = pushNotification.Url
							}
						}
					);
				}
			}
			else
			{
				foreach (var language in mobileTokens.Keys)
				{
					notificationObjects.Add(
						new
						{
							registration_ids = mobileTokens[language].ToArray(),
							time_to_live = 10,
							priority = "high",
							content_available = true,
							android = new
							{
								priority = "high"
							},
							data = pushNotification.Data
						}
					);
				}
			}

			var serverKey = appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.FCMServerKey);
			foreach (var notificationObject in notificationObjects)
			{
				PostNotification(notificationObject, serverKey);
			}
		}

		protected virtual (Dictionary<string, HashSet<string>> mobileTokens, Dictionary<string, HashSet<string>> webTokens) GetTokensForLanguages(IEnumerable<string> usernames)
		{
			var mobileTokens = new Dictionary<string, HashSet<string>>();
			var webTokens = new Dictionary<string, HashSet<string>>();

			virtualRequestHandler.BeginRequest();
			var users = virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<User>>()
				.GetAll().Where(x => usernames.Contains(x.Id));
			var devices = virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Device>>()
				.GetAll()
				.Where(x => x.IsTrusted)
				.Where(x => x.Token != null);

			foreach (var user in users)
			{
				var desktops = devices
					.Where(x => x.Username == user.Id)
					.Where(x => x.DeviceInfo.Contains("Win"))
					.OrderByDescending(x => x.ModifyDate);
				var mobiles = devices
					.Where(x => x.Username == user.Id)
					.Where(x => !x.DeviceInfo.Contains("Win"))
					.OrderByDescending(x => x.ModifyDate);

				foreach(var device in desktops)
				{
					AddTokenToLanguage(user.DefaultLanguageKey, device.Token, webTokens);
				}

				foreach(var device in mobiles)
				{
					AddTokenToLanguage(user.DefaultLanguageKey, device.Token, mobileTokens);
				}
			}

			virtualRequestHandler.EndRequest();

			return (mobileTokens, webTokens);
		}

		protected virtual void AddTokenToLanguage(string languageKey, string token, IDictionary<string, HashSet<string>> dict)
		{
			if (dict.ContainsKey(languageKey))
			{
				dict[languageKey].Add(token);
			}
			else
			{
				dict.Add(languageKey, new HashSet<string> { token });
			}
		}

		protected virtual string GetTranslatedResource(string resourceKey, string language, List<string> parameters = null)
		{
			if (parameters == null)
			{
				return resourceManager.GetTranslation(resourceKey, CultureInfo.GetCultureInfo(language));
			}
			var translatedParams = TranslateParamsIfPossible(parameters, language);
			return resourceManager.GetTranslation(resourceKey, CultureInfo.GetCultureInfo(language))?.WithArgs(translatedParams);
		}

		protected virtual string[] TranslateParamsIfPossible(List<string> parameters, string language)
		{
			var result = new List<string>();
			foreach (var param in parameters)
			{
				var translation = resourceManager.GetTranslation(param, CultureInfo.GetCultureInfo(language));
				result.Add(translation.IsNotNull() ? translation : param);
			}
			return result.ToArray();
		}

		protected virtual void PostNotification(object notification, string serverKey)
		{
			try
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", $"={serverKey}");
				var result = httpClient.PostAsync("https://fcm.googleapis.com/fcm/send", new StringContent(notification.ToJSON(), Encoding.UTF8, "application/json")).Result;
				
				if (!result.IsSuccessStatusCode)
				{
					logger.Warn($"Request returned status code {result.StatusCode}");
				}
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
			}
		}

		public static void Trigger(IScheduler scheduler)
		{
			var alreadyTriggered = scheduler.GetTriggersOfJob(new JobKey(JobName, JobGroup)).Result.OfType<ISimpleTrigger>().Any(x => scheduler.GetTriggerState(x.Key).Result != TriggerState.Complete);
			if (alreadyTriggered) return;

			var trigger = TriggerBuilder
				.Create()
				.ForJob(JobName, JobGroup)
				.StartNow()
				.Build();
			scheduler.ScheduleJob(trigger);
		}
	}
}
