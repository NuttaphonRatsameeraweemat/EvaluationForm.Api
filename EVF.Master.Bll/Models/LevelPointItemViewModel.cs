using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class LevelPointItemViewModel
    {
        public int Id { get; set; }
        public int LevelPointId { get; set; }
        [Required]
        public string LevelPointName { get; set; }
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int PercentPoint { get; set; }
        [Required]
        public int Sequence { get; set; }
    }
}
