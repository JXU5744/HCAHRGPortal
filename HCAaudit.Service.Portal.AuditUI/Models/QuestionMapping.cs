using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
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

        public virtual QuestionBank Question { get; set; }
        public virtual SubCategory SubCatg { get; set; }
    }
}
