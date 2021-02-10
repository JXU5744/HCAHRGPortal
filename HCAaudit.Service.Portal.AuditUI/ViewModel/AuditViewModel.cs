using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAaudit.Service.Portal.AuditUI.ViewModel
{
    public class AuditViewModel
    {
        public string TicketId { get; set; }
        public string AgentName { get; set; }
        public string Agent34Id { get; set; }
        public string AuditorName { get; set; }
        public string SubCatName { get; set; }
        public string SupervisorName { get; set; }
        public int ServiceCatId { get; set; }
        public int TicketSubStatus { get; set; }
        public int TicketStatus { get; set; }

        public DateTime TicketDate { get; set; }
        public int SubCatId { get; set; }
        public string ServiceGroupName { get; set; }
        public bool isDisputed { get; set; }
        public DateTime DisputedDate { get; set; }
        public string AuditorQuit { get; set; }
        public string AuditorQuitReason { get; set; }
        public string EnvironmentType { get; set; }
        public string AuditNote { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DateOfAuditor { get; set; }
        public List<Question> Question { get; set; }
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public int QuestionSequence { get; set; }
        public string QuestionName { get; set; }
        public string QuestionDescription { get; set; }
        public Action Action { get; set; }
        public bool CorrectionRequire { get; set; }
        public string Comments { get; set; }
    }
    public class Action
    {
        public bool IsCompliance { get; set; }
        public bool IsNonCompliance { get; set; }
        public bool IsNotApplicable { get; set; }
        public Impact Impact { get; set; }
    }
    public class Impact
    {
        public bool IsHighImpact { get; set; }
        public bool IsLowImpact { get; set; }
        
    }
}
