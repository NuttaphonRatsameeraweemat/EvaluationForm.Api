using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Interfaces
{
    public interface IEvaluationSummaryReportBll
    {
        void ExportSummaryReport(EvaluationSummaryReportRequestModel model);
    }
}
