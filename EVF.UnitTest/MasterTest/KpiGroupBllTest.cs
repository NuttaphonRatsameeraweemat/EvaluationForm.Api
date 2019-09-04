using EVF.Master.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.MasterTest
{
    public class KpiGroupBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The KpiGroup service manager provides KpiGroup service functionality.
        /// </summary>
        private IKpiGroupBll _KpiGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public KpiGroupBllTest(IoCConfig io)
        {
            _KpiGroup = io.ServiceProvider.GetRequiredService<IKpiGroupBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _KpiGroup.GetList();
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
                var response = _KpiGroup.GetDetail(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("UnitTest KpiGroup")]
        public void Save(string KpiGroupName)
        {
            try
            {
                var response = _KpiGroup.Save(new Master.Bll.Models.KpiGroupViewModel
                {
                    KpiGroupNameTh = KpiGroupName
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1, "UnitTest KpiGroup Edit")]
        public void Edit(int id, string KpiGroupName)
        {
            try
            {
                var response = _KpiGroup.Save(new Master.Bll.Models.KpiGroupViewModel
                {
                    Id = id,
                    KpiGroupNameTh = KpiGroupName
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
                var response = _KpiGroup.Delete(id);
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
