using System;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class SearchViewModel
    {
        public string EnvironmentType { get; set; }
        public string TicketId { get; set; }
        public string ResultType { get; set; }
        public int CategoryID { get; set; }
        public int SubcategoryID { get; set; }
        public string AssignedTo { get; set; }
        public int TicketStatus { get; set; }
        public string TicketSubStatus { get; set; }
        public string ResultCountCriteria { get; set; }
        public String FromDate { get; set; }
        public String EndDate { get; set; }
    }
}
