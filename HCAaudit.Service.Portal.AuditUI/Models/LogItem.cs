using System;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class LogItem
    {
        public Guid Id { get; set; }
        public string ErrorType { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorDiscription { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }


    }
}