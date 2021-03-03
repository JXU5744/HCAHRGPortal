using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

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
    public class UspGetHRAuditSearchResult
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
        public bool IsActive { get; set; }
    }

    public class CatSubCatJoinMast
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public int SubCatgID { get; set; }
        public string SubCatgDescription { get; set; }
    }


    public class AssignedTo
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
    }

    public class Status
    {
        public int StatusID { get; set; }
        public string Statusname { get; set; }
    }

    public class BindSearchGrid
    {
        public string TicketNumber { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string ServiceGroup { get; set; }
        public string AssignedTo { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string UserThreeFourID { get; set; }
        public DateTime? ClosedDateTime { get; set; }
        public string Topic { get; set; }
        public List<BindSearchGrid> DataforGrid { get; set; }
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
    }
    //[Table("SubCategory")]
    public class CategoryMast
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public bool IsActive { get; set; }
        public List<Category> CategoryList { get; set; }
    }

    public class ClsGroupCatSubcat
    {
        public List<ClsCatSubcat> CatSubCatGroup { get; set; }
    }
    public class ClsCatSubcat
    {
        public int CatgID { get; set; }
        public string CatgDescription { get; set; }
        public int SubCatID { get; set; }
        public string SubCatgDescription { get; set; }
    }
}