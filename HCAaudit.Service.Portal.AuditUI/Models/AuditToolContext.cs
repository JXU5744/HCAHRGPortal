using Microsoft.EntityFrameworkCore;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public class AuditToolContext :DbContext
    {

        public AuditToolContext(DbContextOptions<AuditToolContext> options) : base(options) { }

        public DbSet<Categorys> categories { get; set; }
        public DbSet<SubCategory> subCategories { get; set; }
        public DbSet<tbQuestionMaster> questionMasters { get; set; }
        public DbSet<clstbHROCRoster> hrocMaster { get; set; }

        public DbSet<tblQuestionBank> questionBank { get; set; }

        public DbSet<clstbHROCAuditor> hrocAuditors { get; set; }
        

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"data source=(localdb)\\MSSQLLocalDB; initial catalog=HRAudit;persist security info=True;user id=sa");
        //}
    }
}
