using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class QuestionConfigMappingJoinMast
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public int CatSubCatId { get; set; }
        public int QuestionSeqNumber { get; set; }
    }

    [Table("Usp_GetHRAuditSearchResult")]
    [Keyless]
    public class Usp_GetHRAuditSearchResult
    {
        public string TicketCode { get; set; }
        public string ServiceDeliveryGroup { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string TicketStatus { get; set; }
        public string SubStatus { get; set; }
        public string Agent34ID { get; set; }
        public string SupervisorName { get; set; }
        public string EmployeeName { get; set; }
        public string ClosedDate { get; set; }
        public string Topic { get; set; }
        public string Url { get; set; }
    }

    [Table("AuditMainResponse")]
    public class AuditMainResponse
    {
        [Key]
        public int Id { get; set; }

        public int AuditMainID { get; set; }

        public string TicketID { get; set; }

        public int QuestionId { get; set; }
        public int QuestionRank { get; set; }
        public bool isCompliant { get; set; }
        public bool isNonCompliant { get; set; }
        public bool isNA { get; set; }
        public bool isHighNonComplianceImpact { get; set; }
        public bool isLowNonComplianceImpact { get; set; }
        public bool isCorrectionRequired { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string NonComplianceComments { get; set; }

    }

    public class QuesBankMasterJoinMast
    {
        public int QuestionId { get; set; }
        public int SequenceNo { get; set; }
        public string QuestionText { get; set; }
        public string QuestionDesc { get; set; }
        public string QuestionPercentage { get; set; }
        public int SubCatID { get; set; }
        public int QuestionMasterId { get; set; }

        public int QuestionMappingId { get; set; }

        public bool? isActive { get; set; }

    }

    public class CatSubCatJoinMast
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public int SubCatgID { get; set; }
        public string SubCatgDescription { get; set; }
    }

    public class Tickets
    {
        public int TicketID { get; set; }
        public string Ticket { get; set; }
        public static List<Tickets> GetTickets()
        {
            List<Tickets> objticketsList = new List<Tickets> {
            new Tickets { TicketID = 1, Ticket = "ZYU126DG" },
    new Tickets { TicketID = 2, Ticket = "G2NY6Q9Z" },
    new Tickets { TicketID = 3, Ticket = "MXP97HD3" },
    new Tickets { TicketID = 4, Ticket = "JGBWSU6C" },
    new Tickets { TicketID = 5, Ticket = "KC9NDZ1E" }
        };
            return objticketsList;
        }
    }

    public class AssignedTo
    {
        public int memberID { get; set; }
        public string membername { get; set; }
        public static List<AssignedTo> GetAssignedTo()
        {
            List<AssignedTo> objticketsList = new List<AssignedTo> {
            new AssignedTo { memberID = 1, membername = "MadisonBoling" },
    new AssignedTo { memberID = 2, membername = "ATSEmail-Channel" },
    new AssignedTo { memberID = 3, membername = "NicoleDietz" },
    new AssignedTo { memberID = 4, membername = "AllaMaria" },
     new AssignedTo { memberID = 5, membername = "FelisaMccarver" }
        };
            return objticketsList;
        }
    }

    public class Status
    {
        public int StatusID { get; set; }
        public string Statusname { get; set; }
        public static List<Status> GetStatus()
        {
            List<Status> objticketsList = new List<Status> {
            new Status { StatusID = 1, Statusname = "Active" },
    new Status { StatusID = 2, Statusname = "Inactive" },
    new Status { StatusID = 3, Statusname = "outdated" }
        };
            return objticketsList;
        }
    }

    public class BindSearchGrid
    {
        public string TicketNumber { get; set; }
        public string CreatedDate { get; set; }
        public string status { get; set; }
        public string Subject { get; set; }
        public string ServiceGroup { get; set; }
        public string AssignedTo { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }
        public string UserThreeFourID { get; set; }
        public DateTime? ClosedDateTime { get; set; }
        public string Topic { get; set; }



        public List<BindSearchGrid> _dataforGrid { get; set; }
        public static List<BindSearchGrid> GetGridData()
        {
            List<BindSearchGrid> objgriddata = new List<BindSearchGrid> {
        new BindSearchGrid{ TicketNumber="ZYU126DG",CreatedDate=DateTime.Now.ToShortDateString(),status="Active",Subject="Onboarding Employees",ServiceGroup="OA",AssignedTo="Girish"},
        new BindSearchGrid{ TicketNumber="G2NY6Q9Z",CreatedDate=DateTime.Now.ToShortDateString(),status="Inactive",Subject="Z-Auto-Onboarding Confirm",ServiceGroup="WFA",AssignedTo="Manoj"},
        new BindSearchGrid{ TicketNumber="MXP97HD3",CreatedDate=DateTime.Now.ToShortDateString(),status="Resolved",Subject="License and Certification",ServiceGroup="RA",AssignedTo="Vishal"},
        new BindSearchGrid{ TicketNumber="JGBWSU6C",CreatedDate=DateTime.Now.ToShortDateString(),status="Resolved",Subject="HCA Rewards",ServiceGroup="OA",AssignedTo="Jawahar"},
        new BindSearchGrid{ TicketNumber="KC9NDZ1E",CreatedDate=DateTime.Now.ToShortDateString(),status="Active",Subject="Hiring Employees",ServiceGroup="RA",AssignedTo="Girish"}
        };
            return objgriddata;
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class CategoryMastR
    {
        public string CatgID { get; set; }
        public string CatgDescription { get; set; }
        //public List<Category> _categoryList { get; set; }
    }
    //[Table("SubCategory")]
    public class CategoryMast
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public bool IsActive { get; set; }

        public List<Category> _categoryList { get; set; }
    }

    [Table("Category")]
    public class Categorys
    {
        [Key]
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }


    public class clsGroupCatSubcat
    {
        public List<clsCatSubcat> _catSubCatGroup { get; set; }
    }
    public class clsCatSubcat
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public int SubCatID { get; set; }
        public string SubCatgDescription { get; set; }
    }

    public class Category
    {
        [Key]
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public int SubCatID { get; set; }
        public int QuestionId { get; set; }
        public List<QuestionMapping> _questionList { get; set; }
    }

    [Table("SubCategory")]
    public class SubCategory
    {
        [Key]
        public int SubCatgID { get; set; }
        public int CatgID { get; set; }
        public string SubCatgDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    //public class QuestionMaster
    //{

    //   // public string QuestionId { get; set; }
    //   // public int SubCatgID { get; set; }
    //   // public int CatgID { get; set; }

    //    public List<tbQuestionMaster> _questionMasterList { get; set; }
    //}

    //[Table("QuestionMaster")]
    //public class QuestionMaster
    //{
    //    [Key]
    //    public int QuestionMasterId { get; set; }
    //    public int QuestionId { get; set; }
    //    public int SeqNumber { get; set; }
    //    public int SubCatgID { get; set; }
    //    public string QuestionText { get; set; }
    //    public int QuestionScore { get; set; }
    //    public bool IsActive { get; set; }
    //}
    [Table("QuestionMapping")]
    public partial class QuestionMapping
    {
        public int QuestionMappingId { get; set; }
        public int SubCatgId { get; set; }
        public int QuestionId { get; set; }
        public int SeqNumber { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    [Table("QuestionBank")]
    public class QuestionBank
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    [Table("HROCRoster")]
    public class HROCRoster
    {
        [Key]
        [Column("HROCRosterId")]
        public int HROCRosterId { get; set; }

        [Column("Employee Num")]
        public double? EmployeeNumber { get; set; }
        [Column("Employee Full Name")]
        public string EmployeeFullName { get; set; }

        [Column("Last Name")]
        public string EmployeeLastName { get; set; }

        [Column("First Name")]
        public string EmployeeFirstName { get; set; }

        [Column("Supervisor Last Name")]
        public string? SupervisorLastName { get; set; }

        [Column("Supervisor First Name")]
        public string? SupervisorFirstName { get; set; }

        [Column("Employee 3-4 ID (Lower Case)")]
        public string? EmployeethreefourID { get; set; }

        [Column("Employee Status")]
        public string? EmployeeStatus { get; set; }

        [Column("Position Desc - Home Curr")]
        public string? PositionDesc { get; set; }

        [Column("Job Cd Desc - Home Curr")]
        public string? JobCDDesc { get; set; }

        [Column("Employee Status Desc")]
        public string? EmployeeStatusDesc { get; set; }

        [Column("Date Hired")]
        public DateTime? DateHired { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
    [Table("TicketsViaSSIS")]
    public partial class TicketsViaSSIS
    {
        [Key]
        public string TicketCode { get; set; }
        public DateTime LastEditDateTime { get; set; }
        public string AboutEe { get; set; }
        public bool? Archived { get; set; }
        public string CaseType { get; set; }
        public string Category { get; set; }
        public string ChatAgentUserId { get; set; }
        public DateTime? ClosedDateTime { get; set; }
        public string CloseUserId { get; set; }
        public string ContactMethod { get; set; }
        public string ContactName { get; set; }
        public string ContactRelationshipName { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public string CreatorName { get; set; }
        public string CreatorUserId { get; set; }
        public bool? CustomCheckBox1 { get; set; }
        public bool? CustomCheckBox2 { get; set; }
        public bool? CustomCheckBox3 { get; set; }
        public bool? CustomCheckBox4 { get; set; }
        public bool? CustomCheckBox5 { get; set; }
        public bool? CustomCheckBox6 { get; set; }
        public DateTime? CustomDate1 { get; set; }
        public DateTime? CustomDate2 { get; set; }
        public DateTime? CustomDate3 { get; set; }
        public DateTime? CustomDate4 { get; set; }
        public DateTime? CustomDate5 { get; set; }
        public DateTime? CustomDate6 { get; set; }
        public string CustomSelect1 { get; set; }
        public string CustomSelect2 { get; set; }
        public string CustomSelect3 { get; set; }
        public string CustomSelect4 { get; set; }
        public string CustomSelect5 { get; set; }
        public string CustomSelect6 { get; set; }
        public string CustomString1 { get; set; }
        public string CustomString2 { get; set; }
        public string CustomString3 { get; set; }
        public string CustomString4 { get; set; }
        public string CustomString5 { get; set; }
        public string CustomString6 { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string IsFirstCallResolution { get; set; }
        public string Issue { get; set; }
        public string KnowledgeDomain { get; set; }
        public string LastEditUserId { get; set; }
        public string LastName { get; set; }
        public string OwnerUserId { get; set; }
        public string Population { get; set; }
        public string Priority { get; set; }
        public short? ProcessTime { get; set; }
        public string RegardingUserId { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public string ReminderEmail { get; set; }
        public string ReminderNote { get; set; }
        public string ReminderPhone { get; set; }
        public string Resolution { get; set; }
        public DateTime? ResolvedDateTime { get; set; }
        public string Secure { get; set; }
        public string ServiceGroup { get; set; }
        public string ShowToEe { get; set; }
        public DateTime? Sla { get; set; }
        public string Source { get; set; }
        public string SubCategory { get; set; }
        public string Subject { get; set; }
        public string SubStatus { get; set; }
        public string SurveyAgreementResponse { get; set; }
        public string SurveyAnswer1 { get; set; }
        public string SurveyAnswer2 { get; set; }
        public string SurveyAnswer3 { get; set; }
        public string SurveyAnswer4 { get; set; }
        public string SurveyAnswer5 { get; set; }
        public string SurveyCommentResponse { get; set; }
        public DateTime? SurveyDateTime { get; set; }
        public string SurveyFollowup { get; set; }
        public string SurveyId { get; set; }
        public string SurveyScore { get; set; }
        public string TicketStatus { get; set; }
        public string Topic { get; set; }
        public string UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? Sladate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? LoadStamp { get; set; }
        public string SourceFileStamp { get; set; }
    }

}