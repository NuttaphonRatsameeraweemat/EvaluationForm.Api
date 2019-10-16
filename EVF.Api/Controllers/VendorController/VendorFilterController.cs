using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.VendorController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VendorFilterController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Vendor filter manager provides Vendor filter functionality.
        /// </summary>
        private readonly IVendorFilterBll _vendorFilter;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorFilterController" /> class.
        /// </summary>
        /// <param name="vendorFilter"></param>
        public VendorFilterController(IVendorFilterBll vendorFilter)
        {
            _vendorFilter = vendorFilter;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList(int periodItemId)
        {
            return Ok(_vendorFilter.GetList(periodItemId));
        }

        [HttpGet]
        [Route("GetVendorFilters")]
        public IActionResult GetVendorFilters([FromQuery]VendorFilterCriteriaViewModel model)
        {
            return Ok(_vendorFilter.GetVendorFilters(model));
        }

        [HttpGet]
        [Route("SearchVendor")]
        public IActionResult SearchVendor([FromQuery]VendorFilterSearchViewModel model)
        {
            return Ok(_vendorFilter.SearchVendor(model));
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_vendorFilter.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = VendorFilterViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]VendorFilterRequestViewModel model)
        {
            return Ok(_vendorFilter.Save(model));
        }

        [HttpPost]
        [Route("ChangeAssignToSave")]
        [Authorize(Roles = VendorFilterViewModel.RoleForManageData)]
        public IActionResult ChangeAssignToSave([FromBody]VendorFilterEditRequestViewModel model)
        {
            return Ok(_vendorFilter.ChangeAssignTo(model));
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = VendorFilterViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            return Ok(_vendorFilter.Delete(id));
        }

        #endregion

    }
}