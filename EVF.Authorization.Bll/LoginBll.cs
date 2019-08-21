using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EVF.Authorization.Bll
{
    public class LoginBll : ILoginBll
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The AdServuce.
        /// </summary>
        private readonly IAdService _adService;
        /// <summary>
        /// The Role manager provides role functionality.
        /// </summary>
        private readonly IRoleBll _roleBll;
        /// <summary>
        /// The ClaimsIdentity.
        /// </summary>
        private ClaimsIdentity _identity;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginBll" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public LoginBll(IConfigSetting config, IUnitOfWork unitOfWork, IAdService adService, IRoleBll roleBll)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _adService = adService;
            _roleBll = roleBll;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Validate username and password is valid.
        /// </summary>
        /// <param name="login">The login value.</param>
        /// <returns></returns>
        public ResultViewModel Authenticate(LoginViewModel login)
        {
            var result = new ResultViewModel();
            if (_adService.Authen(login.Username, login.Password))
            {
                login.Username = $"{_config.DomainUser}{login.Username}";
                result = _roleBll.ValidateRole(login.Username);
            }
            else result = UtilityService.InitialResultError(MessageValue.LoginFailed, 401);
            return result;
        }

        /// <summary>
        /// Create and setting payload on token.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal.</param>
        /// <returns></returns>
        public string BuildToken(ClaimsPrincipal principal = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config.JwtIssuer,
              _config.JwtIssuer,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds,
              claims: this.GetClaimsPrincipal(principal));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// The Method Add ClaimsIdentity Properties.
        /// </summary>
        /// <param name="username">The identity user.</param>
        public EmployeeViewModel ManageClaimsIdentity(LoginViewModel login)
        {
            Hremployee data = _unitOfWork.GetRepository<Hremployee>().Get(x => x.Aduser == login.Username).FirstOrDefault();
            if (data == null)
            {
                throw new ArgumentNullException(ConstantValue.HrEmployeeArgumentNullExceptionMessage);
            }
            var result = new EmployeeViewModel
            {
                EmpNo = data.EmpNo,
                FirstNameTH = data.FirstnameTh,
                LastNameTH = data.LastnameTh,
                ComCode = data.ComCode,
                OrgId = data.OrgId,
                PositionId = data.PositionId
            };

            var roleList = _roleBll.GetCompositeRoleItemByAdUser(login.Username);
            _identity = new ClaimsIdentity();
            _identity.AddClaim(new Claim(ClaimTypes.Name, data.Aduser));
            _identity.AddClaim(new Claim(ConstantValue.ClamisEncrypt, UtilityService.EncryptString(login.Password, _config.EncryptionKey)));
            _identity.AddClaim(new Claim(ConstantValue.ClamisEmpNo, data.EmpNo));
            _identity.AddClaim(new Claim(ConstantValue.ClamisName, string.Format(ConstantValue.EmpTemplate, data.FirstnameTh, data.LastnameTh)));
            foreach (var item in roleList)
            {
                _identity.AddClaim(new Claim(ClaimTypes.Role, item.RoleMenu));
            }
            return result;
        }

        /// <summary>
        /// Get Claims Principal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal.</param>
        /// <returns></returns>
        private List<Claim> GetClaimsPrincipal(ClaimsPrincipal principal)
        {
            var claims = new List<Claim>();
            if (principal != null)
            {
                claims = principal.Claims.ToList();
            }
            else claims = _identity.Claims.ToList();
            return claims;
        }

        /// <summary>
        /// Setup response cookie and cookie options token.
        /// </summary>
        /// <param name="httpContext">The HttpContext.</param>
        /// <param name="token">The token value.</param>
        public void SetupCookie(HttpContext httpContext, string token)
        {
            httpContext.Response.Cookies.Append("access_token", token, new CookieOptions()
            {
                Path = "/",
                HttpOnly = false, // to prevent XSS
                Secure = false, // set to true in production
                Expires = System.DateTime.UtcNow.AddMinutes(600) // token life time
            });
        }

        #endregion

    }
}
