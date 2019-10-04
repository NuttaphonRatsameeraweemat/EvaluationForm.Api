using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.VendorController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = VendorTransectionViewModel.RoleForManageData)]
    public class VendorTransectionController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Vendor manager provides Vendor functionality.
        /// </summary>
        private readonly IVendorTransectionBll _vendorTransection;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorTransectionController" /> class.
        /// </summary>
        /// <param name="vendorTransection"></param>
        public VendorTransectionController(IVendorTransectionBll vendorTransection)
        {
            _vendorTransection = vendorTransection;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_vendorTransection.GetList());
        }

        [HttpGet]
        [Route("GetListSearch")]
        public IActionResult GetListSearch([FromQuery]VendorTransectionSearchViewModel model)
        {
            return Ok(_vendorTransection.GetListSearch(model));
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_vendorTransection.GetDetail(id));
        }

        [HttpPost]
        [Route("MarkWeightingKey")]
        public IActionResult MarkWeightingKey([FromBody]VendorTransectionRequestViewModel model)
        {
            return Ok(_vendorTransection.MarkWeightingKey(model));
        }

        #endregion

    }
}