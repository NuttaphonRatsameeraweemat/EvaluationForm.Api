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
        public virtual DbSet<ZNCR_03> ZNCR_03 { get; set; }

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

            modelBuilder.Entity<ZNCR_03>(entity =>
            {
                entity.Property(e => e.VendorNo)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.CountyDesc).IsUnicode(false);

                entity.Property(e => e.CountyKey).IsUnicode(false);

                entity.Property(e => e.CreateBy).IsUnicode(false);

                entity.Property(e => e.CreateDate).IsUnicode(false);

                entity.Property(e => e.CreateTime).IsUnicode(false);

                entity.Property(e => e.CustNo).IsUnicode(false);

                entity.Property(e => e.DelFlag).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FaxExt).IsUnicode(false);

                entity.Property(e => e.FaxNo).IsUnicode(false);

                entity.Property(e => e.LanguageKey).IsUnicode(false);

                entity.Property(e => e.MobileNo).IsUnicode(false);

                entity.Property(e => e.NoDel).IsUnicode(false);

                entity.Property(e => e.OneTimeInd).IsUnicode(false);

                entity.Property(e => e.PostBlock).IsUnicode(false);

                entity.Property(e => e.PurBlock).IsUnicode(false);

                entity.Property(e => e.SearchTerm1).IsUnicode(false);

                entity.Property(e => e.TaxNo3).IsUnicode(false);

                entity.Property(e => e.TelExt).IsUnicode(false);

                entity.Property(e => e.TelNo).IsUnicode(false);

                entity.Property(e => e.TimeZone).IsUnicode(false);

                entity.Property(e => e.TrZone).IsUnicode(false);

                entity.Property(e => e.TrZoneDesc).IsUnicode(false);

                entity.Property(e => e.VatRegNo).IsUnicode(false);

                entity.Property(e => e.VendAccGrpName).IsUnicode(false);

                entity.Property(e => e.VendorAccGrp).IsUnicode(false);

                entity.Property(e => e.VendorName).IsUnicode(false);
            });

        }
    }
}
