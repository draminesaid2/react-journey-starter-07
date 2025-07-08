using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
using System.Data;

namespace Crm.Database
{
    [Migration(20220301152000)]
    public class AddColumnPersonNoToCrmPerson : Migration
    {
        public override void Up()
        {
            if (Database.TableExists("[CRM].[Person]"))
            {
                Database.AddColumnIfNotExisting("[CRM].[Person]", new Column("PersonNo", DbType.String, 20, ColumnProperty.Null));
            }
        }
    }
}
