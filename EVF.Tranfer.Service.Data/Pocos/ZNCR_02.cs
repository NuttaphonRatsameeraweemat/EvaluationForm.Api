using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Tranfer.Service.Data.Pocos
{
    public partial class ZNCR_02
    {
        [Column("COM_CODE")]
        [StringLength(4)]
        public string ComCode { get; set; }
        [Column("PUR_ORG")]
        [StringLength(4)]
        public string PurOrg { get; set; }
        [Column("VENDOR")]
        [StringLength(10)]
        public string Vendor { get; set; }
        [Column("YEAR_MONTH")]
        [StringLength(5)]
        public string YearMonth { get; set; }
        [Column("WEIGHT_KEY")]
        [StringLength(2)]
        public string WeightKey { get; set; }
        [Column("SCR01", TypeName = "decimal(5, 0)")]
        public decimal? Scr01 { get; set; }
        [Column("SCR02", TypeName = "decimal(5, 0)")]
        public decimal? Scr02 { get; set; }
        [Column("SCR03", TypeName = "decimal(5, 0)")]
        public decimal? Scr03 { get; set; }
        [Column("SCR04", TypeName = "decimal(5, 0)")]
        public decimal? Scr04 { get; set; }
        [Column("SCR05", TypeName = "decimal(5, 0)")]
        public decimal? Scr05 { get; set; }
        [Column("SCR06", TypeName = "decimal(5, 0)")]
        public decimal? Scr06 { get; set; }
        [Column("SCR07", TypeName = "decimal(5, 0)")]
        public decimal? Scr07 { get; set; }
        [Column("SCR08", TypeName = "decimal(5, 0)")]
        public decimal? Scr08 { get; set; }
        [Column("SCR09", TypeName = "decimal(5, 0)")]
        public decimal? Scr09 { get; set; }
        [Column("SCR10", TypeName = "decimal(5, 0)")]
        public decimal? Scr10 { get; set; }
        [Column("SCR11", TypeName = "decimal(5, 0)")]
        public decimal? Scr11 { get; set; }
        [Column("SCR12", TypeName = "decimal(5, 0)")]
        public decimal? Scr12 { get; set; }
        [Column("SCR13", TypeName = "decimal(5, 0)")]
        public decimal? Scr13 { get; set; }
        [Column("SCR14", TypeName = "decimal(5, 0)")]
        public decimal? Scr14 { get; set; }
        [Column("SCR15", TypeName = "decimal(5, 0)")]
        public decimal? Scr15 { get; set; }
        [Column("SCR16", TypeName = "decimal(5, 0)")]
        public decimal? Scr16 { get; set; }
        [Column("SCR17", TypeName = "decimal(5, 0)")]
        public decimal? Scr17 { get; set; }
        [Column("SCR18", TypeName = "decimal(5, 0)")]
        public decimal? Scr18 { get; set; }
        [Column("SCR19", TypeName = "decimal(5, 0)")]
        public decimal? Scr19 { get; set; }
        [Column("SCR20", TypeName = "decimal(5, 0)")]
        public decimal? Scr20 { get; set; }
        [Column("SCR21", TypeName = "decimal(5, 0)")]
        public decimal? Scr21 { get; set; }
        [Column("SCR22", TypeName = "decimal(5, 0)")]
        public decimal? Scr22 { get; set; }
        [Column("SCR23", TypeName = "decimal(5, 0)")]
        public decimal? Scr23 { get; set; }
        [Column("SCR24", TypeName = "decimal(5, 0)")]
        public decimal? Scr24 { get; set; }
        [Column("SCR25", TypeName = "decimal(5, 0)")]
        public decimal? Scr25 { get; set; }
        [Column("SCR26", TypeName = "decimal(5, 0)")]
        public decimal? Scr26 { get; set; }
        [Column("SCR27", TypeName = "decimal(5, 0)")]
        public decimal? Scr27 { get; set; }
        [Column("SCR28", TypeName = "decimal(5, 0)")]
        public decimal? Scr28 { get; set; }
        [Column("SCR29", TypeName = "decimal(5, 0)")]
        public decimal? Scr29 { get; set; }
        [Column("SCR30", TypeName = "decimal(5, 0)")]
        public decimal? Scr30 { get; set; }
        [Column("SCR31", TypeName = "decimal(5, 0)")]
        public decimal? Scr31 { get; set; }
        [Column("SCR32", TypeName = "decimal(5, 0)")]
        public decimal? Scr32 { get; set; }
        [Column("SCR33", TypeName = "decimal(5, 0)")]
        public decimal? Scr33 { get; set; }
        [Column("SCR34", TypeName = "decimal(5, 0)")]
        public decimal? Scr34 { get; set; }
        [Column("SCR35", TypeName = "decimal(5, 0)")]
        public decimal? Scr35 { get; set; }
        [Column("SCR36", TypeName = "decimal(5, 0)")]
        public decimal? Scr36 { get; set; }
        [Column("SCR37", TypeName = "decimal(5, 0)")]
        public decimal? Scr37 { get; set; }
        [Column("SCR38", TypeName = "decimal(5, 0)")]
        public decimal? Scr38 { get; set; }
        [Column("SCR39", TypeName = "decimal(5, 0)")]
        public decimal? Scr39 { get; set; }
        [Column("SCR40", TypeName = "decimal(5, 0)")]
        public decimal? Scr40 { get; set; }
        [Column("SCR41", TypeName = "decimal(5, 0)")]
        public decimal? Scr41 { get; set; }
        [Column("SCR42", TypeName = "decimal(5, 0)")]
        public decimal? Scr42 { get; set; }
        [Column("SCR43", TypeName = "decimal(5, 0)")]
        public decimal? Scr43 { get; set; }
        [Column("SCR44", TypeName = "decimal(5, 0)")]
        public decimal? Scr44 { get; set; }
        [Column("SCR45", TypeName = "decimal(5, 0)")]
        public decimal? Scr45 { get; set; }
        [Column("SCR46", TypeName = "decimal(5, 0)")]
        public decimal? Scr46 { get; set; }
        [Column("SCR47", TypeName = "decimal(5, 0)")]
        public decimal? Scr47 { get; set; }
        [Column("SCR48", TypeName = "decimal(5, 0)")]
        public decimal? Scr48 { get; set; }
        [Column("SCR49", TypeName = "decimal(5, 0)")]
        public decimal? Scr49 { get; set; }
        [Column("SCR50", TypeName = "decimal(5, 0)")]
        public decimal? Scr50 { get; set; }
        [Column("CTEDB")]
        [StringLength(40)]
        public string CteDb { get; set; }
        [Column("CTEDO")]
        public DateTime? CteDo { get; set; }
        [Column("UPDATE_BY")]
        [StringLength(12)]
        public string UpdateBy { get; set; }
        [Column("UPDATE_ON")]
        public DateTime? UpdateOn { get; set; }
        [Column("PROGRAM_ID")]
        [StringLength(40)]
        public string ProgramId { get; set; }
        [Column("TCODE")]
        [StringLength(20)]
        public string TCode { get; set; }
    }
}
