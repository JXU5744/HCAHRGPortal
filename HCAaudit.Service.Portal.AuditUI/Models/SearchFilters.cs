using System;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class SearchFilters
    {
        public string Environment { get; set; }

        // 1 for Audit and 2 for Dispute
        public int AuditOrDispute { get; set; }

        public int CategoryID { get; set; }

        public int SubcategoryID { get; set; }

        public string AssignedTo { get; set; }

        public string TicketStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
