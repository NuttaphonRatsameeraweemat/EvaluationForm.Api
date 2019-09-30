using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EVF.Tranfer.Service.Data.Pocos
{
    public class SPE_TRANSAC_PO_QA
    {
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
        [Column("DataUpdate_Date", TypeName = "datetime")]
        public DateTime? DataUpdateDate { get; set; }
    }
}
