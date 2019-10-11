using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorLineChart
    {
        public VendorLineChart()
        {
            DataStats = new List<VendorLineChartData>();
        }

        public string[] AllPeriods { get; set; }
        public List<VendorLineChartData> DataStats { get; set; }
    }

    public class VendorLineChartData
    {
        public string PeriodName { get; set; }
        public int TotalScore { get; set; }
    }
}
