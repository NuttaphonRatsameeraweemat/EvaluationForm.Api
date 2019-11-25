using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.ReportController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VendorEvaluationStatusReportController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation compare report manager provides evaluation compare report functionality.
        /// </summary>
        private readonly IVendorEvaluationStatusReportBll _vendorEvaluationStatusReport;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorEvaluationStatusReportController" /> class.
        /// </summary>
        /// <param name="evaluationSummaryReport"></param>
        public VendorEvaluationStatusReportController(IVendorEvaluationStatusReportBll vendorEvaluationStatusReport)
        {
            _vendorEvaluationStatusReport = vendorEvaluationStatusReport;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("ExportSummaryReport")]
        public IActionResult ExportSummaryReport([FromBody]VendorEvaluationStatusReportRequestModel model)
        {
            var result = _vendorEvaluationStatusReport.ExportVendorEvaluationStatusReport(model);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + result.FileName);
            return File(result.FileContent, "application/octet-stream");
        }

        #endregion

    }
}