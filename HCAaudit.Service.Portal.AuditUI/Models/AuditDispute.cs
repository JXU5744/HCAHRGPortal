using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class AuditDispute
    {
        public int Id { get; set; }
        public int AuditMainId { get; set; }
        public string TicketId { get; set; }
        public int QuestionId { get; set; }
        public int? QuestionRank { get; set; }
        public int? GracePeriodId { get; set; }
        public int? OverTurnId { get; set; }
        public bool? IsDowngraded { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual AuditMain AuditMain { get; set; }
        public virtual QuestionBank Question { get; set; }
    }
}
