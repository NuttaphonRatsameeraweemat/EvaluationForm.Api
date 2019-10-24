using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class VendorTransaction
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("Receipt_Date", TypeName = "datetime")]
        public DateTime? ReceiptDate { get; set; }
        [StringLength(30)]
        public string Vendor { get; set; }
        [Column("Material_Code")]
        [StringLength(30)]
        public string MaterialCode { get; set; }
        [StringLength(9)]
        public string MaterialGrp { get; set; }
        [Column("Unit_Code")]
        [StringLength(30)]
        public string UnitCode { get; set; }
        [Column("Quantity_Received")]
        public double? QuantityReceived { get; set; }
        [Column("Total_recieved")]
        public double? TotalRecieved { get; set; }
        [StringLength(100)]
        public string CompanyCode { get; set; }
        [Column("Doc_Number")]
        [StringLength(30)]
        public string DocNumber { get; set; }
        [Column("Doc_Type")]
        [StringLength(30)]
        public string DocType { get; set; }
        [Column("Line_ID")]
        [StringLength(30)]
        public string LineId { get; set; }
        [Column("K2_KEY")]
        [StringLength(240)]
        public string K2Key { get; set; }
        [Column("DATATYPE")]
        [StringLength(240)]
        public string Datatype { get; set; }
        [Column("FREE")]
        [StringLength(1)]
        public string Free { get; set; }
        [StringLength(1)]
        public string Intercomp { get; set; }
        [Column("PURORG_Code")]
        [StringLength(4)]
        public string PurorgCode { get; set; }
        [Column("PURORG_Name")]
        [StringLength(26)]
        public string PurorgName { get; set; }
        [Column("condition")]
        [StringLength(10)]
        public string Condition { get; set; }
        [Column("PURGrop_Code")]
        [StringLength(3)]
        public string PurgropCode { get; set; }
        [Column("GJAHR", TypeName = "numeric(4, 0)")]
        public decimal? Gjahr { get; set; }
        [Column("BELNR")]
        [StringLength(10)]
        public string Belnr { get; set; }
        [Column("BUZEI", TypeName = "numeric(4, 0)")]
        public decimal? Buzei { get; set; }
        [Column("PARA")]
        public int? Para { get; set; }
        [Column("Short_Text")]
        [StringLength(40)]
        public string ShortText { get; set; }
        [Column("Purch_Doc")]
        [StringLength(10)]
        public string PurchDoc { get; set; }
        [Column("DataUpdate_Date", TypeName = "datetime")]
        public DateTime? DataUpdateDate { get; set; }
        [StringLength(2)]
        public string MarkWeightingKey { get; set; }
    }
}
