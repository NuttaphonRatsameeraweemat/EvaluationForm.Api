using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class HolidayCalendar
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(4)]
        public string Year { get; set; }
        public DateTime? HolidayDate { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
    }
}
