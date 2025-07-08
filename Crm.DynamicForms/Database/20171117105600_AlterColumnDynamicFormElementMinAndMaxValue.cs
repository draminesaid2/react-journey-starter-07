namespace Crm.DynamicForms.Database
{
    using System.Data;

    using Crm.Library.Data.MigratorDotNet.Framework;

    [Migration(20171117105600)]
    public class AlterColumnDynamicFormElementMinAndMaxValue : Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("[CRM].[DynamicFormElement]", "Min"))
            {
                Database.ChangeColumn("[CRM].[DynamicFormElement]", new Column("Min", DbType.Decimal, ColumnProperty.Null));
            }
            if (Database.ColumnExists("[CRM].[DynamicFormElement]", "Max"))
            {
                Database.ChangeColumn("[CRM].[DynamicFormElement]", new Column("Max", DbType.Decimal, ColumnProperty.Null));
            }
        }
    }
}