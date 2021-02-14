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
        public string? SupervisorLastName { get; set; }
        public string? SupervisorFirstName { get; set; }
        public string? EmployeethreefourID { get; set; }
        public string? EmployeeStatus { get; set; }
        public string? PositionDesc { get; set; }
        public string? JobCDDesc { get; set; }
        public string? EmployeeStatusDesc { get; set; }
        public DateTime? DateHired { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
