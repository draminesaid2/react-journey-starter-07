using Crm.Library.Data.MigratorDotNet.Framework;


namespace Crm.DynamicForms.Database
{

	[Migration(20210616090500)]
	public class AddDynamicFormCategory : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[DynamicFormCategory]"))
			{
				Database.ExecuteNonQuery(@"INSERT INTO [LU].DynamicFormCategory ([Name],[Language],[Value],[CreateDate],[ModifyDate],[CreateUser],[ModifyUser])
						VALUES ('PDF Checklist', 'en', 'PDF-Checklist',GETUTCDATE(),GETUTCDATE(),'system','system'),
						('PDF Checkliste', 'de', 'PDF-Checklist',GETUTCDATE(),GETUTCDATE(),'system','system'),
						('PDF-checklist', 'es', 'PDF-Checklist',GETUTCDATE(),GETUTCDATE(),'system','system'),
						('PDF listes de contrôle des demandes de service', 'fr', 'PDF-Checklist',GETUTCDATE(),GETUTCDATE(),'system','system')");
			}
		}
	}
}