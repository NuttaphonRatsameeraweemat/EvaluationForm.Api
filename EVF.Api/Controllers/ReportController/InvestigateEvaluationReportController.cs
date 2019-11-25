using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.ReportController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class InvestigateEvaluationReportController : ControllerBase
    {

        #region [Fields]
        
        /// <summary>
        /// The cache manager provides cache functionality.
        /// </summary>
        private readonly IInvestigateEvaluationReportBll _evaluationSummaryReport;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="InvestigateEvaluationReportController" /> class.
        /// </summary>
        /// <param name="evaluationSummaryReport"></param>
        public InvestigateEvaluationReportController(IInvestigateEvaluationReportBll evaluationSummaryReport)
        {
            _evaluationSummaryReport = evaluationSummaryReport;
        }

        #endregion

        #region [Methods]
        
        [HttpPost]
        [Route("ExportSummaryReport")]
        public IActionResult ExportSummaryReport([FromBody]InvestigateEvaluationReportRequestModel model)
        {
            var result = _evaluationSummaryReport.ExportSummaryReport(model);
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + result.FileName);
            return File(result.FileContent, "application/octet-stream");
        }
        
        #endregion

    }
}