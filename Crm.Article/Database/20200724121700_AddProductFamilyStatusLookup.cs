namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200724121700)]
	public class AddProductFamilyStatusLookup : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
			CREATE TABLE [LU].[ProductFamilyStatus](
				[ProductFamilyStatusId] [int] IDENTITY(1,1) NOT NULL,
				[Name] [nvarchar](50) NOT NULL,
				[Language] [nvarchar](2) NOT NULL,
				[Value] [nvarchar](20) NOT NULL,
				[Favorite] [bit] NOT NULL,
				[SortOrder] [int] NOT NULL,
				[Color] [nvarchar](20) NOT NULL,
				[CreateDate] [datetime] NOT NULL,
				[ModifyDate] [datetime] NOT NULL,
				[CreateUser] [nvarchar](256) NOT NULL,
				[ModifyUser] [nvarchar](256) NOT NULL,
				[IsActive] [bit] NOT NULL,
				PRIMARY KEY CLUSTERED 
				(
					[ProductFamilyStatusId] ASC
				  )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				    ) ON [PRIMARY]
				ALTER TABLE [LU].[ProductFamilyStatus] ADD  CONSTRAINT [DF_ProductFamilyStatus_Favorite]  DEFAULT ((0)) FOR [Favorite]
				ALTER TABLE [LU].[ProductFamilyStatus] ADD  CONSTRAINT [DF_ProductFamilyStatus_SortOrder]  DEFAULT ((0)) FOR [SortOrder]
				ALTER TABLE [LU].[ProductFamilyStatus] ADD  CONSTRAINT [DF_LUProductFamilyStatus_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
				ALTER TABLE [LU].[ProductFamilyStatus] ADD  CONSTRAINT [DF_LUProductFamilyStatus_ModifyDate]  DEFAULT (getutcdate()) FOR [ModifyDate]
				ALTER TABLE [LU].[ProductFamilyStatus] ADD  CONSTRAINT [DF_LUProductFamilyStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]

        SET IDENTITY_INSERT [LU].[ProductFamilyStatus] ON 

				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (1, N'Entwurf', N'de', N'draft', 0, 0, N'#FFC107', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (2, N'Draft', N'en', N'draft', 0, 0, N'#FFC107', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (3, N'Borrador', N'es', N'draft', 0, 0, N'#FFC107', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (4, N'Brouillon', N'fr', N'draft', 0, 0, N'#FFC107', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (5, N'Aktiv', N'de', N'active', 0, 0, N'#8BC34A', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (6, N'Active', N'en', N'active', 0, 0, N'#8BC34A', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (7, N'Actif', N'fr', N'active', 0, 0, N'#8BC34A', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (8, N'Activo', N'es', N'active', 0, 0, N'#8BC34A', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (9, N'Im Ruhestand', N'de', N'retired', 0, 0, N'#9E9E9E', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (10, N'Retired', N'en', N'retired', 0, 0, N'#9E9E9E', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)				
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (11, N'Retiré', N'fr', N'retired', 0, 0, N'#9E9E9E', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)
				INSERT [LU].[ProductFamilyStatus] ([ProductFamilyStatusId], [Name], [Language], [Value], [Favorite], [SortOrder], [Color], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES (12, N'Retirado', N'es', N'retired', 0, 0, N'#9E9E9E', GETUTCDATE(), GETUTCDATE(), 'Migration_20200724121700', 'Migration_20200724121700', 1)
				
				SET IDENTITY_INSERT [LU].[ProductFamilyStatus] OFF			
      ");
		}
	}
}