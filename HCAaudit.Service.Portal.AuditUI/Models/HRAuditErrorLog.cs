using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class HRAuditErrorLog
    {
        public int HrauditErrorLogId { get; set; }
        public string ErrorType { get; set; }
        public string SourceLocation { get; set; }
        public string ErrorDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
