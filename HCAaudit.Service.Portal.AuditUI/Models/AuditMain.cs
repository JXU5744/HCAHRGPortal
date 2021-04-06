using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class AuditMain
    {
        public AuditMain()
        {
            AuditDisputes = new HashSet<AuditDispute>();
            AuditMainResponses = new HashSet<AuditMainResponse>();
        }

        public int Id { get; set; }
        public string TicketId { get; set; }
        public DateTime TicketDate { get; set; }
        public string AgentName { get; set; }
        public string Agent34Id { get; set; }
        public string AuditorName { get; set; }
        public DateTime? SubmitDt { get; set; }
        public int SubcategoryId { get; set; }
        public int ServiceGroupId { get; set; }
        public bool? IsDisputed { get; set; }
        public DateTime? DisputeDate { get; set; }
        public string DisputeAuditor34Id { get; set; }
        public string AuditorQuit { get; set; }
        public string AuditorQuitReason { get; set; }
        public string AuditType { get; set; }
        public string AuditNotes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsEscalated { get; set; }

        public virtual Category ServiceGroup { get; set; }
        public virtual SubCategory Subcategory { get; set; }
        public virtual ICollection<AuditDispute> AuditDisputes { get; set; }
        public virtual ICollection<AuditMainResponse> AuditMainResponses { get; set; }
    }
}
