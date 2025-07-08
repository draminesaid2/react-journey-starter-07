namespace Crm.Article.Model.Lookups
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ProductFamilyDescription]")]
	[IgnoreMissingLookups]
	[RestrictedType(TypeRestriction.None)]
	public class ProductFamilyDescription : EntityLookup<string>
	{
	}
}
