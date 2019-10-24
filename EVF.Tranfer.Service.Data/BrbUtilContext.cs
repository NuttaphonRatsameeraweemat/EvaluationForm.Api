using EVF.Tranfer.Service.Data.Pocos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Data
{
    public class BrbUtilContext : DbContext
    {

        public BrbUtilContext()
        {
        }

        public BrbUtilContext(DbContextOptions<BrbUtilContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SPE_TRANSAC_PO_QA> SPE_TRANSAC_PO_QA { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SPE_TRANSAC_PO_QA>(entity =>
            {
                entity.HasKey(e => new { e.Gjahr, e.Belnr, e.Buzei, e.Para })
                    .HasName("PK__VendorTr__3214EC27F65E5B23");
                
                entity.Property(e => e.Belnr).IsUnicode(false);

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

                entity.Property(e => e.PurchDoc).IsUnicode(false);

                entity.Property(e => e.PurgropCode).IsUnicode(false);

                entity.Property(e => e.PurorgCode).IsUnicode(false);

                entity.Property(e => e.PurorgName).IsUnicode(false);

                entity.Property(e => e.ShortText).IsUnicode(false);

                entity.Property(e => e.UnitCode).IsUnicode(false);

                entity.Property(e => e.Vendor).IsUnicode(false);
            });
        }

    }
}
