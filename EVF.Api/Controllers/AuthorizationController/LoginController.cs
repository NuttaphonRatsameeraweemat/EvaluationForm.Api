using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using EVF.Helper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Login manager provides Login functionality.
        /// </summary>
        private readonly ILoginBll _login;
        /// <summary>
        /// The Menu manager provides Menu functionality.
        /// </summary>
        private readonly IMenuBll _menu;
        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="login"></param>
        public LoginController(ILoginBll login, IMenuBll menu)
        {
            _login = login;
            _menu = menu;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginViewModel auth)
        {
            IActionResult response;
            var result = new ResultViewModel();
            result = _login.Authenticate(auth);
            if (!result.IsError)
            {
                var model = _login.ManageClaimsIdentity(auth);
                string token = _login.BuildToken();
                var responseMessage = new
                {
                    Employee = model,
                    Menu = _menu.GenerateMenu(auth.Username),
                    Token = token
                };
                _login.SetupCookie(HttpContext, token);
                response = Ok(responseMessage);
            }
            else response = Unauthorized(result);

            return response;
        }

        #endregion

    }
}