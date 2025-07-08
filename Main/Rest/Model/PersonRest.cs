namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;

	[RestTypeFor(DomainType = typeof(Crm.Model.Person))]
	public class PersonRest : ContactRest
	{
		public string PersonNo { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string BusinessTitleKey { get; set; }
		public string DepartmentTypeKey { get; set; }
		public bool Mima { get; set; }
		public string SalutationKey { get; set; }
		public string SalutationLetterKey { get; set; }
		public Guid StandardAddressKey { get; set; }
		public string TitleKey { get; set; }
		public bool IsRetired { get; set; }
		public Guid? StationKey { get; set; }
		[NotReceived, ExplicitExpand] public StationRest Station { get; set; }
		[NotReceived, ExplicitExpand] public CompanyRest Parent { get; set; }
		[NotReceived, ExplicitExpand] public AddressRest Address { get; set; }
		[NotReceived, ExplicitExpand] public Email[] Emails { get; set; }
		[NotReceived, ExplicitExpand] public Fax[] Faxes { get; set; }
		[NotReceived, ExplicitExpand] public Phone[] Phones { get; set; }
		[NotReceived, ExplicitExpand] public Website[] Websites { get; set; }
		[NotReceived, ExplicitExpand] public TagRest[] Tags { get; set; }
		[NotReceived, ExplicitExpand] public BravoRest[] Bravos { get; set; }
		[NotReceived, ExplicitExpand] public UserRest ResponsibleUserUser { get; set; }
	}
}
