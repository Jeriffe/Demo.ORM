CREATE TABLE [T_Customer](
    Id      	INTEGER    PRIMARY KEY AUTOINCREMENT NOT NULL, 
	[Name] 		[nvarchar](500) NOT NULL,
	[Gender] 	[bit] NOT NULL,
	[Birthday] [datetime] NULL,
	[Phone] 	[nvarchar](10) NULL,
	[Address] 	[nvarchar](500) NULL
)


CREATE TABLE [T_Order](
    Id          	INTEGER   PRIMARY KEY AUTOINCREMENT NOT NULL, 
	[CustomerID] 	[bigint] NOT NULL,
	[CreateDate] 	[datetime] NULL,
	[Description] 	[nvarchar](100) NULL,
	[TotalPrice] 	[numeric](18, 4) NULL
)

CREATE TABLE [T_OrderItem](
    Id          	INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL, 
	[OrderID] 		[bigint] NOT NULL,
	[ProductID] 	[bigint] NOT NULL,
	[Price] 		[numeric](18, 4) NOT NULL,
	[CreateDate] 	[datetime] NOT NULL,
	[Description] 	[nvarchar](100) NULL
)Q

CREATE TABLE [T_PATIENT](
	[PatientId] 		INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
	[MedRecNumber] 		[nvarchar](255) NULL,
	[FirstName] 		[nvarchar](255) NULL,
	[MiddleInitial] 	[nvarchar](255) NULL,
	[LastName] 			[nvarchar](255) NULL,
	[Gender] 			[nvarchar](255) NULL,
	[BirthDate] 		TEXT (32), NULL,
	[DisChargeDate] 	TEXT (32), NULL,
	[PatientType] 		INTEGER NULL,
	[SiteID] 			INTEGER NULL
)

CREATE TABLE T_Product (
    Id          INTEGER   PRIMARY KEY AUTOINCREMENT NOT NULL,                             
    Name        "[NVARCHAR]" (100)  NOT NULL,
    Price       "[NUMERIC]"  (18,4) NOT NULL,
    Description "[NVARCHAR]" (500) 
);



INSERT INTO [T_Customer] ([Id], [Name], [Gender], [Birthday], [Phone], [Address]) VALUES (1, 'John Doe', 1, CAST('1990-01-01T00:00:00.000' AS DateTime), '1234567890', '123 Main St')

INSERT INTO [T_Customer] ([Id], [Name], [Gender], [Birthday], [Phone], [Address]) VALUES (2, 'Jane Smith', 0, CAST('1985-05-15T00:00:00.000' AS DateTime), '0987654321', '456 Elm Ave')

INSERT INTO [T_Customer] ([Id], [Name], [Gender], [Birthday], [Phone], [Address]) VALUES (3, 'Michael Johnso', 1, CAST('1970-10-30T00:00:00.000' AS DateTime), '5551234567', '789 Oak Rd')

INSERT INTO [T_Customer] ([Id], [Name], [Gender], [Birthday], [Phone], [Address]) VALUES (4, 'Emily Davis', 0, CAST('2000-02-14T00:00:00.000' AS DateTime), '8889990000', '321 Pine St')

INSERT INTO [T_Customer] ([Id], [Name], [Gender], [Birthday], [Phone], [Address]) VALUES (5, 'Robert Williams', 1, CAST('1965-07-04T00:00:00.000' AS DateTime), '2345678901', 'ABC St 123')


INSERT INTO [T_Product] ([Id], [Name], [Price], [Description]) VALUES (1, 'Smartphone 2023', CAST(999.9900 AS Numeric(18, 4)), 'A high-end smartphone with the latest technology.')

INSERT INTO [T_Product] ([Id], [Name], [Price], [Description]) VALUES (2, 'Laptop Pro', CAST(1499.9900 AS Numeric(18, 4)), 'A powerful laptop for professionals with an HD display.')


INSERT INTO [T_Order] ([Id], [CustomerID], [CreateDate], [Description], [TotalPrice]) VALUES (1, 1, CAST('2024-05-15T13:14:49.633' AS DateTime), 'Order placed online', CAST(1579.9700 AS Numeric(18, 4)))

INSERT INTO [T_Order] ([Id], [CustomerID], [CreateDate], [Description], [TotalPrice]) VALUES (2, 2, CAST('2024-05-14T13:14:49.633' AS DateTime), 'Order placed over the phone', CAST(1179.9700 AS Numeric(18, 4)))


INSERT INTO [T_OrderItem] ([Id], [OrderID], [ProductID], [Price], [CreateDate], [Description]) VALUES (1, 1, 2, CAST(1499.9900 AS Numeric(18, 4)), CAST('2024-05-15T13:14:49.637' AS DateTime), 'Order detail')

INSERT INTO [T_OrderItem] ([Id], [OrderID], [ProductID], [Price], [CreateDate], [Description]) VALUES (2, 1, 5, CAST(49.9900 AS Numeric(18, 4)), CAST('2024-05-15T13:14:49.637' AS DateTime), 'Order detail')

INSERT INTO [T_OrderItem] ([Id], [OrderID], [ProductID], [Price], [CreateDate], [Description]) VALUES (3, 1, 10, CAST(29.9900 AS Numeric(18, 4)), CAST('2024-05-15T13:14:49.637' AS DateTime), 'Order detail')

INSERT INTO [T_OrderItem] ([Id], [OrderID], [ProductID], [Price], [CreateDate], [Description]) VALUES (4, 2, 1, CAST(999.9900 AS Numeric(18, 4)), CAST('2024-05-15T13:14:49.637' AS DateTime), 'Order detail')


INSERT INTO [T_PATIENT] ([PatientId], [MedRecNumber], [FirstName], [MiddleInitial], [LastName], [Gender], [BirthDate], [DisChargeDate], [PatientType], [SiteID]) VALUES (1, '938074', 'F1', '', 'LastName938074', 'F', CAST('1983-05-07T12:46:45.0000000' AS DateTime2), CAST('2021-05-07T12:46:45.0000000' AS DateTime2), 0, 999)

INSERT INTO [T_PATIENT] ([PatientId], [MedRecNumber], [FirstName], [MiddleInitial], [LastName], [Gender], [BirthDate], [DisChargeDate], [PatientType], [SiteID]) VALUES (2, '938481', 'F2', '', 'LastName938481', 'M', CAST('1994-05-20T16:14:19.2500000' AS DateTime2), CAST('2021-05-07T12:46:45.0000000' AS DateTime2), 0, 999)


