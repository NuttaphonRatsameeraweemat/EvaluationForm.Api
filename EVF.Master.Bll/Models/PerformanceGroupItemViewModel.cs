using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class PerformanceGroupItemViewModel
    {
        public int Id { get; set; }
        public int PerformanceGroupId { get; set; }
        public int PerformanceItemId { get; set; }
        public int Sequence { get; set; }
    }
}
