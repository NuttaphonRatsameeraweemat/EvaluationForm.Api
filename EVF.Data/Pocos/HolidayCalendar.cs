using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class HolidayCalendar
    {
        public int Id { get; set; }
        [StringLength(4)]
        public string Year { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HolidayDate { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastModifyDate { get; set; }
    }
}
