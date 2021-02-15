 
/****** Object:  Table [dbo].[AuditMainResponse]    Script Date: 2/13/2021 10:07:38 PM ******/
DROP TABLE IF EXISTS [dbo].[AuditMainResponse]
GO

/****** Object:  Table [dbo].[AuditMainResponse]    Script Date: 2/13/2021 10:07:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditMainResponse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuditMainID] [int] NOT NULL,
	[TicketID] [nvarchar](255) NULL,
	[QuestionId] [int] FOREIGN KEY REFERENCES QuestionBank(QuestionID) NOT NULL ,
	[QuestionRank] [int] NULL,
	[isCompliant] [bit] NULL,
	[isNonCompliant] [bit] NULL,
	[isNA] [bit] NULL,
	[isHighNonComplianceImpact] [bit] NULL,
	[isLowNonComplianceImpact] [bit] NULL,
	[isCorrectionRequired] [bit] NULL,
	[NonComplianceComments] [nvarchar](max) NULL,
	[CreatedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,

PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AuditMainResponse]  WITH CHECK ADD FOREIGN KEY([AuditMainID])
REFERENCES [dbo].[AuditMain] ([ID])
GO


