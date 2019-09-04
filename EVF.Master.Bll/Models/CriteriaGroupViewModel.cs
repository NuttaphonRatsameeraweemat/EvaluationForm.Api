using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class CriteriaGroupViewModel
    {
        public CriteriaGroupViewModel()
        {
            CriteriaItems = new List<CriteriaItemViewModel>();
        }

        public int Id { get; set; }
        public int? CriteriaId { get; set; }
        [Required]
        public int KpiGroupId { get; set; }
        [Required]
        public int Sequence { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int MaxScore { get; set; }
        public List<CriteriaItemViewModel> CriteriaItems { get; set; }
    }

    public class CriteriaItemViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public int? CriteriaGroupId { get; set; }
        [Required]
        public int? KpiId { get; set; }
        public string KpiNameTh { get; set; }
        public string KpiNameEn { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = MessageValue.GradePointOverRange)]
        public int MaxScore { get; set; }
    }

}
