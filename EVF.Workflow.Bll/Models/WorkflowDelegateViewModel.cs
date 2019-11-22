using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Workflow.Bll.Models
{
    public class WorkflowDelegateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FromUser { get; set; }
        [Required]
        public string ToUser { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartDateString { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndDateString { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class WorkflowDelegateRequestModel
    {
        public int Id { get; set; }
        [Required]
        public string ToUser { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartDate { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndDate { get; set; }
    }

}
