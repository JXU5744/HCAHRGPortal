using Microsoft.EntityFrameworkCore;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class AuditToolContext :DbContext
    {

        public AuditToolContext(DbContextOptions<AuditToolContext> options) : base(options) {
            this.Database.SetCommandTimeout(300);
        }

        public DbSet<Categorys> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<QuestionMaster> QuestionMasters { get; set; }
        public DbSet<clstbHROCRoster> HrocMaster { get; set; }

        public DbSet<tblQuestionBank> QuestionBank { get; set; }

        public DbSet<clstbHROCAuditor> HrocAuditors { get; set; }

        public DbSet<TicketsViaSSIS> SearchTicketDetail { get; set; }

        public DbSet<AuditMainResponse> AuditMainResponse { get; set; }

        public DbSet<AuditMain> AuditMain { get; set; }
        public DbSet<Usp_GetHRAuditSearchResult> Usp_GetHRAuditSearchResult { get; set; }

        public DbSet<AuditDispute> AuditDispute { get; set; }
        public DbSet<ListOfValues> ListOfValues { get; set; }

    }
    
}
