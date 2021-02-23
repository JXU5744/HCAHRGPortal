using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class AuditMain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TicketID { get; set; }
        public DateTime TicketDate { get; set; }
        public string AgentName { get; set; }
        public string Agent34ID { get; set; }
        public string AuditorName { get; set; }
        public DateTime? SubmitDT { get; set; }
        public int SubcategoryID { get; set; }
        public int ServiceGroupID { get; set; }
        public bool isDisputed { get; set; }
        public DateTime? DisputeDate { get; set; }
        public string DisputeAuditor34ID { get; set; }
        public string AuditorQuit { get; set; }
        public string AuditorQuitReason { get; set; }
        public string AuditType { get; set; }
        public string AuditNotes { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

}
