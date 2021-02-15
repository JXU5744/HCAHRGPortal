using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class AuditDispute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int AuditMainID { get; set; }
        public string TicketID { get; set; }
        public int QuestionId { get; set; }
        public int QuestionRank { get; set; }
        public int GracePeriodId { get; set; }
        public int OverTurnId { get; set; } 
        public string Comments { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

}
