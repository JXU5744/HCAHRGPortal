using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HCAaudit.Service.Portal.AuditUI.Models
{
    public partial class AuditToolContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public AuditToolContext()
        {
        }

        public AuditToolContext(DbContextOptions<AuditToolContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
            this.Database.SetCommandTimeout(300);
        }

        public DbSet<UspGetHRAuditSearchResult> UspGetHRAuditSearchResult { get; set; }
        public virtual DbSet<AuditDispute> AuditDispute { get; set; }
        public virtual DbSet<AuditMain> AuditMain { get; set; }
        public virtual DbSet<AuditMainResponse> AuditMainResponse { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<HRAuditErrorLog> HRAuditErrorLog { get; set; }
        public virtual DbSet<HROCRoster> HROCRoster { get; set; }
        public virtual DbSet<HrocrosterCategory> HrocrosterCategories { get; set; }

        public virtual DbSet<ListOfValue> ListOfValue { get; set; }
        public virtual DbSet<QuestionBank> QuestionBank { get; set; }
        public virtual DbSet<QuestionMapping> QuestionMapping { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<TicketsViaSSIS> TicketsViaSSIS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                SqlConnection connection = new SqlConnection
                {
                    ConnectionString = Configuration["HRAuditDatabaseConnectionString"]
                };

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditDispute>(entity =>
            {
                entity.ToTable("AuditDispute");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuditMainId).HasColumnName("AuditMainID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDowngraded)
                    .HasColumnName("isDowngraded")
                    .HasDefaultValueSql("((0))");
                
                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TicketId)
                    .HasMaxLength(255)
                    .HasColumnName("TicketID");

                entity.HasOne(d => d.AuditMain)
                    .WithMany(p => p.AuditDisputes)
                    .HasForeignKey(d => d.AuditMainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditDisp__Audit__123EB7A3");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AuditDisputes)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditDisp__Quest__0F624AF8");
            });

            modelBuilder.Entity<AuditMain>(entity =>
            {
                entity.ToTable("AuditMain");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Agent34Id)
                    .HasMaxLength(255)
                    .HasColumnName("Agent34ID");

                entity.Property(e => e.AgentName).HasMaxLength(255);

                entity.Property(e => e.AuditType).HasMaxLength(255);

                entity.Property(e => e.AuditorName).HasMaxLength(255);

                entity.Property(e => e.AuditorQuit).HasMaxLength(255);

                entity.Property(e => e.AuditorQuitReason).HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisputeAuditor34Id)
                    .HasMaxLength(255)
                    .HasColumnName("DisputeAuditor34ID");

                entity.Property(e => e.DisputeDate).HasColumnType("datetime");

                entity.Property(e => e.IsDisputed).HasColumnName("isDisputed");

                entity.Property(e => e.IsEscalated).HasDefaultValueSql("((0))");
                
                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ServiceGroupId).HasColumnName("ServiceGroupID");

                entity.Property(e => e.SubcategoryId).HasColumnName("SubcategoryID");

                entity.Property(e => e.SubmitDt)
                    .HasColumnType("datetime")
                    .HasColumnName("SubmitDT");

                entity.Property(e => e.TicketDate).HasColumnType("datetime");

                entity.Property(e => e.TicketId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("TicketID");

                entity.HasOne(d => d.ServiceGroup)
                    .WithMany(p => p.AuditMains)
                    .HasForeignKey(d => d.ServiceGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditMain__Servi__6FE99F9F");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.AuditMains)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditMain__Subca__6EF57B66");
            });

            modelBuilder.Entity<AuditMainResponse>(entity =>
            {
                entity.ToTable("AuditMainResponse");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuditMainId).HasColumnName("AuditMainID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsCompliant).HasColumnName("isCompliant");

                entity.Property(e => e.IsCorrectionRequired).HasColumnName("isCorrectionRequired");

                entity.Property(e => e.IsHighNonComplianceImpact).HasColumnName("isHighNonComplianceImpact");

                entity.Property(e => e.IsLowNonComplianceImpact).HasColumnName("isLowNonComplianceImpact");

                entity.Property(e => e.IsNa).HasColumnName("isNA");

                entity.Property(e => e.IsNonCompliant).HasColumnName("isNonCompliant");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TicketId)
                    .HasMaxLength(255)
                    .HasColumnName("TicketID");

                entity.HasOne(d => d.AuditMain)
                    .WithMany(p => p.AuditMainResponses)
                    .HasForeignKey(d => d.AuditMainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditMain__Audit__17F790F9");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AuditMainResponses)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditMain__Quest__151B244E");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatgId)
                    .HasName("PK_CategoryID");

                entity.ToTable("Category");

                entity.Property(e => e.CatgId).HasColumnName("CatgID");

                entity.Property(e => e.CatgDescription)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HRAuditErrorLog>(entity =>
            {
                entity.ToTable("HRAuditErrorLog");

                entity.Property(e => e.HrauditErrorLogId).HasColumnName("HRAuditErrorLogId");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ErrorDescription)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SourceLocation)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HROCRoster>(entity =>
            {
                entity.ToTable("HROCRoster");

                entity.Property(e => e.HROCRosterId).HasColumnName("HROCRosterId");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateHired)
                    .HasColumnType("datetime")
                    .HasColumnName("Date Hired");

                entity.Property(e => e.Employee34IdLowerCase)
                    .HasMaxLength(255)
                    .HasColumnName("Employee 3-4 ID (Lower Case)");

                entity.Property(e => e.EmployeeFullName)
                    .HasMaxLength(255)
                    .HasColumnName("Employee Full Name");

                entity.Property(e => e.EmployeeNum).HasColumnName("Employee Num");

                entity.Property(e => e.EmployeeStatus)
                    .HasMaxLength(255)
                    .HasColumnName("Employee Status");

                entity.Property(e => e.EmployeeStatusDesc)
                    .HasMaxLength(255)
                    .HasColumnName("Employee Status Desc");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("First Name");

                entity.Property(e => e.JobCdDescHomeCurr)
                    .HasMaxLength(255)
                    .HasColumnName("Job Cd Desc - Home Curr");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .HasColumnName("Last Name");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PositionDescHomeCurr)
                    .HasMaxLength(255)
                    .HasColumnName("Position Desc - Home Curr");

                entity.Property(e => e.SupervisorFirstName)
                    .HasMaxLength(255)
                    .HasColumnName("Supervisor First Name");

                entity.Property(e => e.SupervisorLastName)
                    .HasMaxLength(255)
                    .HasColumnName("Supervisor Last Name");
            });

            modelBuilder.Entity<HrocrosterCategory>(entity =>
            {
                entity.ToTable("HROCRosterCategory");

                entity.Property(e => e.HrocrosterCategoryId).HasColumnName("HROCRosterCategoryID");

                entity.Property(e => e.CatgId).HasColumnName("CatgID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HrocrosterId).HasColumnName("HROCRosterId");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Catg)
                    .WithMany(p => p.HrocrosterCategories)
                    .HasForeignKey(d => d.CatgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HROCRoste__CatgI__5BAD9CC8");

                //entity.HasOne(d => d.Hrocroster)
                //    .WithMany(p => p.HrocrosterCategories)
                //    .HasForeignKey(d => d.HrocrosterId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__HROCRoste__HROCR__5CA1C101");
            });
            
            modelBuilder.Entity<ListOfValue>(entity =>
            {
                entity.ToTable("ListOfValues");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CodeType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__Question__0DC06F8CD8406397");

                entity.ToTable("QuestionBank");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuestionDescription).IsUnicode(false);

                entity.Property(e => e.QuestionName).IsUnicode(false);
            });

            modelBuilder.Entity<QuestionMapping>(entity =>
            {
                entity.ToTable("QuestionMapping");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionMappings)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__Quest__00200768");

                entity.HasOne(d => d.SubCatg)
                    .WithMany(p => p.QuestionMappings)
                    .HasForeignKey(d => d.SubCatgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionM__SubCa__7F2BE32F");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.SubCatgId)
                    .HasName("PK_SubCategoryid");

                entity.ToTable("SubCategory");

                entity.Property(e => e.SubCatgId).HasColumnName("SubCatgID");

                entity.Property(e => e.CatgId).HasColumnName("CatgID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubCatgDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Catg)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CatgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubCatego__CatgI__619B8048");
            });

            modelBuilder.Entity<TicketsViaSSIS>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TicketsViaSSIS");

                entity.HasIndex(e => new { e.SubCategory, e.TicketCode, e.ClosedDate }, "IX_Subcatg_TCode_CDate_TktVSSIS");

                entity.HasIndex(e => new { e.TicketCode, e.ClosedDate, e.SubCategory }, "Ix_TCode_CDate_SCategory_TktVSSIS")
                    .IsClustered();

                entity.Property(e => e.AboutEe)
                    .HasMaxLength(255)
                    .HasColumnName("AboutEE");

                entity.Property(e => e.CaseType).HasMaxLength(255);

                entity.Property(e => e.Category).HasMaxLength(255);

                entity.Property(e => e.ChatAgentUserId)
                    .HasMaxLength(255)
                    .HasColumnName("ChatAgentUserID");

                entity.Property(e => e.CloseUserId)
                    .HasMaxLength(255)
                    .HasColumnName("CloseUserID");

                entity.Property(e => e.ContactMethod).HasMaxLength(255);

                entity.Property(e => e.ContactName).HasMaxLength(255);

                entity.Property(e => e.ContactRelationshipName).HasMaxLength(255);

                entity.Property(e => e.CreatorFirstName).HasMaxLength(255);

                entity.Property(e => e.CreatorLastName).HasMaxLength(255);

                entity.Property(e => e.CreatorName).HasMaxLength(255);

                entity.Property(e => e.CreatorUserId)
                    .HasMaxLength(255)
                    .HasColumnName("CreatorUserID");

                entity.Property(e => e.CustomSelect1).HasMaxLength(255);

                entity.Property(e => e.CustomSelect2).HasMaxLength(255);

                entity.Property(e => e.CustomSelect3).HasMaxLength(255);

                entity.Property(e => e.CustomSelect4).HasMaxLength(255);

                entity.Property(e => e.CustomSelect5).HasMaxLength(255);

                entity.Property(e => e.CustomSelect6).HasMaxLength(255);

                entity.Property(e => e.CustomString1).HasMaxLength(255);

                entity.Property(e => e.CustomString2).HasMaxLength(255);

                entity.Property(e => e.CustomString3).HasMaxLength(255);

                entity.Property(e => e.CustomString4).HasMaxLength(255);

                entity.Property(e => e.CustomString5).HasMaxLength(255);

                entity.Property(e => e.CustomString6).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.IsFirstCallResolution).HasMaxLength(255);

                entity.Property(e => e.Issue).HasMaxLength(3000);

                entity.Property(e => e.KnowledgeDomain).HasMaxLength(255);

                entity.Property(e => e.LastEditUserId)
                    .HasMaxLength(255)
                    .HasColumnName("LastEditUserID");

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.OwnerUserId)
                    .HasMaxLength(255)
                    .HasColumnName("OwnerUserID");

                entity.Property(e => e.Population).HasMaxLength(255);

                entity.Property(e => e.Priority).HasMaxLength(255);

                entity.Property(e => e.RegardingUserId)
                    .HasMaxLength(255)
                    .HasColumnName("RegardingUserID");

                entity.Property(e => e.ReminderEmail).HasMaxLength(255);

                entity.Property(e => e.ReminderNote).HasMaxLength(1000);

                entity.Property(e => e.ReminderPhone).HasMaxLength(255);

                entity.Property(e => e.Resolution).HasMaxLength(3000);

                entity.Property(e => e.Secure).HasMaxLength(255);

                entity.Property(e => e.ServiceGroup).HasMaxLength(255);

                entity.Property(e => e.ShowToEe)
                    .HasMaxLength(255)
                    .HasColumnName("ShowToEE");

                entity.Property(e => e.Sla).HasColumnName("SLA");

                entity.Property(e => e.Sladate).HasColumnName("SLADate");

                entity.Property(e => e.Source).HasMaxLength(255);

                entity.Property(e => e.SourceFileStamp).HasMaxLength(255);

                entity.Property(e => e.SubCategory).HasMaxLength(255);

                entity.Property(e => e.SubStatus).HasMaxLength(255);

                entity.Property(e => e.Subject).HasMaxLength(1000);

                entity.Property(e => e.SurveyAgreementResponse).HasMaxLength(255);

                entity.Property(e => e.SurveyAnswer1).HasMaxLength(255);

                entity.Property(e => e.SurveyAnswer2).HasMaxLength(255);

                entity.Property(e => e.SurveyAnswer3).HasMaxLength(255);

                entity.Property(e => e.SurveyAnswer4).HasMaxLength(255);

                entity.Property(e => e.SurveyAnswer5).HasMaxLength(255);

                entity.Property(e => e.SurveyCommentResponse).HasMaxLength(1000);

                entity.Property(e => e.SurveyFollowup).HasMaxLength(255);

                entity.Property(e => e.SurveyId)
                    .HasMaxLength(255)
                    .HasColumnName("SurveyID");

                entity.Property(e => e.SurveyScore).HasMaxLength(255);

                entity.Property(e => e.TicketCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TicketStatus).HasMaxLength(255);

                entity.Property(e => e.Topic).HasColumnType("ntext");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
