using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Interfaces
{
    public interface ISummaryEmailTaskBll
    {
        /// <summary>
        /// Execute email status waiting.
        /// </summary>
        void ExecuteEmailTaskWaiting(string status);
        /// <summary>
        /// Process summary task email.
        /// </summary>
        void ProcessSummaryTask();
    }
}
