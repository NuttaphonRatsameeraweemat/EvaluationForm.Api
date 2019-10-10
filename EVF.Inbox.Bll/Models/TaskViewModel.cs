using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll.Models
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {

        }

        public string AllocatedUser { get; set; }
        public string Folio { get; set; }
        public DateTime StartDate { get; set; }
        public string Folder { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        
        public string SerialNumber { get; set; }
        public string ProcessCode { get; set; }
        public int Step { get; set; }
    }
}
