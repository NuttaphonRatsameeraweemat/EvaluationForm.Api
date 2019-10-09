using EVF.Helper.Models;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Workflow.Bll.Interfaces
{
    public interface IWorkflowBll
    {
        /// <summary>
        /// Start workflow process.
        /// </summary>
        /// <param name="dataId">The identity process.</param>
        /// <param name="processCode">The process code.</param>
        /// <param name="folio">The folio name.</param>
        /// <param name="approvalStep">The approval list.</param>
        void Start(int dataId, string processCode, string folio, Dictionary<int, string> approvalStep);
        /// <summary>
        /// Action workflow by outcome in workflow.
        /// </summary>
        /// <param name="model">The task list information.</param>
        string Action(WorkflowViewModel model);
        /// <summary>
        /// Action mutiple task.
        /// </summary>
        /// <param name="models">The task list information.</param>
        void MultipleTaskAction(IEnumerable<WorkflowViewModel> models);
    }
}
