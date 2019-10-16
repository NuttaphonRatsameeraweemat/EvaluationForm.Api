using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.CentralSettingController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PurchasingOrgController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The PurchasingOrg manager provides PurchasingOrg functionality.
        /// </summary>
        private readonly IPurchasingOrgBll _purchasingOrg;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PurchasingOrgController" /> class.
        /// </summary>
        /// <param name="purchasingOrg"></param>
        public PurchasingOrgController(IPurchasingOrgBll purchasingOrg)
        {
            _purchasingOrg = purchasingOrg;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetAllPurchaseOrg")]
        public IActionResult GetAllPurchaseOrg()
        {
            return Ok(_purchasingOrg.GetAllPurchaseOrg());
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_purchasingOrg.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(string purOrg)
        {
            return Ok(_purchasingOrg.GetDetail(purOrg));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = PurchasingOrgViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]PurchasingOrgViewModel model)
        {
            return Ok(_purchasingOrg.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = PurchasingOrgViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]PurchasingOrgViewModel model)
        {
            return Ok(_purchasingOrg.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = PurchasingOrgViewModel.RoleForManageData)]
        public IActionResult Delete(string purOrg)
        {
            return Ok(_purchasingOrg.Delete(purOrg));
        }

        #endregion

    }
}