using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class UserRequestResponse
    {
        /// <summary>
        /// Flag indicating if the process was successful
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// FailedRequestId
        /// </summary>
        public string FailedRequestId { get; set; }
        /// <summary>
        /// Execption
        /// </summary>
        public Exception Exception { get; set; }
    }
}
