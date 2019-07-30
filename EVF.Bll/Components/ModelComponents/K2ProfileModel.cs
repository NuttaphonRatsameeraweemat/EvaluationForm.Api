using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Components.ModelComponents
{
    public class K2ProfileModel
    {
        public K2ProfileModel()
        {
            Management = false;
            Impersonate = false;
            ImpersonateUser = string.Empty;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Impersonate { get; set; }
        public string ImpersonateUser { get; set; }
        public bool Management { get; set; }
    }
}
