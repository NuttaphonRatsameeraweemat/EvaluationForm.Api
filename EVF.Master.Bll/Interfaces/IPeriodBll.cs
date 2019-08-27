using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface IPeriodBll
    {
        /// <summary>
        /// Get Period list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PeriodViewModel> GetList();
    }
}
