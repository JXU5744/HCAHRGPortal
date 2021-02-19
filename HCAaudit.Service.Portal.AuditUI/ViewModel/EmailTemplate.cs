using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class EmailTemplate
    {
        public string  SendTo { get; set; }
        public string SendFrom { get; set; }
        public string SendFromName { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
    }
}
