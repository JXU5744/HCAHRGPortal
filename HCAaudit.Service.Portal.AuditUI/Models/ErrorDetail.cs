using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    [Table("HRAuditErrorLog")]
    public partial class ErrorDetail
    {
        [Key]
        public int HRAuditErrorLogId { get; set; }
        public string ErrorType { get; set; }
        public string SourceLocation { get; set; }
        public string ErrorDescription { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
