USE [master]
GO
/****** Object:  Database [HCA_HROPS_Rpt_temp]    Script Date: 1/25/2021 2:08:35 AM ******/
CREATE DATABASE [HCA_HROPS_Rpt_temp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HCA_HROPS_Rpt_temp', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HCA_HROPS_Rpt_temp.mdf' , SIZE = 45120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HCA_HROPS_Rpt_temp_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\HCA_HROPS_Rpt_temp_log.ldf' , SIZE = 11456KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HCA_HROPS_Rpt_temp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ARITHABORT OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET  MULTI_USER 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [HCA_HROPS_Rpt_temp]
GO
/****** Object:  User [HCA\DRU5602]    Script Date: 1/25/2021 2:08:35 AM ******/
CREATE USER [HCA\DRU5602] FOR LOGIN [HCA\DRU5602] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [HCA\DRU5602]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/25/2021 2:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[CatgID] [int] IDENTITY(1,1) NOT NULL,
	[CatgDescription] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CatgID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QuestionMaster1]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[QuestionMaster1](
	[QuestionID] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NULL,
	[DepartmentName] [varchar](100) NOT NULL,
	[SubCategoryCode] [int] NULL,
	[SubCategoryName] [varchar](100) NOT NULL,
	[QuestionNumber] [int] NOT NULL,
	[QuestionText] [varchar](max) NOT NULL,
	[QuestionScore] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubCategory](
	[SubCatgID] [int] IDENTITY(1,1) NOT NULL,
	[CatgID] [int] NOT NULL,
	[SubCatgDescription] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED 
(
	[SubCatgID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCMADispute]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCMADispute](
	[TicketID] [nvarchar](255) NULL,
	[AgentName] [nvarchar](255) NULL,
	[Agent34ID] [nvarchar](255) NULL,
	[AuditorName] [nvarchar](255) NULL,
	[DisputeDT] [datetime] NULL,
	[Q1DNotes] [nvarchar](255) NULL,
	[Q2DNotes] [nvarchar](255) NULL,
	[Q3DNotes] [nvarchar](255) NULL,
	[Q4DNotes] [nvarchar](255) NULL,
	[Q5DNotes] [nvarchar](255) NULL,
	[Q6DNotes] [nvarchar](255) NULL,
	[Q7DNotes] [nvarchar](255) NULL,
	[Q8DNotes] [nvarchar](255) NULL,
	[Q9DNotes] [nvarchar](255) NULL,
	[Q10DNotes] [nvarchar](255) NULL,
	[DisputeWeek] [int] NULL,
	[DisputeMonth] [int] NULL,
	[DisputeDateHalf] [int] NULL,
	[DisputeYear] [int] NULL,
	[ServiceGroup] [varchar](3) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCMAMain]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCMAMain](
	[TicketID] [nvarchar](255) NULL,
	[AgentName] [nvarchar](255) NULL,
	[Agent34ID] [nvarchar](255) NULL,
	[AuditorName] [nvarchar](255) NULL,
	[SupervisorName] [nvarchar](255) NULL,
	[SubmitDT] [date] NULL,
	[Q1Score] [int] NULL,
	[Q2Score] [int] NULL,
	[Q3Score] [int] NULL,
	[Q4Score] [int] NULL,
	[Q5Score] [int] NULL,
	[Q6Score] [int] NULL,
	[Q7Score] [int] NULL,
	[Q8Score] [int] NULL,
	[Q9Score] [int] NULL,
	[Q10Score] [int] NULL,
	[TotalActual] [decimal](18, 0) NULL,
	[TotalPossible] [decimal](18, 0) NULL,
	[TotalPercent] [decimal](18, 0) NULL,
	[AuditNotes] [nvarchar](max) NULL,
	[ErrorCount] [int] NULL,
	[IntErrorCount] [int] NULL,
	[ExtErrorCount] [int] NULL,
	[ErrorType] [nvarchar](255) NULL,
	[AuditorQuit] [nvarchar](255) NULL,
	[AuditorQuitReason] [nvarchar](255) NULL,
	[Q1PScore] [int] NULL,
	[Q2PScore] [int] NULL,
	[Q3PScore] [int] NULL,
	[Q4PScore] [int] NULL,
	[Q5PScore] [int] NULL,
	[Q6PScore] [int] NULL,
	[Q7PScore] [int] NULL,
	[Q8PScore] [int] NULL,
	[Q9PScore] [int] NULL,
	[Q10PScore] [int] NULL,
	[AuditWeek] [int] NULL,
	[AuditMonth] [int] NULL,
	[AuditDateHalf] [int] NULL,
	[AuditYear] [int] NULL,
	[Disputed] [nvarchar](255) NULL,
	[Q1IEC] [int] NULL,
	[Q2IEC] [int] NULL,
	[Q3IEC] [int] NULL,
	[Q4IEC] [int] NULL,
	[Q5IEC] [int] NULL,
	[Q6IEC] [int] NULL,
	[Q7IEC] [int] NULL,
	[Q8IEC] [int] NULL,
	[Q9IEC] [int] NULL,
	[Q10IEC] [int] NULL,
	[Q1EEC] [int] NULL,
	[Q2EEC] [int] NULL,
	[Q3EEC] [int] NULL,
	[Q4EEC] [int] NULL,
	[Q5EEC] [int] NULL,
	[Q6EEC] [int] NULL,
	[Q7EEC] [int] NULL,
	[Q8EEC] [int] NULL,
	[Q9EEC] [int] NULL,
	[Q10EEC] [int] NULL,
	[Subcategory] [nvarchar](max) NULL,
	[ServiceGroup] [varchar](3) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCMATDispute]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCMATDispute](
	[TicketID] [nvarchar](255) NULL,
	[AgentName] [nvarchar](255) NULL,
	[Agent34ID] [nvarchar](255) NULL,
	[AuditorName] [nvarchar](255) NULL,
	[DisputeDT] [datetime] NULL,
	[Q1DNotes] [nvarchar](255) NULL,
	[Q2DNotes] [nvarchar](255) NULL,
	[Q3DNotes] [nvarchar](255) NULL,
	[Q4DNotes] [nvarchar](255) NULL,
	[Q5DNotes] [nvarchar](255) NULL,
	[Q6DNotes] [nvarchar](255) NULL,
	[Q7DNotes] [nvarchar](255) NULL,
	[Q8DNotes] [nvarchar](255) NULL,
	[Q9DNotes] [nvarchar](255) NULL,
	[Q10DNotes] [nvarchar](255) NULL,
	[DisputeWeek] [int] NULL,
	[DisputeMonth] [int] NULL,
	[DisputeDateHalf] [int] NULL,
	[DisputeYear] [int] NULL,
	[ServiceGroup] [varchar](3) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCMATMain]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCMATMain](
	[TicketID] [nvarchar](255) NULL,
	[AgentName] [nvarchar](255) NULL,
	[Agent34ID] [nvarchar](255) NULL,
	[AuditorName] [nvarchar](255) NULL,
	[SupervisorName] [nvarchar](255) NULL,
	[SubmitDT] [date] NULL,
	[Q1Score] [int] NULL,
	[Q2Score] [int] NULL,
	[Q3Score] [int] NULL,
	[Q4Score] [int] NULL,
	[Q5Score] [int] NULL,
	[Q6Score] [int] NULL,
	[Q7Score] [int] NULL,
	[Q8Score] [int] NULL,
	[Q9Score] [int] NULL,
	[Q10Score] [int] NULL,
	[TotalActual] [decimal](18, 0) NULL,
	[TotalPossible] [decimal](18, 0) NULL,
	[TotalPercent] [decimal](18, 0) NULL,
	[AuditNotes] [nvarchar](max) NULL,
	[ErrorCount] [int] NULL,
	[IntErrorCount] [int] NULL,
	[ExtErrorCount] [int] NULL,
	[ErrorType] [nvarchar](255) NULL,
	[AuditorQuit] [nvarchar](255) NULL,
	[AuditorQuitReason] [nvarchar](255) NULL,
	[Q1PScore] [int] NULL,
	[Q2PScore] [int] NULL,
	[Q3PScore] [int] NULL,
	[Q4PScore] [int] NULL,
	[Q5PScore] [int] NULL,
	[Q6PScore] [int] NULL,
	[Q7PScore] [int] NULL,
	[Q8PScore] [int] NULL,
	[Q9PScore] [int] NULL,
	[Q10PScore] [int] NULL,
	[AuditWeek] [int] NULL,
	[AuditMonth] [int] NULL,
	[AuditDateHalf] [int] NULL,
	[AuditYear] [int] NULL,
	[Disputed] [nvarchar](255) NULL,
	[Q1IEC] [int] NULL,
	[Q2IEC] [int] NULL,
	[Q3IEC] [int] NULL,
	[Q4IEC] [int] NULL,
	[Q5IEC] [int] NULL,
	[Q6IEC] [int] NULL,
	[Q7IEC] [int] NULL,
	[Q8IEC] [int] NULL,
	[Q9IEC] [int] NULL,
	[Q10IEC] [int] NULL,
	[Q1EEC] [int] NULL,
	[Q2EEC] [int] NULL,
	[Q3EEC] [int] NULL,
	[Q4EEC] [int] NULL,
	[Q5EEC] [int] NULL,
	[Q6EEC] [int] NULL,
	[Q7EEC] [int] NULL,
	[Q8EEC] [int] NULL,
	[Q9EEC] [int] NULL,
	[Q10EEC] [int] NULL,
	[Subcategory] [nvarchar](255) NULL,
	[ServiceGroup] [varchar](3) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbHROCAuditors]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbHROCAuditors](
	[Agent34ID] [nvarchar](255) NULL,
	[AgentRole] [varchar](7) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbHROCRoster]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbHROCRoster](
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
	[Employee Status Desc] [nvarchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblQuestionBank]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblQuestionBank](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionName] [varchar](250) NOT NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbQuestionMaster]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbQuestionMaster](
	[QuestionId] [int] NOT NULL,
	[SubCatgID] [int] NOT NULL,
	[QuestionText] [varchar](250) NOT NULL,
	[QuestionScore] [int] NULL,
	[Status] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbstatus]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbstatus](
	[statusID] [int] NOT NULL,
	[statustext] [varchar](10) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TicketsViaSSIS]    Script Date: 1/25/2021 2:08:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketsViaSSIS](
	[TicketCode] [nvarchar](20) NOT NULL,
	[LastEditDateTime] [datetime2](7) NOT NULL,
	[AboutEE] [nvarchar](255) NULL,
	[Archived] [bit] NULL,
	[CaseType] [nvarchar](255) NULL,
	[Category] [nvarchar](255) NULL,
	[ChatAgentUserID] [nvarchar](255) NULL,
	[ClosedDateTime] [datetime2](7) NULL,
	[CloseUserID] [nvarchar](255) NULL,
	[ContactMethod] [nvarchar](255) NULL,
	[ContactName] [nvarchar](255) NULL,
	[ContactRelationshipName] [nvarchar](255) NULL,
	[CreatedDateTime] [datetime2](7) NULL,
	[CreatorFirstName] [nvarchar](255) NULL,
	[CreatorLastName] [nvarchar](255) NULL,
	[CreatorName] [nvarchar](255) NULL,
	[CreatorUserID] [nvarchar](255) NULL,
	[CustomCheckBox1] [bit] NULL,
	[CustomCheckBox2] [bit] NULL,
	[CustomCheckBox3] [bit] NULL,
	[CustomCheckBox4] [bit] NULL,
	[CustomCheckBox5] [bit] NULL,
	[CustomCheckBox6] [bit] NULL,
	[CustomDate1] [datetime2](7) NULL,
	[CustomDate2] [datetime2](7) NULL,
	[CustomDate3] [datetime2](7) NULL,
	[CustomDate4] [datetime2](7) NULL,
	[CustomDate5] [datetime2](7) NULL,
	[CustomDate6] [datetime2](7) NULL,
	[CustomSelect1] [nvarchar](255) NULL,
	[CustomSelect2] [nvarchar](255) NULL,
	[CustomSelect3] [nvarchar](255) NULL,
	[CustomSelect4] [nvarchar](255) NULL,
	[CustomSelect5] [nvarchar](255) NULL,
	[CustomSelect6] [nvarchar](255) NULL,
	[CustomString1] [nvarchar](255) NULL,
	[CustomString2] [nvarchar](255) NULL,
	[CustomString3] [nvarchar](255) NULL,
	[CustomString4] [nvarchar](255) NULL,
	[CustomString5] [nvarchar](255) NULL,
	[CustomString6] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[IsFirstCallResolution] [nvarchar](255) NULL,
	[Issue] [nvarchar](3000) NULL,
	[KnowledgeDomain] [nvarchar](255) NULL,
	[LastEditUserID] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[OwnerUserID] [nvarchar](255) NULL,
	[Population] [nvarchar](255) NULL,
	[Priority] [nvarchar](255) NULL,
	[ProcessTime] [smallint] NULL,
	[RegardingUserID] [nvarchar](255) NULL,
	[ReminderDateTime] [datetime2](7) NULL,
	[ReminderEmail] [nvarchar](255) NULL,
	[ReminderNote] [nvarchar](1000) NULL,
	[ReminderPhone] [nvarchar](255) NULL,
	[Resolution] [nvarchar](3000) NULL,
	[ResolvedDateTime] [datetime2](7) NULL,
	[Secure] [nvarchar](255) NULL,
	[ServiceGroup] [nvarchar](255) NULL,
	[ShowToEE] [nvarchar](255) NULL,
	[SLA] [datetime2](7) NULL,
	[Source] [nvarchar](255) NULL,
	[SubCategory] [nvarchar](255) NULL,
	[Subject] [nvarchar](1000) NULL,
	[SubStatus] [nvarchar](255) NULL,
	[SurveyAgreementResponse] [nvarchar](255) NULL,
	[SurveyAnswer1] [nvarchar](255) NULL,
	[SurveyAnswer2] [nvarchar](255) NULL,
	[SurveyAnswer3] [nvarchar](255) NULL,
	[SurveyAnswer4] [nvarchar](255) NULL,
	[SurveyAnswer5] [nvarchar](255) NULL,
	[SurveyCommentResponse] [nvarchar](1000) NULL,
	[SurveyDateTime] [datetime2](7) NULL,
	[SurveyFollowup] [nvarchar](255) NULL,
	[SurveyID] [nvarchar](255) NULL,
	[SurveyScore] [nvarchar](255) NULL,
	[TicketStatus] [nvarchar](255) NULL,
	[Topic] [ntext] NULL,
	[UserID] [nvarchar](255) NULL,
	[CreateDate] [datetime2](7) NULL,
	[SLADate] [datetime2](7) NULL,
	[ClosedDate] [datetime2](7) NULL,
	[LoadStamp] [datetime2](7) NULL,
	[SourceFileStamp] [nvarchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [HCA_HROPS_Rpt_temp] SET  READ_WRITE 
GO
