using EVF.CentralSetting.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.CentralSettingTest
{
    public class HolidayCalendarBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The HolidayCalendar service manager provides HolidayCalendar service functionality.
        /// </summary>
        private IHolidayCalendarBll _holiday;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public HolidayCalendarBllTest(IoCConfig io)
        {
            _holiday = io.ServiceProvider.GetRequiredService<IHolidayCalendarBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _holiday.GetList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("2018")]
        [InlineData("2019")]
        [InlineData("2020")]
        public void GetDetail(string year)
        {
            try
            {
                var response = _holiday.GetDetail(year);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("2018")]
        public void Save(string year)
        {
            try
            {
                var response = _holiday.Save(new CentralSetting.Bll.Models.HolidayCalendarViewModel
                {
                    Year = year,
                    HolidayList = new List<CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail>
                    {
                        new CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail{ HolidayDateString = "2018-01-01", Description = "ปีใหม่" },
                        new CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail{ HolidayDateString = "2018-01-02", Description = "ปีใหม่" },
                        new CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail{ HolidayDateString = "2018-01-03", Description = "ปีใหม่" }
                    }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("2018")]
        public void Edit(string year)
        {
            try
            {
                var response = _holiday.Edit(new CentralSetting.Bll.Models.HolidayCalendarViewModel
                {
                    Year = year,
                    HolidayList = new List<CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail>
                    {
                        new CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail{ HolidayDateString = "2018-01-01", Description = "ปีใหม่" },
                        new CentralSetting.Bll.Models.HolidayCalendarViewModel.HolidayCalendarDetail{ HolidayDateString = "2018-01-02", Description = "ปีใหม่" }
                    }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("2018")]
        public void Delete(string year)
        {
            try
            {
                var response = _holiday.Delete(new CentralSetting.Bll.Models.HolidayDeleteRequestModel { Year = year });
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
