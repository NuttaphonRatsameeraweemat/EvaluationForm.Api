using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Interfaces
{
    public interface IVendorEvaluationStatusReportBll
    {
        /// <summary>
        /// Export vendor evaluation status excel report.
        /// </summary>
        /// <param name="model">The filter criteria value.</param>
        ResponseFileModel ExportVendorEvaluationStatusReport(EvaluationSummaryReportRequestModel model);
    }
}
