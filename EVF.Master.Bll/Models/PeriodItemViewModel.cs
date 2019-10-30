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
        [Required(ErrorMessage = MessageValue.PleaseFillPeriodItemName)]
        [MaxLength(80, ErrorMessage = MessageValue.PeriodItemNameOverLength)]
        public string PeriodName { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillStartEvaDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartEvaDateString { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillEndEvaDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndEvaDateString { get; set; }

        public DateTime StartEvaDate { get; set; }
        public DateTime EndEvaDate { get; set; }
    }
}
