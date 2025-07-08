namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201108111900)]
	public class AddProductFamilyDescriptionLookup : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
			CREATE TABLE [LU].[ProductFamilyDescription](
							[ProductFamilyDescriptionId][int] IDENTITY(1, 1) NOT FOR REPLICATION NOT NULL,
							[Name] [nvarchar](150) NOT NULL,
							[Language] [char](2) NULL,
							[Value] [nvarchar](450) NOT NULL,
							[Favorite] [bit] NOT NULL,
							[SortOrder] [int] NOT NULL,
							[Remark] [nvarchar](250) NULL,
							[CreateUser] [nvarchar](256) NULL,
							[ModifyUser] [nvarchar](256) NULL,
							[CreateDate] [datetime] NOT NULL,
							[ModifyDate] [datetime] NOT NULL,
							[IsActive] [bit] NOT NULL,
							CONSTRAINT [PK_ProductFamilyDescription] PRIMARY KEY CLUSTERED
								(
							[ProductFamilyDescriptionId] ASC
							) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
							) ON [PRIMARY]
							ALTER TABLE [LU].[ProductFamilyDescription] ADD CONSTRAINT [DF_ProductFamilyDescription_Favorite]  DEFAULT((0)) FOR[Favorite]
							ALTER TABLE [LU].[ProductFamilyDescription] ADD CONSTRAINT [DF_ProductFamilyDescription_SortOrder]  DEFAULT((0)) FOR[SortOrder]
							ALTER TABLE [LU].[ProductFamilyDescription] ADD CONSTRAINT [DF_LUProductFamilyDescription_CreateDate]  DEFAULT(getutcdate()) FOR [CreateDate]
							ALTER TABLE [LU].[ProductFamilyDescription] ADD CONSTRAINT [DF_LUProductFamilyDescription_ModifyDate]  DEFAULT(getutcdate()) FOR [ModifyDate]
							ALTER TABLE [LU].[ProductFamilyDescription] ADD CONSTRAINT [DF_LUProductFamilyDescription_IsActive]  DEFAULT((1)) FOR [IsActive]
				"
				);
		}
	}
}