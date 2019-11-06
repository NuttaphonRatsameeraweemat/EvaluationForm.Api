using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class ReportTemplate
    {
        [StringLength(30)]
        public string ProcessCode { get; set; }
        [StringLength(100)]
        public string TemplateName { get; set; }
        public string Content1Th { get; set; }
        public string Content1En { get; set; }
        public string Content2Th { get; set; }
        public string Content2En { get; set; }
        public string Content3Th { get; set; }
        public string Content3En { get; set; }
        public string Content4Th { get; set; }
        public string Content4En { get; set; }
        public string Content5Th { get; set; }
        public string Content5En { get; set; }
    }
}
