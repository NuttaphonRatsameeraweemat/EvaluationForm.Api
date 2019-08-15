﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Bll.Models
{
    public class PerformanceGroupViewModel
    {
        public PerformanceGroupViewModel()
        {
            PerformanceGroupItems = new List<int>();
        }

        public const string RoleForManageData = "Role_MA_PerformanceGroup";
        public const string RoleForDisplayData = "Role_DS_PerformanceGroup";

        public int Id { get; set; }
        [Required]
        public string PerformanceGroupName { get; set; }
        public List<int> PerformanceGroupItems { get; set; }
    }
}
