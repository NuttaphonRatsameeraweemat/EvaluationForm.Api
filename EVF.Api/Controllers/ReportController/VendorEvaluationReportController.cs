using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Report.Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.ReportController
{
    [Route("[controller]")]
    [ApiController]
    public class VendorEvaluationReportController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The cache manager provides cache functionality.
        /// </summary>
        private readonly IReportService _report;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="VendorEvaluationReportController" /> class.
        /// </summary>
        /// <param name="report"></param>
        public VendorEvaluationReportController(IReportService report)
        {
            _report = report;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("PreviewFile")]
        public IActionResult PreviewFile()
        {
            var result = _report.Try();
            Response.Headers.Add("Content-Disposition", "inline; filename=" + result.FileName + ".pdf");
            return File(result.FileContent, "application/pdf");
        }

        [HttpGet]
        [Route("DownloadFile")]
        public IActionResult DownloadFile()
        {
            var result = _report.Try();
            return File(result.FileContent, "application/pdf", result.FileName);
        }

        #endregion

    }
}