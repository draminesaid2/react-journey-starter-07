namespace Crm.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Model.Notes;
	using Crm.Model.Relationships;

	[Migration(20180220170000)]
	public class AddEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Address>("CRM", "Address");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<BusinessRelationship>("CRM", "BusinessRelationship");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Bravo>("CRM", "Bravo");

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Email>("CRM", "Communication");
			helper.AddEntityType<Fax>();
			helper.AddEntityType<Phone>();
			helper.AddEntityType<Website>();

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Company>("CRM", "Contact");
			helper.AddEntityType<Folder>();
			helper.AddEntityType<Person>();

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<DocumentAttribute>("CRM", "DocumentAttributes");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<FileResource>("CRM", "FileResource");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<LinkResource>("CRM", "LinkResource");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Message>("CRM", "Message");

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<EmailNote>("CRM", "Note");
			helper.AddEntityType<TaskCompletedNote>();
			helper.AddEntityType<UserNote>();

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Station>("CRM", "Station");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<User>("CRM", "User");

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Tag>("CRM", "ContactTags");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Task>("CRM", "Task");
		}
	}
}