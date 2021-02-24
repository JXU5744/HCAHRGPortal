 
DROP TABLE IF EXISTS [dbo].[SubCategory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SubCategory](
	[SubCatgID] [int] IDENTITY(1,1) NOT NULL,
	[CatgID] [int] FOREIGN KEY REFERENCES Category(CatgID) NOT NULL, 
	[SubCatgDescription] [varchar](500) NOT NULL,
	[IsActive] [bit] NULL DEFAULT ((1)),
	[CreatedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[CreatedBy] [varchar](100) NULL,
    [ModifiedDate] [DateTime]  NULL DEFAULT (GETDATE()),
	[ModifiedBy] [varchar](100) NULL,
 CONSTRAINT [PK_SubCategoryid] PRIMARY KEY CLUSTERED 
(
	[SubCatgID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

 


