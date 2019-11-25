using EVF.Workflow.Bll.Models;
using System.Collections.Generic;

namespace EVF.Workflow.Bll.Interfaces
{
    public interface IWorkflowActivityBll
    {
        /// <summary>
        /// Get workflow activity logs.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        IEnumerable<WorkflowActivityViewModel> GetWorkflowActivity(int id);
    }
}
