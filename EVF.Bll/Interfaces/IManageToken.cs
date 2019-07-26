using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
{
    public interface IManageToken
    {
        /// <summary>
        /// Get Ad User from payload token.
        /// </summary>
        string AdUser { get; }
        /// <summary>
        /// Get Full Name from payload token.
        /// </summary>
        string EmpName { get; }
        /// <summary>
        /// Get Employee No from payload token.
        /// </summary>
        string EmpNo { get; }
    }
}
