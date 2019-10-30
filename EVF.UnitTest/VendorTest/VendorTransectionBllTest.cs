using EVF.Vendor.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.VendorTest
{
    public class VendorTransectionBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The vendpr transaction service manager provides vendor transaction service functionality.
        /// </summary>
        private IVendorTransectionBll _vendorTransection;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorTransectionBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public VendorTransectionBllTest(IoCConfig io)
        {
            _vendorTransection = io.ServiceProvider.GetRequiredService<IVendorTransectionBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var list = _vendorTransection.GetList();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void GetListSearch()
        {
            try
            {
                var list = _vendorTransection.GetListSearch(new Vendor.Bll.Models.VendorTransectionSearchViewModel { StartDate = "2019-10-24", EndDate = "2019-10-25", PurGroup = "311" });
                var list2 = _vendorTransection.GetListSearchElastic(new Vendor.Bll.Models.VendorTransectionSearchViewModel { StartDate = "2019-10-24", EndDate = "2019-10-25", PurGroup = "311" });
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void GetTransections()
        {
            try
            {
                var list = _vendorTransection.GetTransections("2018-01-29", "2019-10-29", new string[] { "311", "911" }, "1600", "1600");
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void ReImportTransaction()
        {
            try
            {
                _vendorTransection.ReImportTransaction();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ReImportTransactionById(int id)
        {
            try
            {
                _vendorTransection.ReImportTransactionById(id);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BulkVendorTransaction()
        {
            try
            {
                _vendorTransection.BulkVendorTransaction();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void BulkUpdateVendorTransaction()
        {
            try
            {
                _vendorTransection.BulkUpdateVendorTransaction();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
