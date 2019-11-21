using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Interfaces
{
    public interface IEvaluationSummaryReportBll
    {
        /// <summary>
        /// Export summary evaluation excel report.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        ResponseFileModel ExportSummaryReport(EvaluationSummaryReportRequestModel model);
    }
}
