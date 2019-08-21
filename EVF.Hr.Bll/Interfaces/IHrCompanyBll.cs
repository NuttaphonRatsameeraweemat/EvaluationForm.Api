using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Hr.Bll.Interfaces
{
    public interface IHrCompanyBll
    {
        /// <summary>
        /// Get Company list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrCompanyViewModel> GetList();
    }
}
