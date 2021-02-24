
DROP TABLE IF EXISTS [dbo].[AuditDispute]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditDispute](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuditMainID] [int] NOT NULL,
	[TicketID] [nvarchar](255) NULL,
	[QuestionId] [int] NOT NULL REFERENCES QuestionBank(QuestionID),
	[QuestionRank] [int] NULL,
	[GracePeriodId] [int] NULL,
	[OverTurnId] [int] NULL,
	[Comments] [nvarchar](max) NULL,
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

ALTER TABLE [dbo].[AuditDispute]  WITH CHECK ADD FOREIGN KEY([AuditMainID])
REFERENCES [dbo].[AuditMain] ([ID])
GO


