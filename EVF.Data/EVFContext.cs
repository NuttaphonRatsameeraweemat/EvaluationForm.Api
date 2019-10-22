using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EVF.Data.Pocos;

namespace EVF.Data
{
    public partial class EVFContext : DbContext
    {
        public EVFContext()
        {
        }

        public EVFContext(DbContextOptions<EVFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppCompositeRole> AppCompositeRole { get; set; }
        public virtual DbSet<AppCompositeRoleItem> AppCompositeRoleItem { get; set; }
        public virtual DbSet<AppMenu> AppMenu { get; set; }
        public virtual DbSet<Approval> Approval { get; set; }
        public virtual DbSet<ApprovalItem> ApprovalItem { get; set; }
        public virtual DbSet<AuthorityCompany> AuthorityCompany { get; set; }
        public virtual DbSet<Criteria> Criteria { get; set; }
        public virtual DbSet<CriteriaGroup> CriteriaGroup { get; set; }
        public virtual DbSet<CriteriaItem> CriteriaItem { get; set; }
        public virtual DbSet<EmailTask> EmailTask { get; set; }
        public virtual DbSet<EmailTaskContent> EmailTaskContent { get; set; }
        public virtual DbSet<EmailTaskReceiver> EmailTaskReceiver { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Evaluation> Evaluation { get; set; }
        public virtual DbSet<EvaluationAssign> EvaluationAssign { get; set; }
        public virtual DbSet<EvaluationLog> EvaluationLog { get; set; }
        public virtual DbSet<EvaluationLogItem> EvaluationLogItem { get; set; }
        public virtual DbSet<EvaluationPercentageConfig> EvaluationPercentageConfig { get; set; }
        public virtual DbSet<EvaluationSapResult> EvaluationSapResult { get; set; }
        public virtual DbSet<EvaluationTemplate> EvaluationTemplate { get; set; }
        public virtual DbSet<EvaluatorGroup> EvaluatorGroup { get; set; }
        public virtual DbSet<EvaluatorGroupItem> EvaluatorGroupItem { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<GradeItem> GradeItem { get; set; }
        public virtual DbSet<HolidayCalendar> HolidayCalendar { get; set; }
        public virtual DbSet<Hrcompany> Hrcompany { get; set; }
        public virtual DbSet<Hremployee> Hremployee { get; set; }
        public virtual DbSet<Hrorg> Hrorg { get; set; }
        public virtual DbSet<HrorgRelation> HrorgRelation { get; set; }
        public virtual DbSet<Hrposition> Hrposition { get; set; }
        public virtual DbSet<Kpi> Kpi { get; set; }
        public virtual DbSet<KpiGroup> KpiGroup { get; set; }
        public virtual DbSet<KpiGroupItem> KpiGroupItem { get; set; }
        public virtual DbSet<LevelPoint> LevelPoint { get; set; }
        public virtual DbSet<LevelPointItem> LevelPointItem { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<PeriodItem> PeriodItem { get; set; }
        public virtual DbSet<PurGroupWeightingKey> PurGroupWeightingKey { get; set; }
        public virtual DbSet<PurchaseOrg> PurchaseOrg { get; set; }
        public virtual DbSet<PurchaseOrgItem> PurchaseOrgItem { get; set; }
        public virtual DbSet<SapFields> SapFields { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<ValueHelp> ValueHelp { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<VendorFilter> VendorFilter { get; set; }
        public virtual DbSet<VendorTransection> VendorTransection { get; set; }
        public virtual DbSet<WorkflowActivityLog> WorkflowActivityLog { get; set; }
        public virtual DbSet<WorkflowActivityStep> WorkflowActivityStep { get; set; }
        public virtual DbSet<WorkflowDelegate> WorkflowDelegate { get; set; }
        public virtual DbSet<WorkflowProcess> WorkflowProcess { get; set; }
        public virtual DbSet<WorkflowProcessInstance> WorkflowProcessInstance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AppMenu>(entity =>
            {
                entity.HasKey(e => e.MenuCode)
                    .HasName("AppMenu_pkey");

                entity.Property(e => e.MenuCode).ValueGeneratedNever();
            });

            modelBuilder.Entity<EmailTemplate>(entity =>
            {
                entity.Property(e => e.EmailType).ValueGeneratedNever();
            });

            modelBuilder.Entity<EvaluationSapResult>(entity =>
            {
                entity.HasKey(e => new { e.ComCode, e.PurOrg, e.Vendor, e.YearMonth, e.WeightKey })
                    .HasName("PK__Evaluati__123CF657B3391254");

                entity.Property(e => e.ComCode).IsUnicode(false);

                entity.Property(e => e.PurOrg).IsUnicode(false);

                entity.Property(e => e.Vendor).IsUnicode(false);

                entity.Property(e => e.YearMonth).IsUnicode(false);

                entity.Property(e => e.WeightKey).IsUnicode(false);
            });

            modelBuilder.Entity<Hrcompany>(entity =>
            {
                entity.HasKey(e => e.ComCode)
                    .HasName("HRCompany_pkey");

                entity.Property(e => e.ComCode).ValueGeneratedNever();
            });

            modelBuilder.Entity<Hremployee>(entity =>
            {
                entity.HasKey(e => e.EmpNo)
                    .HasName("HREmployee_pkey");

                entity.Property(e => e.EmpNo).ValueGeneratedNever();
            });

            modelBuilder.Entity<Hrorg>(entity =>
            {
                entity.HasKey(e => e.OrgId)
                    .HasName("HROrg_pkey");

                entity.Property(e => e.OrgId).ValueGeneratedNever();
            });

            modelBuilder.Entity<HrorgRelation>(entity =>
            {
                entity.HasKey(e => new { e.ParentOrgId, e.ChildOrgId })
                    .HasName("HROrgRelation_pkey");
            });

            modelBuilder.Entity<Hrposition>(entity =>
            {
                entity.HasKey(e => e.PosId)
                    .HasName("HRPosition_pkey");

                entity.Property(e => e.PosId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PurGroupWeightingKey>(entity =>
            {
                entity.HasKey(e => new { e.PurGroup, e.WeightingKey })
                    .HasName("PK__PurGroup__2B0B1E1E853E2023");
            });

            modelBuilder.Entity<PurchaseOrg>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrg1)
                    .HasName("PK__Purchase__59DCBB309AE37431");

                entity.Property(e => e.PurchaseOrg1).ValueGeneratedNever();
            });

            modelBuilder.Entity<PurchaseOrgItem>(entity =>
            {
                entity.HasKey(e => new { e.PuchaseOrg, e.AdUser })
                    .HasName("PK__Purchase__A58DDCA0F9A98ACA");
            });

            modelBuilder.Entity<ValueHelp>(entity =>
            {
                entity.HasKey(e => new { e.ValueType, e.ValueKey })
                    .HasName("ValueHelp_pkey");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VendorNo)
                    .HasName("PK__Vendor__FC8600A8EBBBDA33");

                entity.Property(e => e.VendorNo).ValueGeneratedNever();
            });

            modelBuilder.Entity<VendorTransection>(entity =>
            {
                entity.Property(e => e.Condition).IsUnicode(false);

                entity.Property(e => e.Datatype).IsUnicode(false);

                entity.Property(e => e.DocNumber).IsUnicode(false);

                entity.Property(e => e.DocType).IsUnicode(false);

                entity.Property(e => e.Free).IsUnicode(false);

                entity.Property(e => e.Intercomp).IsUnicode(false);

                entity.Property(e => e.K2Key).IsUnicode(false);

                entity.Property(e => e.LineId).IsUnicode(false);

                entity.Property(e => e.MaterialCode).IsUnicode(false);

                entity.Property(e => e.MaterialGrp).IsUnicode(false);

                entity.Property(e => e.PurgropCode).IsUnicode(false);

                entity.Property(e => e.PurorgCode).IsUnicode(false);

                entity.Property(e => e.PurorgName).IsUnicode(false);

                entity.Property(e => e.UnitCode).IsUnicode(false);

                entity.Property(e => e.Vendor).IsUnicode(false);
            });

            modelBuilder.Entity<WorkflowActivityLog>(entity =>
            {
                entity.HasKey(e => new { e.ProcessInstanceId, e.SerialNo });
            });

            modelBuilder.Entity<WorkflowActivityStep>(entity =>
            {
                entity.HasKey(e => new { e.ProcessInstanceId, e.Step, e.ActionUser });
            });

            modelBuilder.Entity<WorkflowProcess>(entity =>
            {
                entity.Property(e => e.ProcessCode).ValueGeneratedNever();
            });

            modelBuilder.Entity<WorkflowProcessInstance>(entity =>
            {
                entity.Property(e => e.ProcessInstanceId).ValueGeneratedNever();
            });

        }
    }
}
