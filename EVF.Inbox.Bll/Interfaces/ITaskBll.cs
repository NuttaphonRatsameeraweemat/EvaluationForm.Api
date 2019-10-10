using EVF.Inbox.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Interfaces
{
    public interface ITaskBll
    {
        /// <summary>
        /// Get Task pending list from k2.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskViewModel> GetTaskList();
        /// <summary>
        /// Get Task delegate pending list from k2.
        /// </summary>
        /// <param name="fromUser">The user task delegate task.</param>
        /// <returns></returns>
        IEnumerable<TaskViewModel> GetTaskListDelegate(string fromUser);
    }
}
