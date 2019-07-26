using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Bll
{
    public class ManageToken : IManageToken
    {

        #region [Fields]

        /// <summary>
        /// The httpcontext.
        /// </summary>
        private readonly HttpContext _httpContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageToken" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The httpcontext value.</param>
        public ManageToken(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Ad User from payload token.
        /// </summary>
        public string AdUser => _httpContext.User.Identity.Name;
        /// <summary>
        /// Get Full Name from payload token.
        /// </summary>
        public string EmpName => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.ClamisName)?.Value;
        /// <summary>
        /// Get Employee No from payload token.
        /// </summary>
        public string EmpNo => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.ClamisEmpNo)?.Value;

        #endregion

    }
}
