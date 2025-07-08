namespace Crm.Database.Modify
{
	using System.Collections.Generic;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220629145111)]
	public class AddColorValuesToArticleType : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[ArticleType]") && Database.ColumnExists("[LU].[ArticleType]", "Color"))
			{
				var colorValues = new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("Material","#2196F3"),
					new KeyValuePair<string, string>("Service","#4CAF50"),
					new KeyValuePair<string, string>("Tool","#795548"),
					new KeyValuePair<string, string>("Cost","#F44336"),
					new KeyValuePair<string, string>("Set","#673AB7"),
					new KeyValuePair<string, string>("Variable","#FF9800"),
					new KeyValuePair<string, string>("ConfigurationBase","#9C27B0")
				};
				foreach (var colorvalue in colorValues)
				{
					Database.ExecuteNonQuery($"UPDATE [LU].[ArticleType] SET Color = '{colorvalue.Value}' Where Value = '{colorvalue.Key}'");
				}
			}
		}
	}
}
