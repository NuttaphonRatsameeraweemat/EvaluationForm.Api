using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Components
{
    public static class ConstantValue
    {
        //Claims Type
        public const string CLAMIS_NAME = "EmpName";
        //Template format.
        public const string EMP_TEMPLATE = "{0} {1}";
        //Http Method Type.
        public const string HttpMethodPost = "POST";
        //Role and Menu.
        public const string RootMenuCode = "ROOT";
        //Error Log Messages.
        public const string HrEmployeeArgumentNullExceptionMessage = "The {0} hasn't in HrEmployee Table.";
    }
}
