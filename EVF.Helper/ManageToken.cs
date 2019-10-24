using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace EVF.Helper
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
        /// <summary>
        /// Get Encrypt value from payload token.
        /// </summary>
        public string Encrypt => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.ClamisEncrypt)?.Value;
        /// <summary>
        /// Get Org identity value from payload token.
        /// </summary>
        public string OrgId => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.ClamisOrg)?.Value;
        /// <summary>
        /// Get Position identity value from payload token.
        /// </summary>
        public string PositionId => _httpContext.User.Claims.FirstOrDefault(x => x.Type == ConstantValue.ClamisPosition)?.Value;
        /// <summary>
        /// Get Company code value from payload token.
        /// </summary>
        public string[] ComCode
        {
            get
            {
                var result = new List<string>();
                var comList = _httpContext.User.Claims.Where(x => x.Type == ConstantValue.ClamisComCode);
                foreach (var item in comList)
                {
                    result.Add(item.Value);
                }
                return result.ToArray();
            }
        }
        /// <summary>
        /// Get Purchasing org value from payload token.
        /// </summary>
        public string[] PurchasingOrg
        {
            get
            {
                var result = new List<string>();
                var purList = _httpContext.User.Claims.Where(x => x.Type == ConstantValue.ClamisPurchasing);
                foreach (var item in purList)
                {
                    result.Add(item.Value);
                }
                return result.ToArray();
            }
        }

        #endregion

    }
}
