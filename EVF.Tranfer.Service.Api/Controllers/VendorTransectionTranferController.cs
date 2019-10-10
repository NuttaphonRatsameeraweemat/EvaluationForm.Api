using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Helper.Components;
using EVF.Tranfer.Service.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Tranfer.Service.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class VendorTransectionTranferController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The tranfer to datamart manager provides tranfer to datamart functionality.
        /// </summary>
        private readonly IVendorTransectionTranferBll _tranfer;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorTransectionTranferController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public VendorTransectionTranferController(IVendorTransectionTranferBll tranfer)
        {
            _tranfer = tranfer;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("TranferVendorTransection")]
        public IActionResult TranferVendorTransection()
        {
            return Ok(_tranfer.TranferVendorTransection());
        }

        [HttpPost]
        [Route("TryToConnect")]
        public IActionResult TryToConnect()
        {
            return Ok(_tranfer.TryToConnect());
        }

        #endregion

    }
}