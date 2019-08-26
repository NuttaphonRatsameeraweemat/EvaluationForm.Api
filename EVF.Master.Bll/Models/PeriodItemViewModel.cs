using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class PeriodItemViewModel
    {
        public int Id { get; set; }
        public int PeriodID { get; set; }
        public int Round { get; set; }
        public DateTime StartEvaDate { get; set; }
        public DateTime EndEvaDate { get; set; }
    }
}
