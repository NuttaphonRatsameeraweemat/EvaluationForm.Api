using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Interfaces
{
    public interface IVendorEvaluationReportBll
    {
        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationReportViewModel> GetList();
        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <param name="periodItemId">The period item identity target.</param>
        /// <returns></returns>
        IEnumerable<EvaluationReportViewModel> GetList(int periodItemId);
        /// <summary>
        /// Export vendor evaluation report
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        ResponseFileModel EvaluationExportReport(int id);
    }
}
