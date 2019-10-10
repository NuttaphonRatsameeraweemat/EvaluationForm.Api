﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Models
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {

        }
        //Data for post back when action task.
        public string SerialNumber { get; set; }
        public string ProcessCode { get; set; }
        public int Step { get; set; }
        public int DataId { get; set; }

        //Display information
        public string ProcessName { get; set; }
        public string VendorName { get; set; }
        public string PurchaseOrgName { get; set; }
        public int TotalScore { get; set; }
        public string GradeName { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
