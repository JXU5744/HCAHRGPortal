using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class QuestionBank
    {
        public QuestionBank()
        {
            AuditDisputes = new HashSet<AuditDispute>();
            AuditMainResponses = new HashSet<AuditMainResponse>();
            QuestionMappings = new HashSet<QuestionMapping>();
        }

        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<AuditDispute> AuditDisputes { get; set; }
        public virtual ICollection<AuditMainResponse> AuditMainResponses { get; set; }
        public virtual ICollection<QuestionMapping> QuestionMappings { get; set; }
    }
}
