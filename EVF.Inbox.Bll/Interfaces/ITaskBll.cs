﻿using EVF.Inbox.Bll.Models;
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
        TaskViewModel GetTaskList(string fromUser);
    }
}
