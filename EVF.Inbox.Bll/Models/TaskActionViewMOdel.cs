using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Inbox.Bll.Models
{
    public class TaskActionViewModel
    {
        [Required]
        public string ProcessCode { get; set; }
        [Required]
        public int DataId { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public int Step { get; set; }
        public string Comment { get; set; }
    }
}
