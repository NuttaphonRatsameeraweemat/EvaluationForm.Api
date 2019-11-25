using EVF.Report.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.ReportTest
{
    public class InvestigateEvaluationReportBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The evaluation summary report service manager provides evaluation summary report service functionality.
        /// </summary>
        private IInvestigateEvaluationReportBll _evaluationSummaryReport;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="InvestigateEvaluationReportBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public InvestigateEvaluationReportBllTest(IoCConfig io)
        {
            _evaluationSummaryReport = io.ServiceProvider.GetRequiredService<IInvestigateEvaluationReportBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("", null, null, "", "", null)]
        public void ExportSummaryReport(string comCode, int[] year, int[] periodItemId, 
                                        string purchaseOrg, string weightingKey, string[] status)
        {
            try
            {
                var response = _evaluationSummaryReport.ExportSummaryReport(new Report.Bll.Models.InvestigateEvaluationReportRequestModel
                {
                    ComCode = comCode,
                    Year = year,
                    PeriodItemId = periodItemId,
                    PurchaseOrg = purchaseOrg,
                    WeightingKey = weightingKey,
                    Status = status
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
