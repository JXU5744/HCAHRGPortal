
DROP PROCEDURE [dbo].[usp_GetHRAuditSearchResult]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetHRAuditSearchResult]    Script Date: 3/10/2021 4:00:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



 
--GO
-- EXEC [dbo].[usp_GetHRAuditSearchResult] 'Production',3,28,'Audit',0,'','All','','2020-10-01' ,'2020-10-02'
CREATE  PROCEDURE [dbo].[usp_GetHRAuditSearchResult]

  @EnvironmentType varchar(50) = null,
  @CategoryId int = null,
  @SubCategoryId int = NULL,
   @ResultType varchar(50) = null,
   @TicketStatus int = null,
   @TicketSubStatus varchar(50) = NULL,
   @ResultCountCriteria varchar(10) = NULL,
   @AssignedTo varchar(50) = NULL,
   @FromDate DateTime = NULL,
   @ToDate DateTime = NULL,
   @TicketId varchar(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@ResultType = 'Audit')
       BEGIN
              IF (@TicketStatus = 0)
              BEGIN
                     SELECT 
                           ssisTable.TicketCode As TicketCode,
                           cat.CatgDescription as ServiceDeliveryGroup,
                           ssisTable.Category as Category,
                           ssisTable.SubCategory as SubCategory,
						   Subcat.SubCatgID as SubCategoryId,
						   cat.CatgID as CategoryId,
						   ssisTable.TicketStatus as TicketStatus,
						   ssisTable.SubStatus as SubStatus,
                           UPPER(hroc.[Employee 3-4 ID (Lower Case)]) as Agent34ID,
						   CONCAT(hroc.[Supervisor First Name], ' ', hroc.[Supervisor Last Name]) as SupervisorName,
                           hroc.[Employee Full Name] as EmployeeName,
                           convert(varchar, ssisTable.ClosedDate, 101) as ClosedDate,
                           ssisTable.Topic as Topic,
						   CONCAT('/Audit/Index?TicketId=',ssisTable.TicketCode,'&TicketStatus=',
						   @TicketStatus,'&TicketDate=',convert(varchar, ssisTable.ClosedDate, 101),'&ServiceCatId=',cat.CatgID,
						   '&SubCatId=',Subcat.SubCatgID,'&EnvironmentType=',@EnvironmentType,'&TicketSubStatus=',ssisTable.SubStatus) as Url
                     FROM dbo.TicketsViaSSIS ssisTable WITH (NOLOCK)
                     JOIN dbo.HROCRoster hroc WITH (NOLOCK)
                     ON hroc.[Employee 3-4 ID (Lower Case)] = Substring(ssisTable.CloseUserID,1,7)
                     JOIN dbo.Category cat WITH (NOLOCK)
                     ON hroc.[Job Cd Desc - Home Curr] = cat.CatgDescription
                     JOIN dbo.SubCategory Subcat WITH (NOLOCK)
                     ON Subcat.SubCatgDescription = ssisTable.SubCategory
                     and Subcat.CatgID = cat.CatgID
					 LEFT JOIN dbo.AuditMain main with (nolock)
					 on main.TicketID = ssisTable.TicketCode
					 and main.AuditType = @EnvironmentType
					 and main.TicketDate = ssisTable.ClosedDate
					 and main.SubcategoryID = Subcat.SubCatgID
                     WHERE 1=1
                     
                     and CAST(ssisTable.ClosedDate as date) between 
                     CASE WHEN (ISNULL(@FromDate,'')) = '1900-01-01' THEN '1900-01-01'  ELSE CAST(@FromDate as date) END
                     and 
                     CASE WHEN (ISNULL(@ToDate,'')) = '1900-01-01' THEN '3000-01-01'  ELSE CAST(@ToDate as date) END
                     
                     and hroc.[Employee 3-4 ID (Lower Case)] like CONCAT(@AssignedTo,'%')
                     
                     and cat.CatgID = CASE WHEN (ISNULL(@CategoryId,'')) = 0 THEN cat.CatgID ELSE @CategoryId END
                     
                     AND ISNULL(Subcat.SubCatgID,'') = CASE WHEN (ISNULL(@SubCategoryId,'')) = 0 THEN ISNULL(Subcat.SubCatgID,'') ELSE @SubCategoryId END 
                     
                     and TicketStatus = CASE WHEN ISNULL(@TicketStatus,'') = ''  THEN TicketStatus ELSE @TicketStatus END 
					 and TicketCode = CASE WHEN ISNULL(@TicketId,'') = ''  THEN TicketCode ELSE @TicketId END 
					 and main.TicketID is null
              END

              IF (@TicketStatus = 1)
              BEGIN
                     SELECT 
                           ssisTable.TicketCode As TicketCode,
                           cat.CatgDescription as ServiceDeliveryGroup,
                           ssisTable.Category as Category,
                           ssisTable.SubCategory as SubCategory,
						   Subcat.SubCatgID as SubCategoryId,
						   cat.CatgID as CategoryId,
						   ssisTable.TicketStatus as TicketStatus,
						   ssisTable.SubStatus as SubStatus,
                           UPPER(hroc.[Employee 3-4 ID (Lower Case)]) as Agent34ID,
						   CONCAT(hroc.[Supervisor First Name], ' ', hroc.[Supervisor Last Name]) as SupervisorName,
                           hroc.[Employee Full Name] as EmployeeName,
                           convert(varchar, ssisTable.CreateDate, 101) as ClosedDate,
                           ssisTable.Topic as Topic,
						   CONCAT('/Audit/Index?TicketId=',ssisTable.TicketCode,'&TicketStatus=',
						   @TicketStatus,'&TicketDate=',convert(varchar, ssisTable.CreateDate, 101),'&ServiceCatId=',cat.CatgID,
						   '&SubCatId=',Subcat.SubCatgID,'&EnvironmentType=',@EnvironmentType,'&TicketSubStatus=',ssisTable.SubStatus) as Url
                     FROM dbo.TicketsViaSSIS ssisTable WITH (NOLOCK)
                     JOIN dbo.HROCRoster hroc WITH (NOLOCK)
                     ON hroc.[Employee 3-4 ID (Lower Case)] = Substring(ssisTable.CloseUserID,1,7)
                     JOIN dbo.Category cat WITH (NOLOCK)
                     ON hroc.[Job Cd Desc - Home Curr] = cat.CatgDescription
                     JOIN dbo.SubCategory Subcat WITH (NOLOCK)
                     ON Subcat.SubCatgDescription = ssisTable.SubCategory
                     and Subcat.CatgID = cat.CatgID
                     LEFT JOIN dbo.AuditMain main with (nolock)
					 on main.TicketID = ssisTable.TicketCode
					 and main.AuditType = @EnvironmentType
					 and main.TicketDate = ssisTable.ClosedDate
					 and main.SubcategoryID = Subcat.SubCatgID
                     WHERE 1=1
                     
                     and CAST(ssisTable.CreateDate as date) between 
                     CASE WHEN (ISNULL(@FromDate,'')) = '1900-01-01' THEN '1900-01-01'  ELSE CAST(@FromDate as date) END
                     and 
                     CASE WHEN (ISNULL(@ToDate,'')) = '1900-01-01' THEN '3000-01-01'  ELSE CAST(@ToDate as date) END
                     
                     and hroc.[Employee 3-4 ID (Lower Case)] like CONCAT(@AssignedTo,'%')
                     
                     and cat.CatgID = CASE WHEN (ISNULL(@CategoryId,'')) = 0 THEN cat.CatgID ELSE @CategoryId END
                     
                     AND ISNULL(Subcat.SubCatgID,'') = CASE WHEN (ISNULL(@SubCategoryId,'')) = 0 THEN ISNULL(Subcat.SubCatgID,'') ELSE @SubCategoryId END 
                     
                     and TicketStatus = CASE WHEN ISNULL(@TicketStatus,'') = ''  THEN TicketStatus ELSE @TicketStatus END
					 
					 and SubStatus = CASE WHEN ISNULL(@TicketSubStatus,'') = ''  THEN SubStatus ELSE @TicketSubStatus END 
					 and TicketCode = CASE WHEN ISNULL(@TicketId,'') = ''  THEN TicketCode ELSE @TicketId END 
					 and main.TicketID is null
              END
	END

	IF (@ResultType = 'Dispute')
       BEGIN
			SELECT
				auditMain.TicketID As TicketCode,
                cat.CatgDescription as ServiceDeliveryGroup,
                '' as Category,
                Subcat.SubCatgDescription as SubCategory,
				Subcat.SubCatgID as SubCategoryId,
				cat.CatgID as CategoryId,
				cat.CatgDescription as TicketStatus,
				'' as SubStatus,
                UPPER(auditMain.Agent34ID) as Agent34ID,
				CONCAT(hroc.[Supervisor First Name], ' ', hroc.[Supervisor Last Name]) as SupervisorName,
                hroc.[Employee Full Name] as EmployeeName,
                convert(varchar, auditMain.SubmitDT, 101) as ClosedDate,
                '' as Topic,
				CONCAT('/Dispute/Index?AuditMainId=',auditMain.ID) as Url
			FROM dbo.AuditMain auditMain WITH (NOLOCK)
				JOIN dbo.HROCRoster hroc WITH (NOLOCK)
				ON hroc.[Employee 3-4 ID (Lower Case)] = auditMain.Agent34ID
                JOIN dbo.Category cat WITH (NOLOCK)
                ON auditMain.ServiceGroupID = cat.CatgID
                JOIN dbo.AuditMainResponse resp WITH (NOLOCK)
                ON resp.AuditMainID = auditMain.ID
                JOIN dbo.SubCategory Subcat WITH (NOLOCK)
                ON Subcat.SubCatgID = auditMain.SubcategoryID
                and Subcat.CatgID = cat.CatgID
				WHERE auditMain.DisputeDate is NULL
					and CAST(auditMain.SubmitDT as date) between 
                    CASE WHEN (ISNULL(@FromDate,'')) = '1900-01-01' THEN '1900-01-01'  ELSE CAST(@FromDate as date) END
                    and 
                    CASE WHEN (ISNULL(@ToDate,'')) = '1900-01-01' THEN '3000-01-01'  ELSE CAST(@ToDate as date) END
                    
                    and auditMain.Agent34ID like CONCAT(@AssignedTo,'%')
                    
                    and cat.CatgID = CASE WHEN (ISNULL(@CategoryId,'')) = 0 THEN cat.CatgID ELSE @CategoryId END
                    
                    AND ISNULL(Subcat.SubCatgID,'') = CASE WHEN (ISNULL(@SubCategoryId,'')) = 0 THEN ISNULL(Subcat.SubCatgID,'') ELSE @SubCategoryId END 
                    
                    and AuditType = CASE WHEN ISNULL(@EnvironmentType,'') = ''  THEN AuditType ELSE @EnvironmentType END 

					and auditMain.TicketID = CASE WHEN ISNULL(@TicketId,'') = ''  THEN auditMain.TicketID ELSE @TicketId END 

					and resp.isNonCompliant = 1
			GROUP BY 
				auditMain.TicketID,
                cat.CatgDescription,
                Subcat.SubCatgDescription,
				Subcat.SubCatgID,
				cat.CatgID,
				cat.CatgDescription,
                UPPER(auditMain.Agent34ID),
				CONCAT(hroc.[Supervisor First Name], ' ', hroc.[Supervisor Last Name]),
                hroc.[Employee Full Name],
                convert(varchar, auditMain.SubmitDT, 101),
				CONCAT('/Dispute/Index?AuditMainId=',auditMain.ID)
		END
end
GO

GRANT EXECUTE ON [dbo].[usp_GetHRAuditSearchResult] TO public
