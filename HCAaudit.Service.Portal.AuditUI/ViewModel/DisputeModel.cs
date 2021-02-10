using HCAaudit.Service.Portal.AuditUI.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class DisputeModel
    {
        public AuditMain AuditMain = new AuditMain();
        public List<AuditNonComplianceModel> AuditNonComplianceModel = new List<AuditNonComplianceModel>();
        public string ServiceDeliveryGroupName { get; set; }
        public string SubCategoryName { get; set; }
        public IEnumerable<SelectListItem> GracePeriod { get; set; }
        public IEnumerable<SelectListItem> Overturn { get; set; }
    }

    public class AuditNonComplianceModel
    {
        public int QuestionId { get; set; }
        public String Question { get; set; }
        public string TicketId { get; set; }
        public int QuestionRank { get; set; }
        public bool IsCompliant { get; set; }
        public bool IsNonCompliant { get; set; }
        public bool IsCorrectionRequired { get; set; }
        public string NonComplianceComments { get; set; }
        public string GracePeriodId { get; set; }
        public string OverturnId { get; set; }
        public String Comment { get; set; }
    }
}
