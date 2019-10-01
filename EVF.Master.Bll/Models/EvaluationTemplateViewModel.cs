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
        [Required]
        [MaxLength(200)]
        public string EvaluationTemplateName { get; set; }
        [Required]
        public int CriteriaId { get; set; }
        [Required]
        public int GradeId { get; set; }
        [Required]
        public int LevelPointId { get; set; }
        public bool IsUse { get; set; }
    }

    public class EvaluationTemplateDisplayViewModel
    {
        public string Name { get; set; }
        public CriteriaViewModel Criteria { get; set; }
        public LevelPointViewModel LevelPoint { get; set; }
        public GradeViewModel Grade { get; set; }
    }

}
