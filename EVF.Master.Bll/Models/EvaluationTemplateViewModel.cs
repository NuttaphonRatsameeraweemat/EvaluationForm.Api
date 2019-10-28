using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class EvaluationTemplateViewModel
    {

        public const string RoleForManageData = "Role_MA_EvaluationTemplate";
        public const string RoleForDisplayData = "Role_DS_EvaluationTemplate";

        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillEvaluationTemplateName)]
        [MaxLength(200, ErrorMessage = MessageValue.EvaluationTemplateNameOverLength)]
        public string EvaluationTemplateName { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedCriteria)]
        public int? CriteriaId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedGrade)]
        public int? GradeId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedLevelPoint)]
        public int? LevelPointId { get; set; }
        public bool IsUse { get; set; }
    }

    public class EvaluationTemplateDisplayViewModel
    {
        public string Name { get; set; }
        public int MaxTotalScore { get; set; }
        public CriteriaViewModel Criteria { get; set; }
        public LevelPointViewModel LevelPoint { get; set; }
        public GradeViewModel Grade { get; set; }
    }

}
