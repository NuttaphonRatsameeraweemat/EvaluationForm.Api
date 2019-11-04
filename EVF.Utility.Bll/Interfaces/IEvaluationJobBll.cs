using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVF.Utility.Bll.Interfaces
{
    public interface IEvaluationJobBll
    {
        /// <summary>
        /// Execute evaluation process end of evaluation period.
        /// </summary>
        /// <returns></returns>
        ResultViewModel ExecuteEvaluationProcess();
    }
}
