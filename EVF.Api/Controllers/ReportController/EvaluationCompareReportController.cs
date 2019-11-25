using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.ReportController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EvaluationCompareReportController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation compare report manager provides evaluation compare report functionality.
        /// </summary>
        private readonly IEvaluationCompareReportBll _evaluationCompareReport;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationCompareReportController" /> class.
        /// </summary>
        /// <param name="evaluationSummaryReport"></param>
        public EvaluationCompareReportController(IEvaluationCompareReportBll evaluationCompareReport)
        {
            _evaluationCompareReport = evaluationCompareReport;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("ExportSummaryReport")]
        public IActionResult ExportSummaryReport([FromBody]EvaluationCompareReportRequestModel model)
        {
            var result = _evaluationCompareReport.ExportEvaluationCompareReport(model);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + result.FileName);
            return File(result.FileContent, "application/octet-stream");
        }

        #endregion

    }
}