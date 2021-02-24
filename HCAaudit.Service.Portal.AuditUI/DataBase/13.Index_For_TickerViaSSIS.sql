GO
DROP INDEX  IF EXISTS [IX_Subcatg_TCode_CDate_TktVSSIS] ON [dbo].[TicketsViaSSIS]
GO

SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_Subcatg_TCode_CDate_TktVSSIS] ON [dbo].[TicketsViaSSIS]
(
	[SubCategory] ASC,
	[TicketCode] ASC,
	[ClosedDate] ASC
)
INCLUDE([Category],[CloseUserID],[SubStatus],[TicketStatus],[CreateDate]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



DROP INDEX  IF EXISTS [Ix_TCode_CDate_SCategory_TktVSSIS] ON [dbo].[TicketsViaSSIS] WITH ( ONLINE = OFF )
GO

SET ANSI_PADDING ON
GO

CREATE CLUSTERED INDEX [Ix_TCode_CDate_SCategory_TktVSSIS] ON [dbo].[TicketsViaSSIS]
(
	[TicketCode] ASC,
	[ClosedDate] ASC,
	[SubCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



GO
IF EXISTS (SELECT name 
           FROM sys.stats 
           WHERE name = N'Stat_TCode_Cdate_SCatg_TktVSSIS' 
           AND object_id = OBJECT_ID(N'DBO.TicketsViaSSIS'))
BEGIN
 DROP STATISTICS      [dbo].[TicketsViaSSIS].[Stat_TCode_Cdate_SCatg_TktVSSIS]
END


GO

CREATE STATISTICS [Stat_TCode_Cdate_SCatg_TktVSSIS] ON [dbo].[TicketsViaSSIS]([TicketCode], [ClosedDate], [SubCategory])
GO


 