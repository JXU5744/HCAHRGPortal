using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class HrocrosterCategory
    {
        public int HrocrosterCategoryId { get; set; }
        public int CatgId { get; set; }
        public int HrocrosterId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Category Catg { get; set; }
      //  public virtual Hrocroster Hrocroster { get; set; }
    }
}
