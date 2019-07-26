using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EVF.Data.Pocos
{
    public partial class d9vs38qobc5nv6Context : DbContext
    {
        public d9vs38qobc5nv6Context()
        {
        }

        public d9vs38qobc5nv6Context(DbContextOptions<d9vs38qobc5nv6Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AppCompositeRole> AppCompositeRole { get; set; }
        public virtual DbSet<AppCompositeRoleItem> AppCompositeRoleItem { get; set; }
        public virtual DbSet<AppMenu> AppMenu { get; set; }
        public virtual DbSet<Hremployee> Hremployee { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AppMenu>(entity =>
            {
                entity.HasKey(e => e.MenuCode)
                    .HasName("AppMenu_pkey");

                entity.Property(e => e.MenuCode).ValueGeneratedNever();
            });

            modelBuilder.Entity<Hremployee>(entity =>
            {
                entity.HasKey(e => e.EmpNo)
                    .HasName("HREmployee_pkey");

                entity.Property(e => e.EmpNo).ValueGeneratedNever();
            });
        }
    }
}
