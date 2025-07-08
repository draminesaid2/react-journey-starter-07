namespace Crm.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Globalization;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model.Lookups;

	using Geocoding;
	using Geocoding.Google;

	using log4net;

	using Microsoft.Extensions.Caching.Distributed;

	public interface IGeocoderCache : ISingletonDependency
	{
		void CacheAddresses(string cacheString, Location[] addresses);
		Location[] GetAddresses(string cacheString);
		bool IsQuotaExceeded();
		void SetQuotaExceeded();
	}

	public class GeocoderCache : Cache<Location[]>, IGeocoderCache
	{
		private const string Geocoding = "Geocoding";
		private const string GeocoderQuotaExceeded = "GeocoderQuotaExceeded";
		public GeocoderCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(GeocoderCache), cache, serializer)
		{
		}
		public virtual void CacheAddresses(string cacheString, Location[] addresses) => DictSet(Geocoding, cacheString, addresses);
		public virtual Location[] GetAddresses(string cacheString) => DictGet(Geocoding, cacheString);
		public virtual bool IsQuotaExceeded() => Get<bool>(GeocoderQuotaExceeded) == true;
		public virtual void SetQuotaExceeded() => Set(GeocoderQuotaExceeded, true, new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
	}

	public class GeocodingService : IGeocodingService
	{
		private readonly ILookupManager lookupManager;
		private readonly IGeocoder geocoder;
		private readonly IGeocoderCache geocoderCache;
		private readonly ILog logger;

		public GeocodingService(ILookupManager lookupManager, IGeocoder geocoder, IGeocoderCache geocoderCache, ILog logger)
		{
			this.lookupManager = lookupManager;
			this.geocoder = geocoder;
			this.geocoderCache = geocoderCache;
			this.logger = logger;
		}

		public virtual bool QuotaExceeded => geocoderCache.IsQuotaExceeded();
		public virtual bool GeocoderIsGoogle => geocoder.GetType().Name == typeof(GoogleGeocoder).Name;

		public virtual bool TryGeocode(IEntityWithGeocode entityWithGeocode)
		{
			if (QuotaExceeded)
			{
				return false;
			}

			var countryKey = entityWithGeocode.CountryKey ?? lookupManager.GetFavoriteKey<Country>();
			Country countryLookup = null;
			var languages = lookupManager.List<Language>().Where(x => x.IsSystemLanguage).OrderBy(x => x.SortOrder).ThenBy(x => x.Key).Select(x => x.Key);
			foreach (var lang in languages)
			{
				countryLookup = lookupManager.Get<Country>(countryKey, lang);
				if (countryLookup != null)
					break;
			}

			if (countryLookup == null)
			{
				entityWithGeocode.GeocodingRetryCounter = 4;
				var countryKeyLogging = countryKey.ToString()?.Replace("\n", "_").Replace("\r", "_");
				logger.WarnFormat("Missing country lookup entry for key {0}, please add accordingly", countryKeyLogging);
				return true;
			}

			try
			{
				var cacheString = $"street:{entityWithGeocode.Street},city:{entityWithGeocode.City},zip:{entityWithGeocode.ZipCode},country:{countryLookup.Value}";
				var cachedResult = geocoderCache.GetAddresses(cacheString);
				var addresses = cachedResult ?? geocoder.GeocodeAsync(entityWithGeocode.Street, entityWithGeocode.City, null, entityWithGeocode.ZipCode, countryLookup.Value).Result.Select(x => x.Coordinates).ToArray();
				if (cachedResult == null && addresses.Any())
				{
					geocoderCache.CacheAddresses(cacheString, addresses);
				}
				if (addresses.Any())
				{
					var address = addresses.First();
					entityWithGeocode.Latitude = address.Latitude;
					entityWithGeocode.Longitude = address.Longitude;

					var addressLogging = address.ToString()?.Replace("\n", "_").Replace("\r", "_");
					var nameLogging = geocoder.GetType().Name.Replace("\n", "_").Replace("\r", "_");
					var cacheStringLogging = cacheString.Replace("\n", "_").Replace("\r", "_");
					logger.DebugFormat("Retrieved point ({0}) from {1} for address {2}", addressLogging, nameLogging, cacheStringLogging);

					return true;
				}
				else
				{
					logger.Debug("geocoding yielded zero results");
					entityWithGeocode.GeocodingRetryCounter = 4;
					return true;
				}
			}
			catch (GoogleGeocodingException ex)
			{
				switch (ex.Status)
				{
					case GoogleStatus.OverQueryLimit:
						logger.Warn("Cannot geocode on google since you have exceeded your Google API query limit.", ex);
						geocoderCache.SetQuotaExceeded();
						return false;
					case GoogleStatus.RequestDenied:
						logger.Warn("Cannot geocode on google because your request denied. Please check your Google API key.", ex);
						return false;
					case GoogleStatus.ZeroResults:
						entityWithGeocode.GeocodingRetryCounter = 4;
						logger.Debug("geocoding yielded zero results", ex);
						return true;
					default:
						entityWithGeocode.GeocodingRetryCounter += 1;
						logger.Error("There was a problem requesting the Google Maps geocoding API.", ex);
						return true;
				}
			}
			catch (Exception ex)
			{
				entityWithGeocode.GeocodingRetryCounter += 1;
				logger.Error("There was a problem during a call to the geocoding API.", ex);
				return true;
			}
		}
	}

	public interface IGeocodingService : IDependency
	{
		bool QuotaExceeded { get; }
		bool GeocoderIsGoogle { get; }
		bool TryGeocode(IEntityWithGeocode entityWithGeocode);
	}
}
