using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Models
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            IsError = false;
            Message = "Completed";
        }

        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
