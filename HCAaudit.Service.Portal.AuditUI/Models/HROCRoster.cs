using System;
using System.Collections.Generic;

#nullable disable

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class HROCRoster
    {
        public int HROCRosterId { get; set; }
        public string EmployeeFullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Employee34IdLowerCase { get; set; }
        public double? EmployeeNum { get; set; }
        public string SupervisorLastName { get; set; }
        public string SupervisorFirstName { get; set; }
        public DateTime? DateHired { get; set; }
        public string JobCdDescHomeCurr { get; set; }
        public string PositionDescHomeCurr { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmployeeStatusDesc { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
