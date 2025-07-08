namespace Crm.Article.Model.Lookups
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ArticleDescription]")]
	[IgnoreMissingLookups]
	[RestrictedType(TypeRestriction.None)]
	public class ArticleDescription : EntityLookup<string>
	{
	}
}
