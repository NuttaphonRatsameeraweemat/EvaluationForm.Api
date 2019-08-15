using EVF.Bll.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Bll.Models
{
    public class HolidayCalendarViewModel
    {
        public HolidayCalendarViewModel()
        {
            HolidayList = new List<HolidayCalendarDetail>();
        }

        public const string RoleForManageData = "Role_MA_HolidayCalendar";
        public const string RoleForDisplayData = "Role_DS_HolidayCalendar";

        [Required]
        [RegularExpression(ConstantValue.RegexYearFormat, ErrorMessage = ConstantValue.YearIncorrectFormat)]
        public string Year { get; set; }
        public List<HolidayCalendarDetail> HolidayList { get; set; }
        
        public class HolidayCalendarDetail
        {
            [Required]
            [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
            public string HolidayDateString { get; set; }
            public string Description { get; set; }
            public DateTime HolidayDate { get; set; }
        }

    }
}
