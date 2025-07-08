namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Model.Address))]
	public class AddressRest : RestEntityWithExtensionValues
	{
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Street { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string CountryKey { get; set; }
		public string RegionKey { get; set; }
		public string POBox { get; set; }
		public string ZipCodePOBox { get; set; }
		public string AddressTypeKey { get; set; }
		public Guid? CompanyId { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		[NotReceived] public string LegacyId { get; set; }
		public bool IsCompanyStandardAddress { get; set; }
		[NotReceived, ExplicitExpand] public Email[] Emails { get; set; }
		[NotReceived, ExplicitExpand] public Fax[] Faxes { get; set; }
		[NotReceived, ExplicitExpand] public Phone[] Phones { get; set; }
		[NotReceived, ExplicitExpand] public Website[] Websites { get; set; }
	}
}
