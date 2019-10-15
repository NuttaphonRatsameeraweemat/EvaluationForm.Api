using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Models
{
    public class ResponseFileModel
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
