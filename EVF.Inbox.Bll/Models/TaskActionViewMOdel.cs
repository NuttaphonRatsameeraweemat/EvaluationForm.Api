using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Models
{
    public class TaskActionViewModel
    {
        public string ProcessCode { get; set; }
        public int DataId { get; set; }
        public string SerialNumber { get; set; }
        public int Step { get; set; }
        public string Comment { get; set; }
    }
}
