using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Models
{
    public class TableServerSideModel<T> where T : class
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public T SearchProperty { get; set; }
    }
}
