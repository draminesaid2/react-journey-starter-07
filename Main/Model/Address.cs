namespace Crm.Model
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization;
	using Crm.Model.Lookups;

	public class Address : EntityBase<Guid>, IEntityWithGeocode, IExportable, ISoftDelete
	{
		// Properties
		public virtual string LegacyId { get; set; }
		public virtual Guid? CompanyId { get; set; }
		public virtual Contact Contact { get; set; }
		public virtual bool IsCompanyStandardAddress { get; set; }
		public virtual string Name1 { get; set; }
		public virtual string Name2 { get; set; }
		public virtual string Name3 { get; set; }
		public virtual string City { get; set; }
		public virtual string POBox { get; set; }
		public virtual string ZipCode { get; set; }
		public virtual string ZipCodePOBox { get; set; }
		public virtual string Street { get; set; }
		public virtual string CountryKey { get; set; }
		public virtual string RegionKey { get; set; }
		public virtual string AddressTypeKey { get; set; }
		public virtual string LanguageKey { get; set; }
		public virtual bool IsExported { get; set; }

		public virtual string AddressString { get; set; }
		
		public virtual Country Country
		{
			get { return CountryKey != null ? LookupManager.Get<Country>(CountryKey) : null; }
		}
		public virtual Region Region
		{
			get { return RegionKey != null ? LookupManager.Get<Region>(RegionKey) : null; }
		}
		public virtual AddressType AddressType
		{
			get { return AddressTypeKey != null ? LookupManager.Get<AddressType>(AddressTypeKey) : null; }
		}
		public virtual Language Language
		{
			get { return LanguageKey != null ? LookupManager.Get<Language>(LanguageKey) : null; }
		}

		public virtual double? Latitude { get; set; }
		public virtual double? Longitude { get; set; }
		public virtual int GeocodingRetryCounter { get; set; }

		public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
		public virtual ICollection<Fax> Faxes { get; set; } = new List<Fax>();
		public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
		public virtual ICollection<Website> Websites { get; set; } = new List<Website>();

		public override string ToString()
		{
			return ToString(String.Empty);
		}
		/// <summary>
		/// Return the address as a string with different formats. Parameter is not case sensitive.
		/// e.g.
		/// "M" for Multiline format with web separators <![CDATA[</br>]]><br/>
		/// "G" for Google Maps query format
		/// </summary>
		/// <param name="format">The address format, M for multiline, G for google maps, none for inline display.</param>
		/// <param name="displayOnlyRegionKey">If set to true, display only the key instead of value.</param>
		/// <param name="region">The region to use, can be specified to prevent database access during formatting.</param>
		/// <param name="country">The country  to use, can be specified to prevent database access during formatting.</param>
		/// <returns>A formatted address.</returns>
		public virtual string ToString(string format, bool displayOnlyRegionKey = false, Region region = null, Country country = null)
		{
			string separator;
			var includePoBox = true;
			var includeRegion = true;
			var htmlEncode = false;
			region = region ?? Region;
			country = country ?? Country;

			var sb = new StringBuilder();

			switch (format.ToUpper())
			{
				case "M":
					// Multiline web format
					separator = "<br/>";
					htmlEncode = true;
					break;
				case "G":
					// Google Maps Format
					separator = "+";
					htmlEncode = true;
					includePoBox = false;
					includeRegion = false;
					break;
				default:
					// Fallback
					separator = " ";
					break;
			}
			if (format == "G" && Latitude.HasValue && Latitude > 0 && Longitude.HasValue && Longitude > 0)
			{
				return $"{Latitude.Value.ToString(CultureInfo.InvariantCulture)},{Longitude.Value.ToString(CultureInfo.InvariantCulture)}";
			}
			if (!String.IsNullOrEmpty(Street) && format == "M")
			{
				sb.Append(Street.Replace("\r\n", "<br/>").Trim().HtmlEncode(false))
					.Append(separator);
			}
			if (!String.IsNullOrEmpty(Street) && format != "M")
			{
				sb.Append(Street.Trim().HtmlEncode(htmlEncode))
				  .Append(separator);
			}
			if (!String.IsNullOrEmpty(ZipCode) || !String.IsNullOrEmpty(City))
			{
				sb.Append(String.Format("{0} {1}", ZipCode, City).HtmlEncode(htmlEncode))
				  .Append(separator);
			}
			if (includePoBox && !String.IsNullOrEmpty(ZipCodePOBox) || !String.IsNullOrEmpty(POBox))
			{
				sb.Append(String.Format("{0} {1}", ZipCodePOBox, POBox).HtmlEncode(htmlEncode))
				  .Append(separator);
			}
			if (includeRegion && !(region == null || region == Region.None))
			{
				if (displayOnlyRegionKey)
				{
					sb.Append(region.Key.HtmlEncode(htmlEncode)).Append(separator);
				}
				else
				{
					sb.Append(region.Value.HtmlEncode(htmlEncode)).Append(separator);
				}
			}

			// It's better to use Iso2Code if available, otherwise fallback to default value
			if (country != null && country != Country.None && country.Iso2Code != null)
			{
				sb.Append(country.Iso2Code.HtmlEncode(htmlEncode))
				  .Append(separator);
			}
			else if (country != null && country != Country.None)
			{
				sb.Append(country.Value.HtmlEncode(htmlEncode))
					.Append(separator);
			}

			if (sb.Length > 0 && sb.ToString(sb.Length - separator.Length, separator.Length).Equals(separator))
			{
				sb.Remove(sb.Length - separator.Length, separator.Length);
			}

			return sb.Replace("  ", " ").ToString();
		}
	}
}
