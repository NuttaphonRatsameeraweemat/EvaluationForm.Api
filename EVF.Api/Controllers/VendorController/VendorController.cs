using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.VendorController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = VendorViewModel.RoleForManageData)]
    public class VendorController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Vendor manager provides Vendor functionality.
        /// </summary>
        private readonly IVendorBll _vendor;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorController" /> class.
        /// </summary>
        /// <param name="vendor"></param>
        public VendorController(IVendorBll vendor)
        {
            _vendor = vendor;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_vendor.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(string vendorNo)
        {
            return Ok(_vendor.GetDetail(vendorNo));
        }

        [HttpGet]
        [Route("GetVendorEvaluationHistory")]
        public IActionResult GetVendorEvaluationHistory(string vendorNo, int periodId)
        {
            return Ok(_vendor.GetVendorEvaluationHistory(vendorNo, periodId));
        }

        [HttpGet]
        [Route("GetPieChart")]
        public IActionResult GetPieChart(string vendorNo)
        {
            return Ok(_vendor.GetPieChart(vendorNo));
        }

        [HttpGet]
        [Route("GetLineChart")]
        public IActionResult GetLineChart(string vendorNo)
        {
            return Ok(_vendor.GetLineChart(vendorNo));
        }

        [HttpPost]
        [Route("UpdateVendorContact")]
        public IActionResult UpdateVendorContact([FromBody]VendorRequestViewModel model)
        {
            return Ok(_vendor.UpdateVendorContact(model));
        }

        #endregion

    }
}