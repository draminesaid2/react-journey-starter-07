﻿namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161104)]
	public class AddIndexForSmsInstallationHeadLocationContactId : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[InstallationHead]') AND name = N'IX_InstallationHead_LocationContactId'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_InstallationHead_LocationContactId] ON [SMS].[InstallationHead]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_InstallationHead_LocationContactId] ON [SMS].[InstallationHead] ([LocationContactId])");
		}
	}
}