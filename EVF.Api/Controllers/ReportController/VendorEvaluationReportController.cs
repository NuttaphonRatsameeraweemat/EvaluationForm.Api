using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Report.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.ReportController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VendorEvaluationReportController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The cache manager provides cache functionality.
        /// </summary>
        private readonly IReportService _report;
        /// <summary>
        /// The cache manager provides cache functionality.
        /// </summary>
        private readonly IVendorEvaluationReportBll _vendorEvaluationReport;
        
        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorEvaluationReportController" /> class.
        /// </summary>
        /// <param name="report"></param>
        public VendorEvaluationReportController(IReportService report, IVendorEvaluationReportBll vendorEvaluationReport)
        {
            _report = report;
            _vendorEvaluationReport = vendorEvaluationReport;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_vendorEvaluationReport.GetList());
        }

        [HttpGet]
        [Route("GetListByPeriod")]
        public IActionResult GetListByPeriod(int periodItemId)
        {
            return Ok(_vendorEvaluationReport.GetList(periodItemId));
        }
        
        [HttpGet]
        [Route("DownloadFile/{id}")]
        public IActionResult DownloadFile(int id)
        {
            var result = _vendorEvaluationReport.EvaluationExportReport(id);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + result.FileName);
            return File(result.FileContent, "application/octet-stream");
        }

        #endregion

    }
}