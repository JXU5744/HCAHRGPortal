
DROP TABLE  IF EXISTS [dbo].[ListOfValues]
GO

/****** Object:  Table [dbo].[ListOfValues]    Script Date: 2/13/2021 10:14:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ListOfValues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](255) NULL,
	[CodeType] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [DateTime] NOT NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime] NOT NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


