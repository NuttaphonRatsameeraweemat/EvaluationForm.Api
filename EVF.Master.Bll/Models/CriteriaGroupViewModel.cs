using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class CriteriaGroupViewModel
    {
        public int Id { get; set; }
        public int? CriteriaId { get; set; }
        [Required]
        public int PerformanceGroupId { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int Score { get; set; }
        public List<CriteriaItemViewModel> CriteriaItems { get; set; }
    }

    public class CriteriaItemViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int? CriteriaGroupId { get; set; }
        [Required]
        public int? PerformanceId { get; set; }
        public string PerformanceNameTh { get; set; }
        public string PerformanceNameEn { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int Score { get; set; }
    }

}
