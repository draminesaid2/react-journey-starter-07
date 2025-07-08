namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220218090000)]
	public class AddArticleProperties : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.Article", "IsEnabled"))
			{
				Database.AddColumn("CRM.Article", new Column("IsEnabled"){
					Type = DbType.Boolean,
					ColumnProperty = ColumnProperty.NotNull,
					DefaultValue = true
				});
			}
			
			if (!Database.ColumnExists("CRM.Article", "ValidTo"))
			{
				Database.AddColumn("CRM.Article", "ValidTo", DbType.DateTime, ColumnProperty.Null);
			}
			
			if (!Database.ColumnExists("CRM.Article", "ValidFrom"))
			{
				Database.AddColumn("CRM.Article", "ValidFrom", DbType.DateTime, ColumnProperty.Null);
			}
			
			if (!Database.ColumnExists("CRM.Article", "ManufacturerItemNo"))
			{
				Database.AddColumn("CRM.Article", "ManufacturerItemNo", DbType.String, 50, ColumnProperty.Null);
			}
			
			if (!Database.ColumnExists("CRM.Article", "GuaranteeInMonths"))
			{
				Database.AddColumn("CRM.Article", "GuaranteeInMonths", DbType.Int32, ColumnProperty.Null);
			}
			if (!Database.ColumnExists("CRM.Article", "WarrantyInMonths"))
			{
				Database.AddColumn("CRM.Article", "WarrantyInMonths", DbType.Int32, ColumnProperty.Null);
			}
			
			if (!Database.ColumnExists("CRM.Article", "LeadTimeInDays"))
			{
				Database.AddColumn("CRM.Article", "LeadTimeInDays", DbType.Decimal, ColumnProperty.Null);
			}
		}
	}
}
