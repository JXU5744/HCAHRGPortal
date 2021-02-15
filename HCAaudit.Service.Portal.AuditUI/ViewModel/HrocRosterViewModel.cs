using System;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class HROCRosterViewModel
    {
        public int HROCRosterId { get; set; }
        public double? EmployeeNumber { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeFirstName { get; set; }

#nullable enable
        public string? SupervisorLastName { get; set; }
#nullable enable
        public string? SupervisorFirstName { get; set; }
#nullable enable
        public string? EmployeethreefourID { get; set; }
#nullable enable
        public string? EmployeeStatus { get; set; }
#nullable enable
        public string? PositionDesc { get; set; }
#nullable enable
        public string? JobCDDesc { get; set; }
#nullable enable
        public string? EmployeeStatusDesc { get; set; }
#nullable enable
        public DateTime? DateHired { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
