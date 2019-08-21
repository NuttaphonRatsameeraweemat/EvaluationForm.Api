using EVF.Master.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.MasterTest
{
    public class PerformanceGroupBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The PerformanceGroup service manager provides PerformanceGroup service functionality.
        /// </summary>
        private IPerformanceGroupBll _performanceGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public PerformanceGroupBllTest(IoCConfig io)
        {
            _performanceGroup = io.ServiceProvider.GetRequiredService<IPerformanceGroupBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _performanceGroup.GetList();
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
                var response = _performanceGroup.GetDetail(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("UnitTest PerformanceGroup")]
        public void Save(string performanceGroupName)
        {
            try
            {
                var response = _performanceGroup.Save(new Master.Bll.Models.PerformanceGroupViewModel
                {
                    PerformanceGroupNameTh = performanceGroupName,
                    PerformanceGroupItems = new List<int> { 1, 2, 3 }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1, "UnitTest PerformanceGroup Edit")]
        public void Edit(int id, string performanceGroupName)
        {
            try
            {
                var response = _performanceGroup.Save(new Master.Bll.Models.PerformanceGroupViewModel
                {
                    Id = id,
                    PerformanceGroupNameTh = performanceGroupName,
                    PerformanceGroupItems = new List<int> { 1, 2 }
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
                var response = _performanceGroup.Delete(id);
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
