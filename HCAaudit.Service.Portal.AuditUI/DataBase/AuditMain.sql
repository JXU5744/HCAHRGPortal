DROP TABLE IF EXISTS [dbo].[AuditMain]
GO

/****** Object:  Table [dbo].[AuditMain]    Script Date: 2/13/2021 10:06:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditMain](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TicketID] [nvarchar](255) NOT NULL,
	[TicketDate] [datetime] NOT NULL,
	[AgentName] [nvarchar](255) NULL,
	[Agent34ID] [nvarchar](255) NULL,
	[AuditorName] [nvarchar](255) NULL,
	[SubmitDT] [datetime] NULL,
	[SubcategoryID] [int] FOREIGN KEY REFERENCES Subcategory(SubCatgId) NOT NULL,
	[ServiceGroupID] [int] FOREIGN KEY REFERENCES Category(CatgId) NOT NULL,
	[isDisputed] [bit] NULL,
	[DisputeDate] [datetime] NULL,
	[DisputeAuditor34ID] [nvarchar](255) NULL,
	[AuditorQuit] [nvarchar](255) NULL,
	[AuditorQuitReason] [nvarchar](255) NULL,
	[AuditType] [nvarchar](255) NULL,
	[AuditNotes] [nvarchar](max) NULL,
	[CreatedDate] [DateTime] NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,
	
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


