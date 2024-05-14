CREATE TABLE [dbo].[T_OrderDetail]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderID] BIGINT NOT NULL, 
    [ProductID] BIGINT NOT NULL, 
    [Price] NUMERIC(18, 4) NOT NULL, 
    [CreateDate] DATETIME NOT NULL, 
    [Description] NVARCHAR(100) NULL
)
