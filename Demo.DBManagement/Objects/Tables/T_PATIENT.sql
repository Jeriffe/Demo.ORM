CREATE TABLE [dbo].[T_PATIENT](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](20) NULL,
	[MiddleInitial] [nvarchar](20) NULL,
	[LastName] [nvarchar](100) NULL,
	[BirthDate] [datetime] NULL,
	[Gender] [nchar](1) NULL,
	[MedRecNumber] [nvarchar](20) NULL,
	[PatientType] [smallint] NULL,
	[SiteID] [int]  NOT NULL DEFAULT 0 ,
	[DisChargeDate] [datetime] NULL,
	 CONSTRAINT [PK_T_PATIENT] PRIMARY KEY CLUSTERED 
	(
		[PatientID] ASC
	)
) ON [PRIMARY]