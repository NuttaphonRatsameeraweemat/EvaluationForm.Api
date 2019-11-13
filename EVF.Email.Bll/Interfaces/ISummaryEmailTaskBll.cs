using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Interfaces
{
    public interface ISummaryEmailTaskBll
    {
        void ExecuteEmailTaskWaiting();
        void ProcessSummaryTask();
    }
}
