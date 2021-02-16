DROP TABLE IF EXISTS [dbo].[QuestionBank]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[QuestionBank](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionName] [varchar](max) NULL,
	[QuestionDescription] [varchar](max) NULL,
	[IsActive] [bit] NULL DEFAULT ((1)),
	[CreatedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime] NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,
	

PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


