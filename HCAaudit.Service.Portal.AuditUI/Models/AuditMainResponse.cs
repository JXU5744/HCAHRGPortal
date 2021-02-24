using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class AuditMainResponse
    {
        public int Id { get; set; }
        public int AuditMainId { get; set; }
        public string TicketId { get; set; }
        public int QuestionId { get; set; }
        public int? QuestionRank { get; set; }
        public bool? IsCompliant { get; set; }
        public bool? IsNonCompliant { get; set; }
        public bool? IsNa { get; set; }
        public bool? IsHighNonComplianceImpact { get; set; }
        public bool? IsLowNonComplianceImpact { get; set; }
        public bool? IsCorrectionRequired { get; set; }
        public string NonComplianceComments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual AuditMain AuditMain { get; set; }
        public virtual QuestionBank Question { get; set; }
    }
}
