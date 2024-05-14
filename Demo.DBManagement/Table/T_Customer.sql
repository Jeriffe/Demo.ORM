CREATE TABLE [dbo].[T_Customer]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(500) NOT NULL, 
    [Gender] BIT NOT NULL, 
    [Birthday] DATETIME NULL, 
    [Phone] NVARCHAR(10) NULL, 
    [Address] NVARCHAR(500) NULL
)
