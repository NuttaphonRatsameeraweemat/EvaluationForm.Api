using EVF.Tranfer.Service.Data.Pocos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Data
{
    public partial class DMContext : DbContext
    {
        public DMContext()
        {
        }

        public DMContext(DbContextOptions<DMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ZSPE_02> ZSPE_02 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZSPE_02>(entity =>
            {
                entity.HasKey(e => new { e.ComCode, e.PurOrg, e.Vendor, e.YearMonth, e.WeightKey })
                    .HasName("ZSPE_02_pkey");

                entity.Property(e => e.ComCode).IsUnicode(false);

                entity.Property(e => e.PurOrg).IsUnicode(false);

                entity.Property(e => e.Vendor).IsUnicode(false);

                entity.Property(e => e.YearMonth).IsUnicode(false);

                entity.Property(e => e.WeightKey).IsUnicode(false);
            });
        }
    }
}
