using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class KpiGroupItemViewModel
    {
        public int Id { get; set; }
        public int KpiGroupId { get; set; }
        public int KpiId { get; set; }
        public int Sequence { get; set; }
    }
}
