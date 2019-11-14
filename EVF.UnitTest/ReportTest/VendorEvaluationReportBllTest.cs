using EVF.Report.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.ReportTest
{
    public class VendorEvaluationReportBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The evaluation summary report service manager provides evaluation summary report service functionality.
        /// </summary>
        private IVendorEvaluationReportBll _vendorEvaluationReport;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationSummaryReportBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public VendorEvaluationReportBllTest(IoCConfig io)
        {
            _vendorEvaluationReport = io.ServiceProvider.GetRequiredService<IVendorEvaluationReportBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void SendVendorEvaluaitonReportEmail()
        {
            try
            {
                _vendorEvaluationReport.SendVendorEvaluaitonReportEmail(3);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
