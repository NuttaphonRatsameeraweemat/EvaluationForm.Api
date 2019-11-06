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
        [Required(ErrorMessage = MessageValue.PleaseFillStartPoint)]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int? StartPoint { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillEndPoint)]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int? EndPoint { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillGradeNameTh)]
        [MaxLength(250, ErrorMessage = MessageValue.GradeNameOverLength)]
        public string GradeNameTh { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillGradeNameEn)]
        [MaxLength(100, ErrorMessage = MessageValue.GradeNameOverLength)]
        public string GradeNameEn { get; set; }
    }
}
