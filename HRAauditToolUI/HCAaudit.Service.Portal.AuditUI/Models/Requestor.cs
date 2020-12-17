using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class Requestor
    {
        /// <summary>
        /// Npi
        /// </summary>
        public string Npi { get; set; }

        /// <summary>
        /// List of Array DirectAddress
        /// </summary>
        public List<string> DirectAddress { get; set; }

        /// <summary>
        /// RequestorType
        /// </summary>
        public string RequestorType { get; set; }

        /// <summary>
        /// Requestor Full Name
        /// </summary>
        [Required]
        public string RequestorFullName { get; set; }

        /// <summary>
        /// Requestor Organization
        /// </summary>
        public string RequestorOrganization { get; set; }

        public Requestor()
        {
            DirectAddress = new List<string>();
        }
  
    }
}
