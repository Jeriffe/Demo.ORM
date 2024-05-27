CREATE TABLE [dbo].[T_Order]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerID] BIGINT NOT NULL, 
    [CreateDate] DATETIME NULL, 
    [Description] NVARCHAR(100) NULL, 
    [TotalPrice] NUMERIC(18, 4) NULL
)
