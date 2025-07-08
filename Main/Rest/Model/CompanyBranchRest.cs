namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Rest;
	using Crm.Model;

	[RestTypeFor(DomainType = typeof(CompanyBranch))]
	public class CompanyBranchRest : RestEntityWithExtensionValues
	{
		public Guid CompanyKey { get; set; }
		public string Branch1Key { get; set; }
		public string Branch2Key { get; set; }
		public string Branch3Key { get; set; }
		public string Branch4Key { get; set; }
	}
}
