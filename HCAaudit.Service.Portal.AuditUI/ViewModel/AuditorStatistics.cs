using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class AuditorStatistics
    {
        public AuditorStatistics() {
            RecentTicketLists = new List<RecentTicket>();
            }

        public String YearToDate { get; set; }
        public String MonthToDate { get; set; }
        public IEnumerable<RecentTicket> RecentTicketLists { get; set; }
    }

    public class RecentTicket
    {
        public String TicketCode { get; set; }
        public String Agent34Id { get; set; }
        public String CompliantNonCompliant { get; set; }
        public DateTime AuditDate { get; set; }
        public String Dispute { get; set; }
    }
}
