using EVF.Report.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.ReportTest
{
    public class EvaluationSummaryReportBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The evaluation summary report service manager provides evaluation summary report service functionality.
        /// </summary>
        private IEvaluationSummaryReportBll _evaluationSummaryReport;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationSummaryReportBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public EvaluationSummaryReportBllTest(IoCConfig io)
        {
            _evaluationSummaryReport = io.ServiceProvider.GetRequiredService<IEvaluationSummaryReportBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("", null, null, "", "")]
        [InlineData("1600", null, 22, "", "")]
        [InlineData("1600", 12, null, "", "")]
        [InlineData("1600", 12, 22, "1600", "A2")]
        [InlineData("1600", 12, 22, "1600", "A3")]
        [InlineData("1600", 12, 22, "1600", "A4")]
        [InlineData("1600", 12, 22, "1600", "A5")]
        public void GetList(string comCode, int? periodId, int? periodItemId, string purchaseOrg, string weightingKey)
        {
            try
            {
                _evaluationSummaryReport.ExportSummaryReport(new Report.Bll.Models.EvaluationSummaryReportRequestModel
                {
                    ComCode = comCode,
                    PeriodId = periodId,
                    PeriodItemId = periodItemId,
                    PurchaseOrg = purchaseOrg,
                    WeightingKey = weightingKey
                });
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
