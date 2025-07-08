namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;

	[RestTypeFor(DomainType = typeof(Crm.Model.Company))]
	public class CompanyRest : ContactRest
	{
		public string CompanyTypeKey { get; set; }
		public string AreaSalesManager { get; set; }
		public string SalesRepresentative { get; set; }
		public string CompanyGroupFlag1Key { get; set; }
		public string CompanyGroupFlag2Key { get; set; }
		public string CompanyGroupFlag3Key { get; set; }
		public string CompanyGroupFlag4Key { get; set; }
		public string CompanyGroupFlag5Key { get; set; }
		public string CompanyNo { get; set; }
		public string NumberOfEmployeesKey { get; set; }
		public string TurnoverKey { get; set; }
		public string ShortText { get; set; }
		public bool IsEnabled { get; set; }
		public Guid? StationKey { get; set; }
		[NotReceived] public bool IsOwnCompany { get; set; }
		[NotReceived, ExplicitExpand] public UserRest AreaSalesManagerUser { get; set; }
		[NotReceived, ExplicitExpand] public AddressRest StandardAddress { get; set; }
		[NotReceived, ExplicitExpand] public AddressRest[] Addresses { get; set; }
		[NotReceived, ExplicitExpand] public Email[] Emails { get; set; }
		[NotReceived, ExplicitExpand] public Fax[] Faxes { get; set; }
		[NotReceived, ExplicitExpand] public Phone[] Phones { get; set; }
		[NotReceived, ExplicitExpand] public Website[] Websites { get; set; }
		[NotReceived, ExplicitExpand] public PersonRest[] Staff { get; set; }
		[NotReceived, ExplicitExpand] public TagRest[] Tags { get; set; }
		[NotReceived, ExplicitExpand] public CompanyRest ParentCompany { get; set; }
		[NotReceived, ExplicitExpand] public BravoRest[] Bravos { get; set; }
		[NotReceived, ExplicitExpand] public CompanyRest[] Subsidiaries { get; set; }
		[NotReceived, ExplicitExpand] public CompanyBranchRest[] CompanyBranches { get; set; }
		[NotReceived, ExplicitExpand] public UserRest ResponsibleUserUser { get; set; }
		[ExplicitExpand, NotReceived] public StationRest Station { get; set; }
	}
}
