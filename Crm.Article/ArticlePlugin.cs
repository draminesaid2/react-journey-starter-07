namespace Crm.Article
{
	using Crm.Library.Modularization;

	[Plugin(Requires = "Main")]
	public class ArticlePlugin : Plugin
	{
		public static readonly string PluginName = typeof(ArticlePlugin).Namespace;

		public static class PermissionGroup
		{
			public const string Article = nameof(Model.Article);
			public const string ArticleRelationship = nameof(Model.Relationships.ArticleRelationship);
			public const string ProductFamily = nameof(Model.ProductFamily);
		}
	}
}