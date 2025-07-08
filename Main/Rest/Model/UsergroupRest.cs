namespace Crm.Rest.Model
{
	using System.Collections.Generic;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestrictedType(TypeRestriction.None)]
	[RestTypeFor(DomainType = typeof(Library.Model.Usergroup))]
	public class UsergroupRest : RestEntityWithExtensionValues
	{
		public string Name { get; set; }
		[NotReceived, ExplicitExpand] public IEnumerable<UserRest> Members { get; set; }
	}
}
