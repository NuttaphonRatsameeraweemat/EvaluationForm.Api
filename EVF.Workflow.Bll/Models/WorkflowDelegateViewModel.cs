using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Workflow.Bll.Models
{
    public class WorkflowDelegateViewModel
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
