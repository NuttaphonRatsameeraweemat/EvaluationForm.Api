using EVF.Master.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.MasterTest
{
    public class KpiBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The Kpi service manager provides Kpi service functionality.
        /// </summary>
        private IKpiBll _Kpi;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public KpiBllTest(IoCConfig io)
        {
            _Kpi = io.ServiceProvider.GetRequiredService<IKpiBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _Kpi.GetList();
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
                var response = _Kpi.GetDetail(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("UnitTest Kpi")]
        public void Save(string KpiName)
        {
            try
            {
                var response = _Kpi.Save(new Master.Bll.Models.KpiViewModel
                {
                    KpiNameTh = KpiName
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1, "UnitTest Kpi Edit")]
        public void Edit(int id, string KpiName)
        {
            try
            {
                var response = _Kpi.Edit(new Master.Bll.Models.KpiViewModel
                {
                    Id = id,
                    KpiNameTh = KpiName
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
                var response = _Kpi.Delete(id);
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
