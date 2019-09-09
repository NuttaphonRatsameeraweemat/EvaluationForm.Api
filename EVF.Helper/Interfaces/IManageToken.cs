using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Interfaces
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
        /// <summary>
        /// Get Encrypt value from payload token.
        /// </summary>
        string Encrypt { get; }
        /// <summary>
        /// Get Org identity value from payload token.
        /// </summary>
        string OrgId { get; }
        /// <summary>
        /// Get Position identity value from payload token.
        /// </summary>
        string PositionId { get; }
        /// <summary>
        /// Get Company code value from payload token.
        /// </summary>
        string ComCode { get; }
    }
}
