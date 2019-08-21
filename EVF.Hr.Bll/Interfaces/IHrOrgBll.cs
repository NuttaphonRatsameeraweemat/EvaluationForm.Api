using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Hr.Bll.Interfaces
{
    public interface IHrOrgBll
    {
        /// <summary>
        /// Get Organization list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrOrgViewModel> GetList();
        /// <summary>
        /// Get Organization list by company code.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrOrgViewModel> GetListByComCode(string comCode);
    }
}
