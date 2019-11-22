using EVF.Helper.Models;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Workflow.Bll.Interfaces
{
    public interface IWorkflowDelegateBll
    {
        /// <summary>
        /// Get workflow delegate list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<WorkflowDelegateViewModel> GetList();
        /// <summary>
        /// Get workflow delegate detail item.
        /// </summary>
        /// <param name="id">The identity of workflow delegate.</param>
        /// <returns></returns>
        WorkflowDelegateViewModel GetDetail(int id);
        /// <summary>
        /// Get workflow delegate detail item.
        /// </summary>
        /// <param name="id">The identity of workflow delegate.</param>
        /// <returns></returns>
        WorkflowDelegateRequestModel GetDelegateInbox();
        /// <summary>
        /// Save deletegate task fromuser touser.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        ResultViewModel SaveDelegateFromInbox(WorkflowDelegateRequestModel model);
        /// <summary>
        /// Save deletegate task fromuser touser.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        ResultViewModel SaveDelegate(WorkflowDelegateViewModel model);
        /// <summary>
        /// Update delegate task user.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        ResultViewModel UpdateDelegate(WorkflowDelegateViewModel model);
        /// <summary>
        /// Update delegate task user.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        ResultViewModel UpdateDelegateInbox(WorkflowDelegateRequestModel model);
        /// <summary>
        /// Remove delegate workflw task.
        /// </summary>
        /// <param name="id">The identity of delegate.</param>
        /// <returns></returns>
        ResultViewModel RemoveDelegate(int id);
        /// <summary>
        /// Remove delegate workflw task.
        /// </summary>
        /// <param name="id">The identity of delegate.</param>
        /// <returns></returns>
        ResultViewModel RemoveDelegateInbox();
    }
}
