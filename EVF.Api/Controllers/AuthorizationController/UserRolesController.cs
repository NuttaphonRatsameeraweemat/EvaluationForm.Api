using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserRolesController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The User Role manager provides User Role functionality.
        /// </summary>
        private readonly IUserRoleBll _userRole;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="UserRolesController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public UserRolesController(IUserRoleBll userRole)
        {
            _userRole = userRole;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_userRole.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(string adUser)
        {
            IActionResult response;
            if (string.IsNullOrEmpty(adUser))
            {
                response = BadRequest(ConstantValue.ArgullmentNullOrEmptyMessage);
            }
            else response = Ok(_userRole.GetDetail(adUser));
            return response;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]UserRoleViewModel model)
        {
            return Ok(_userRole.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]UserRoleViewModel model)
        {
            return Ok(_userRole.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody]string adUser)
        {
            IActionResult response;
            if (string.IsNullOrEmpty(adUser))
            {
                response = BadRequest(ConstantValue.ArgullmentNullOrEmptyMessage);
            }
            else response = Ok(_userRole.Delete(adUser));
            return response;
        }

        #endregion

    }
}