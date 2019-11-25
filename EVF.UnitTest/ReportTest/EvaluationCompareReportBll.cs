using EVF.Report.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.ReportTest
{
    public class EvaluationCompareReportBll : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The evaluation compare report service manager provides evaluation compare report service functionality.
        /// </summary>
        private IEvaluationCompareReportBll _evaluationSummaryReport;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationCompareReportBll" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public EvaluationCompareReportBll(IoCConfig io)
        {
            _evaluationSummaryReport = io.ServiceProvider.GetRequiredService<IEvaluationCompareReportBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("", null, null, "", "")]
        [InlineData("", new int[] { 2016 }, null, "", "")]
        public void ExportSummaryReport(string comCode, int[] year, int[] periodItemIds, string purchaseOrg, string weightingKey)
        {
            try
            {
                var response = _evaluationSummaryReport.ExportEvaluationCompareReport(new Report.Bll.Models.EvaluationCompareReportRequestModel
                {
                    ComCode = comCode,
                    Year = year,
                    PeriodItemId = periodItemIds,
                    PurchaseOrg = purchaseOrg,
                    WeightingKey = weightingKey
                });

                var filePath = $@"D:\{response.FileName}";
                System.IO.File.WriteAllBytes(filePath, response.FileContent);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
