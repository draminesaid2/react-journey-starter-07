SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[V].[External_CostCenter]'))
	EXEC sp_executesql N'CREATE VIEW [V].[External_CostCenter] AS SELECT ''This is a code stub which will be replaced by an Alter Statement'' as [code_stub]'
GO

ALTER VIEW [V].[External_CostCenter]
AS
SELECT 
--update code instead name temporary because name is empty
	[Name] COLLATE DATABASE_DEFAULT AS [Name]
	,'de' AS [Language]
	,[Code] COLLATE DATABASE_DEFAULT AS [Value]
	,0 AS [Favorite]
	,0 AS [SortOrder]
	,BINARY_CHECKSUM(
	[Name]
		,[Code]
	) AS [LegacyVersion]
FROM [S].[External_Costcenter] WITH (READUNCOMMITTED)
WHERE  [Code] <> '' and [Name] not like 'Projekt%' and  [Name] not like 'Service%' and  [Name] not like 'FA%'
union 
SELECT 
--update code instead name temporary because name is empty
	[Name] COLLATE DATABASE_DEFAULT AS [Name]
	,'en' AS [Language]
	,[Code] COLLATE DATABASE_DEFAULT AS [Value]
	,0 AS [Favorite]
	,0 AS [SortOrder]
	,BINARY_CHECKSUM(
	[Name]
		,[Code]
	) AS [LegacyVersion]
FROM [S].[External_Costcenter] WITH (READUNCOMMITTED)
WHERE  [Code] <> '' and [Name] not like 'Projekt%' and  [Name] not like 'Service%' and  [Name] not like 'FA%'
Go