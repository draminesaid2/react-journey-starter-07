namespace Sms.Checklists.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191003165000)]
	public class InsertDynamicFormAttributeFormCategory : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();

			query.AppendLine("INSERT INTO LU.DynamicFormCategory (Name, Language, Value) VALUES ('Erweiterungen', 'de', 'AttributeForm')");
			query.AppendLine("INSERT INTO LU.DynamicFormCategory (Name, Language, Value) VALUES ('Attribute form', 'en', 'AttributeForm')");
			query.AppendLine("INSERT INTO LU.DynamicFormCategory (Name, Language, Value) VALUES ('Formulaire d`attribut', 'fr', 'AttributeForm')");
			query.AppendLine("INSERT INTO LU.DynamicFormCategory (Name, Language, Value) VALUES ('Adatlap', 'hu', 'AttributeForm')");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}
