﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Workflow.Bll.Models
{
    public class WorkflowViewModel
    {
        public int ProcessInstanceId { get; set; }
        public string SerialNo { get; set; }
        public int DataId { get; set; }
        public int Step { get; set; }
        public string Action { get; set; }
        public string Comment { get; set; }

    }
}
