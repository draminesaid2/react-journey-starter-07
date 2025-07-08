namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200512111800)]
	public class CrmProductFamily : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"
						IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductFamily' AND xtype='U')
							CREATE TABLE [CRM].[ProductFamily] (
								[ContactKey] [uniqueidentifier] NOT NULL,
								[StatusKey] [nvarchar](20) NULL
							) ON [PRIMARY]

						IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'FK_ProductFamily_Contact' and xtype = 'F')
							ALTER TABLE [CRM].[ProductFamily] WITH CHECK ADD  CONSTRAINT [FK_ProductFamily_Contact] FOREIGN KEY([ContactKey])
							REFERENCES [CRM].[Contact] ([ContactId])
              ALTER TABLE [CRM].[ProductFamily] CHECK CONSTRAINT [FK_ProductFamily_Contact]
            ");
		}
	}
}