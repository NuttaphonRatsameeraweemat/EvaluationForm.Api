using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class PeriodItemViewModel
    {
        public int Id { get; set; }
        public int PeriodID { get; set; }
        [Required]
        public string PeriodName { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartEvaDateString { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndEvaDateString { get; set; }

        public DateTime StartEvaDate { get; set; }
        public DateTime EndEvaDate { get; set; }
    }
}
