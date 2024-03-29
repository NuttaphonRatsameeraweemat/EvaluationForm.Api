﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EmailTemplate
    {
        [Key]
        [StringLength(100)]
        public string EmailType { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
