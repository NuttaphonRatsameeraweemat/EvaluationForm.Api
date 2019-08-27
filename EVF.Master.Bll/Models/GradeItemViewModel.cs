using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class GradeItemViewModel
    {
        public int Id { get; set; }
        public int? GradeId { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int? StartPoint { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int? EndPoint { get; set; }
        [Required]
        [MaxLength(100)]
        public string GradeName { get; set; }
    }
}
