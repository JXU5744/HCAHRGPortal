using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class Category
    {
        public Category()
        {
            AuditMains = new HashSet<AuditMain>();
            HrocrosterCategories = new HashSet<HrocrosterCategory>();
            SubCategories = new HashSet<SubCategory>();
        }

        public int CatgId { get; set; }
        public string CatgDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<AuditMain> AuditMains { get; set; }
        public virtual ICollection<HrocrosterCategory> HrocrosterCategories { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
