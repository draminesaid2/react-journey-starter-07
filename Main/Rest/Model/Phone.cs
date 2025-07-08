namespace Crm.Rest.Model
{
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Model.Phone))]
	public class Phone : Communication
	{
		public string CountryKey { get; set; }
		public string AreaCode { get; set; }
		[NotReceived]
		public string DataOnlyNumbers { get; set; }
	}
}