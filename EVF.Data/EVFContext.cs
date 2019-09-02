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
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<GradeItem> GradeItem { get; set; }
        public virtual DbSet<HolidayCalendar> HolidayCalendar { get; set; }
        public virtual DbSet<Hrcompany> Hrcompany { get; set; }
        public virtual DbSet<Hremployee> Hremployee { get; set; }
        public virtual DbSet<Hrorg> Hrorg { get; set; }
        public virtual DbSet<HrorgRelation> HrorgRelation { get; set; }
        public virtual DbSet<Hrposition> Hrposition { get; set; }
        public virtual DbSet<LevelPoint> LevelPoint { get; set; }
        public virtual DbSet<LevelPointItem> LevelPointItem { get; set; }
        public virtual DbSet<Performance> Performance { get; set; }
        public virtual DbSet<PerformanceGroup> PerformanceGroup { get; set; }
        public virtual DbSet<PerformanceGroupItem> PerformanceGroupItem { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<PeriodItem> PeriodItem { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<ValueHelp> ValueHelp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AppMenu>(entity =>
            {
                entity.HasKey(e => e.MenuCode)
                    .HasName("AppMenu_pkey");

                entity.Property(e => e.MenuCode).ValueGeneratedNever();
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

            modelBuilder.Entity<ValueHelp>(entity =>
            {
                entity.HasKey(e => new { e.ValueType, e.ValueKey })
                    .HasName("ValueHelp_pkey");
            });
        }
    }
}
