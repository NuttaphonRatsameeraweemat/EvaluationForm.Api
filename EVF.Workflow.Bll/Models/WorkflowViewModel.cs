using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Workflow.Bll.Models
{
    public class WorkflowViewModel
    {
        public int ProcessInstanceId { get; set; }
        public string SerialNo { get; set; }
        public int Step { get; set; }
        public string Action { get; set; }
        public string Comment { get; set; }
        public Dictionary<string, object> DataFields { get; private set; }
        public string Status { get; private set; }

        /// <summary>
        /// Set datafields property.
        /// </summary>
        /// <param name="source"></param>
        public void SetDataFields(Dictionary<string, object> source)
        {
            this.DataFields = source;
        }

        /// <summary>
        /// Set status property.
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(string status)
        {
            this.Status = status;
        }

    }
}
