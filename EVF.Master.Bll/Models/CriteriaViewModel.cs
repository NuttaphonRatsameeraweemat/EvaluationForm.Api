using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class CriteriaViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string CriteriaName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUse { get; set; }
        [Required]
        public List<CriteriaGroupViewModel> CriteriaGroups { get; set; }
    }
}
