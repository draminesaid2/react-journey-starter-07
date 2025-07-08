namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200310143000)]
	public class UpdateTimePickerValueToTime : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					SET DATEFORMAT DMY
					UPDATE [CRM].[DynamicFormResponse]
					SET Value = CASE WHEN dbo.IsDateInDST(ModifyDate) = 1
												THEN FORMAT(DATEADD(MINUTE, 60 + DATEPART(tz, SYSDATETIMEOFFSET()), CAST(Value as time)), 'hh\:mm')
												ELSE FORMAT(DATEADD(MINUTE, DATEPART(tz, SYSDATETIMEOFFSET()), CAST(Value as time)), 'hh\:mm')
											END,
						ModifyDate = GETUTCDATE(),
						ModifyUser = 'Migration_20200310143000'
					WHERE DynamicFormElementType = 'Time' AND Value IS NOT NULL AND Value != '' AND Value NOT LIKE 'PT%'");

			Database.ExecuteNonQuery(@"UPDATE [CRM].[DynamicFormResponse]
					SET Value = FORMAT(CAST(CONCAT( 
								CASE
									WHEN CHARINDEX('H', Value) = 0 THEN 0
									ELSE SUBSTRING(Value, CHARINDEX('T', Value) + 1, CHARINDEX('H', Value) - CHARINDEX('T', Value) - 1)
								END, ':',
								CASE
									WHEN CHARINDEX('M', Value) = 0 THEN 0
									ELSE
										CASE
											WHEN CHARINDEX('H', Value) = 0 THEN SUBSTRING(Value, CHARINDEX('T', Value) + 1, CHARINDEX('M', Value) - CHARINDEX('T', Value) - 1)
											ELSE SUBSTRING(Value, CHARINDEX('H', Value) + 1, CHARINDEX('M', Value) - CHARINDEX('H', Value) - 1)
										END
								END) as time), 'hh\:mm'),
						ModifyDate = GETUTCDATE(),
						ModifyUser = 'Migration_20200310143000'
					WHERE DynamicFormElementType = 'Time' AND Value LIKE 'PT%'");

			Database.ExecuteNonQuery(@"
					UPDATE [CRM].[DynamicFormReference]
						SET ModifyDate = GETUTCDATE(),
								ModifyUser = 'Migration_20200310143000'
						WHERE [DynamicFormReferenceId] IN (
								SELECT DISTINCT DynamicFormReferenceKey 
								FROM [CRM].[DynamicFormResponse] 
								WHERE [ModifyUser] = 'Migration_20200310143000')");
		}
	}
}