﻿using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Hr.Bll.Interfaces
{
    public interface IHrEmployeeBll
    {
        /// <summary>
        /// Get Employee list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrEmployeeViewModel> GetList();
        /// <summary>
        /// Get Employee list in purchase org.
        /// </summary>
        /// <param name="purOrg">The purchase org code.</param>
        /// <returns></returns>
        IEnumerable<HrEmployeeViewModel> GetListByPurchaseOrg(string purOrg);
        /// <summary>
        /// Get Employee list without purchase org.
        /// </summary>
        /// <param name="purOrg">The purchase org code.</param>
        /// <returns></returns>
        IEnumerable<HrEmployeeViewModel> GetListWithOutPurchaseOrg(string purOrg);
        /// <summary>
        /// Get Employee list filter by company code.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrEmployeeViewModel> GetListByComCode(string comCode);
        /// <summary>
        /// Get Employee list filter by organization id.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HrEmployeeViewModel> GetListByOrg(string orgId);
    }
}
