namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120521194029)]
	public class CopySmsArticlesToCrmArticle : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[Articles]"))
			{
				var sb = new StringBuilder();

				sb.AppendLine("INSERT INTO [CRM].[Article](");
				sb.AppendLine("	[ArticleId],");
				AppendIdenticalColumns(sb);
				sb.AppendLine(") SELECT ");
				sb.AppendLine("	[ContactKey],");
				AppendIdenticalColumns(sb);
                sb.AppendLine("FROM [SMS].[Articles] where ContactKey is not null");

				Database.ExecuteNonQuery(sb.ToString());

				Database.ExecuteNonQuery("sp_rename '[SMS].[Articles]', 'Old_Articles'");
			}
		}
		public override void Down()
		{
		}
		private static void AppendIdenticalColumns(StringBuilder sb)
		{
			sb.AppendLine("	[ItemNo],");
			sb.AppendLine("	[Description],");
			sb.AppendLine("	[ArticleType],");
			sb.AppendLine("	[Price],");
			sb.AppendLine("	[QuantityUnit],");
			sb.AppendLine("	[IsSerial],");
			sb.AppendLine("	[Weight],");
			sb.AppendLine("	[WeightNet],");
			sb.AppendLine("	[Length],");
			sb.AppendLine("	[Width],");
			sb.AppendLine("	[Height],");
			sb.AppendLine("	[MatchCode],");
			sb.AppendLine("	[ArticleGroup01],");
			sb.AppendLine("	[ArticleGroup02],");
			sb.AppendLine("	[ArticleGroup03],");
			sb.AppendLine("	[IsBatch],");
			sb.AppendLine("	[Remark],");
			sb.AppendLine("	[MaintenanceIntervalMonths],");
			sb.AppendLine("	[UserString01],");
			sb.AppendLine("	[UserString02],");
			sb.AppendLine("	[UserString03],");
			sb.AppendLine("	[UserString04],");
			sb.AppendLine("	[UserString05],");
			sb.AppendLine("	[UserFlag01],");
			sb.AppendLine("	[UserFlag02],");
			sb.AppendLine("	[UserFlag03],");
			sb.AppendLine("	[UserFlag04],");
			sb.AppendLine("	[UserFlag05],");
			sb.AppendLine("	[UserDate01],");
			sb.AppendLine("	[UserDate02],");
			sb.AppendLine("	[UserDate03],");
			sb.AppendLine("	[UserReal01],");
			sb.AppendLine("	[UserReal02],");
			sb.AppendLine("	[UserReal03],");
			sb.AppendLine("	[DangerousGoodsFlag],");
			sb.AppendLine("	[MinimumStock],");
			sb.AppendLine("	[Barcode],");
			sb.AppendLine("	[IsSparePart],");
			sb.AppendLine("	[Cost]");
		}
	}
}