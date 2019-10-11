using EVF.Helper.Models;
using EVF.Inbox.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Interfaces
{
    public interface ITaskActionBll
    {
        /// <summary>
        /// Process task action k2.
        /// </summary>
        /// <param name="model">The task action information.</param>
        /// <param name="action">The action value.</param>
        /// <returns></returns>
        ResultViewModel ActionTask(TaskActionViewModel model, string action);
        /// <summary>
        /// Process multi task action k2.
        /// </summary>
        /// <param name="models">The task list action information.</param>
        /// <param name="action">The action value.</param>
        /// <returns></returns>
        ResultViewModel ActionMultiTask(IEnumerable<TaskActionViewModel> models, string action);
    }
}
