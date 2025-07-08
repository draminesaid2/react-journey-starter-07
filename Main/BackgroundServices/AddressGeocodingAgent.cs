namespace Crm.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Model;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class AddressGeocodingAgent : ManualSessionHandlingJobBase
	{
		private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
		private readonly IGeocodingService geocodingService;
		private readonly IAppSettingsProvider appSettings;
		private IScheduler scheduler;

		protected override void Run(IJobExecutionContext context)
		{
			// We exceeded our Geocode quota for today, try next day.
			if (receivedShutdownSignal)
			{
				return;
			}
			if (geocodingService.QuotaExceeded)
			{
				return;
			}

			var hasGoogleMapsApiKey = appSettings.GetValue(MainPlugin.Settings.Geocoder.GoogleMapsApiKey).IsNotNullOrEmpty();
			if (geocodingService.GeocoderIsGoogle && !hasGoogleMapsApiKey)
			{
				scheduler.PauseJob(new JobKey("AddressGeocodingAgent", "Core"));
				Logger.Warn("Google geocoder is not able to geocode locations anymore without API Key. See config: Geocoder/GoogleMapsApiKey");
				return;
			}

			var entityWithGeocodes = addressRepository.GetAll().Where(x => x.Latitude == null || x.Longitude == null);
			entityWithGeocodes = entityWithGeocodes.Where(x => x.GeocodingRetryCounter <= 3);
			entityWithGeocodes = entityWithGeocodes.Where(x => x.Contact != null || x.CompanyId == null);
			entityWithGeocodes = entityWithGeocodes.Where(x =>
				x.Street != null && x.Street != String.Empty
				|| x.City != null && x.City != String.Empty
				|| x.ZipCode != null && x.ZipCode != String.Empty
				|| x.CountryKey != null && x.CountryKey != String.Empty);

			var counter = 0;
			var batchSizeConfig = appSettings.GetValue(MainPlugin.Settings.Geocoder.BatchSize);
			var batchSize = batchSizeConfig > 0 ? batchSizeConfig : 50;
			var batch = entityWithGeocodes.Skip(counter).Take(batchSize);
			while (batch.Any())
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				foreach (var entityWithGeocode in batch)
				{
					if (receivedShutdownSignal)
					{
						break;
					}
					if (!geocodingService.TryGeocode(entityWithGeocode))
					{
						continue;
					}

					BeginTransaction();
					addressRepository.SaveOrUpdate(entityWithGeocode);
					EndTransaction();
				}

				counter += batchSize;
				batch = entityWithGeocodes.Skip(counter).Take(batchSize);
			}
		}

		public AddressGeocodingAgent(ISessionProvider sessionProvider, IRepositoryWithTypedId<Address, Guid> addressRepository, IGeocodingService geocodingService, IAppSettingsProvider appSettings, ILog logger, IHostApplicationLifetime hostApplicationLifetime, IScheduler scheduler)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.addressRepository = addressRepository;
			this.geocodingService = geocodingService;
			this.appSettings = appSettings;
			this.scheduler = scheduler;
		}
	}
}