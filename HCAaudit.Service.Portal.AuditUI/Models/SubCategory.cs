using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            AuditMains = new HashSet<AuditMain>();
            QuestionMappings = new HashSet<QuestionMapping>();
        }

        public int SubCatgId { get; set; }
        public int CatgId { get; set; }
        public string SubCatgDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Category Catg { get; set; }
        public virtual ICollection<AuditMain> AuditMains { get; set; }
        public virtual ICollection<QuestionMapping> QuestionMappings { get; set; }
    }
}
