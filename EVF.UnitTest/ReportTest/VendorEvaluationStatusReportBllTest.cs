using EVF.Report.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace EVF.UnitTest.ReportTest
{
    public class VendorEvaluationStatusReportBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The vendor evaluation status report service manager provides vendor evaluation status report service functionality.
        /// </summary>
        private IVendorEvaluationStatusReportBll _vendorEvaluationStatusReport;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorEvaluationStatusReportBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public VendorEvaluationStatusReportBllTest(IoCConfig io)
        {
            _vendorEvaluationStatusReport = io.ServiceProvider.GetRequiredService<IVendorEvaluationStatusReportBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("", null, null, "", "")]
        //[InlineData("1600", null, 22, "", "")]
        //[InlineData("1600", 12, null, "", "")]
        //[InlineData("1600", 12, 22, "1600", "A2")]
        //[InlineData("1600", 12, 22, "1600", "A3")]
        //[InlineData("1600", 12, 22, "1600", "A4")]
        //[InlineData("1600", 12, 22, "1600", "A5")]
        public void ExportSummaryReport(string comCode, int? periodId, int? periodItemId, string purchaseOrg, string weightingKey)
        {
            try
            {
                var response = _vendorEvaluationStatusReport.ExportVendorEvaluationStatusReport(new Report.Bll.Models.EvaluationSummaryReportRequestModel
                {
                    ComCode = comCode,
                    PeriodId = periodId,
                    PeriodItemId = periodItemId,
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
