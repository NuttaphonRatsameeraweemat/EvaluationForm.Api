using EVF.Helper.Components;
using EVF.Tranfer.Service.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVF.Tranfer.Service.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class VendorTranferController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The tranfer to datamart manager provides tranfer to datamart functionality.
        /// </summary>
        private readonly IVendorTranferBll _tranfer;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorTranferController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public VendorTranferController(IVendorTranferBll tranfer)
        {
            _tranfer = tranfer;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("TranferVendorData")]
        public IActionResult TranferVendorData()
        {
            return Ok(_tranfer.TranferVendorData());
        }

        [HttpPost]
        [Route("UpdateVendorDataFromZncr03/{vendorNo}")]
        public IActionResult UpdateVendorDataFromZncr03(string vendorNo)
        {
            return Ok(_tranfer.UpdateVendorDataFromZncr03(vendorNo));
        }

        [HttpPost]
        [Route("AddNewVendorDataFromZncr03/{vendorNo}")]
        public IActionResult AddNewVendorDataFromZncr03(string vendorNo)
        {
            return Ok(_tranfer.AddNewVendorDataFromZncr03(vendorNo));
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
