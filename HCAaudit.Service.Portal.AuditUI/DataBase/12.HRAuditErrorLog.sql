 
DROP TABLE IF EXISTS  [dbo].[HRAuditErrorLog]
GO

 
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HRAuditErrorLog](
	[HRAuditErrorLogId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ErrorType] [varchar](50) NULL,
	[SourceLocation] [varchar](500) NULL,
	[ErrorDescription] [varchar](8000) NULL,
	[CreatedBy] [varchar](100) NULL,
	[CreatedDate] [datetime] DEFAULT(GETDATE()) NULL,
	)
	 
  
