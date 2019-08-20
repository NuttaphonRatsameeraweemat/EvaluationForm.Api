using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Authorization.Bll.Models
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool DisplayOnly { get; set; }
        public string Url { get; set; }
        public List<MenuViewModel> Parent { get; set; }
    }
}
