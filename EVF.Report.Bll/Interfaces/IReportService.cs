using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Call service export vendor evaluation pdf report.
        /// </summary>
        /// <param name="model">The request information model for export report.</param>
        /// <returns></returns>
        ResponseFileModel CallVendorEvaluationReport(VendorEvaluationRequestModel model);
    }
}
