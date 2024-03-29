﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class WorkflowProcess
    {
        [Key]
        [StringLength(100)]
        public string ProcessCode { get; set; }
        [StringLength(200)]
        public string ProcessName { get; set; }
        [Column("K2WorkflowProcess")]
        [StringLength(100)]
        public string K2workflowProcess { get; set; }
    }
}
