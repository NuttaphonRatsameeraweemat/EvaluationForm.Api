﻿using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = RoleViewModel.RoleForManageData)]
    public class RoleController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Role manager provides Role functionality.
        /// </summary>
        private readonly IRoleBll _role;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="RoleController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public RoleController(IRoleBll role)
        {
            _role = role;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetAllMenu")]
        public IActionResult GetAllMenu()
        {
            return Ok(_role.GetAllMenu());
        }

        [HttpGet]
        [Route("GetDetailCompositeRole")]
        public IActionResult GetDetailCompositeRole(int id)
        {
            return Ok(_role.GetDetailCompositeRole(id));
        }

        [HttpGet]
        [Route("GetRoleList")]
        public IActionResult GetRoleList()
        {
            return Ok(_role.GetRoleList());
        }

        [HttpGet]
        [Route("GetActiveRoleList")]
        public IActionResult GetActiveRoleList()
        {
            return Ok(_role.GetActiveRoleList());
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]RoleViewModel model)
        {
            return Ok(_role.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]RoleViewModel model)
        {
            return Ok(_role.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_role.Delete(id));
        }

        #endregion

    }
}