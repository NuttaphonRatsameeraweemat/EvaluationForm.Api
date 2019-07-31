﻿using EVF.Bll.Interfaces;
using EVF.Bll.Models;
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
            return Ok(_userRole.GetDetail(adUser));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(UserRoleViewModel model)
        {
            return Ok(_userRole.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(UserRoleViewModel model)
        {
            return Ok(_userRole.Edit(model));
        }

        #endregion

    }
}