namespace Crm.Article.Database
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130816123700)]
	public class CheckPermissions : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateMaterial') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateMaterial','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditMaterial') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditMaterial','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteMaterial') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteMaterial','Crm.Article') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateService') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateService','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditService') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditService','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteService') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteService','Crm.Article') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateTool') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateTool','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditTool') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditTool','Crm.Article') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteTool') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteTool','Crm.Article') END");

			Database.ExecuteNonQuery(sb.ToString());
		}

		public override void Down()
		{
			
		}
	}
}