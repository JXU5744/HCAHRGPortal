go
DROP TABLE IF EXISTS [dbo].[HROCRoster]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HROCRoster](
	[HROCRosterId] [int] IDENTITY(1,1) NOT NULL,
	[Employee Full Name] [nvarchar](255) NULL,
	[Last Name] [nvarchar](255) NULL,
	[First Name] [nvarchar](255) NULL,
	[Employee 3-4 ID (Lower Case)] [nvarchar](255) NULL,
	[Employee Num] [float] NULL,
	[Supervisor Last Name] [nvarchar](255) NULL,
	[Supervisor First Name] [nvarchar](255) NULL,
	[Date Hired] [datetime] NULL,
	[Job Cd Desc - Home Curr] [nvarchar](255) NULL,
	[Position Desc - Home Curr] [nvarchar](255) NULL,
	[Employee Status] [nvarchar](255) NULL,
	[Employee Status Desc] [nvarchar](255) NULL,
	[CreatedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[HROCRosterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


