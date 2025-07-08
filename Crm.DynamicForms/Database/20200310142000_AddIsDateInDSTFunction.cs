namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200310142000)]
	public class AddIsDateInDSTFunction : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					CREATE FUNCTION dbo.IsDateInDST(@value AS DATETIME)
					RETURNS BIT
					AS
						BEGIN
							DECLARE
								@StartOfApril DATETIME,
								@StartOfNovember DATETIME,
								@DstEnd DATETIME,
								@DstStart DATETIME 

							SET @StartOfApril = DATEADD(MONTH, 3, DATEADD(YEAR, DATEPART(YYYY, @value) - 1900, 0))
							SET @StartOfNovember = DATEADD(MONTH, 10, DATEADD(YEAR, DATEPART(YYYY, @value) - 1900, 0))
							SET @DstStart = DATEADD(HOUR, 2, DATEADD(day, ((15 - DATEPART(dw, @StartOfApril)) % 7) - 7, @StartOfApril))
							SET @DstEnd = DATEADD(HOUR, 2, DATEADD(day, ((8 - DATEPART(dw, @StartOfNovember)) % 7) - 7, @StartOfNovember))
							IF @value >= @DstStart AND @value < @DstEnd
								RETURN 1
							RETURN 0
					END");
		}
	}
}