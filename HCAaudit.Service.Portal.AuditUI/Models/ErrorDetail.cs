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
        private string _ErrorType;
        private string _SourceLocation;
        private string _ErrorDescription;

        [Key]
        public int HRAuditErrorLogId { get; set; }
        public string ErrorType {
            get
            {
               return _ErrorType;
            }
            set
            {
                _ErrorType = value.Length > 50? value.Substring(0,50):value;
            }
        }
        public string SourceLocation {
            get
            {
                return _SourceLocation;
            }
            set
            {
                _SourceLocation = value.Length > 200 ? value.Substring(0, 200) : value; ;
            }
        }
        public string ErrorDescription {
            get
            {
                return _ErrorDescription;
            }
            set
            {
                _ErrorDescription = value.Length > 2000 ? value.Substring(0, 2000) : value; ;
            }
        }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
