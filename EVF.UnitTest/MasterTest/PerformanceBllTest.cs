using EVF.Master.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.MasterTest
{
    public class PerformanceBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The Performance service manager provides Performance service functionality.
        /// </summary>
        private IPerformanceBll _performance;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public PerformanceBllTest(IoCConfig io)
        {
            _performance = io.ServiceProvider.GetRequiredService<IPerformanceBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _performance.GetList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetAllMenu(int id)
        {
            try
            {
                var response = _performance.GetDetail(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("UnitTest Performance")]
        public void Save(string performanceName)
        {
            try
            {
                var response = _performance.Save(new Master.Bll.Models.PerformanceViewModel
                {
                    PerformanceNameTh = performanceName
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1, "UnitTest Performance Edit")]
        public void Edit(int id, string performanceName)
        {
            try
            {
                var response = _performance.Edit(new Master.Bll.Models.PerformanceViewModel
                {
                    Id = id,
                    PerformanceNameTh = performanceName
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Delete(int id)
        {
            try
            {
                var response = _performance.Delete(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}
