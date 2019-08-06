using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Components
{
    public static class ConstantValue
    {
        //Claims Type
        public const string ClamisName = "EmpName";
        public const string ClamisEmpNo = "EmpName";
        public const string ClamisEncrypt = "UlZaR1gxTkZRMUpGVkE9PQ==";
        //Response Header Content Type Format
        public const string ContentTypeJson = "application/json";
        public const string BasicAuthentication = "BasicAuthentication";
        //Template format.
        public const string EmpTemplate = "{0} {1}";
        //Http Method Type.
        public const string HttpMethodPost = "POST";
        //Role and Menu.
        public const string RootMenuCode = "ROOT";
        public const string GroupMenuCode = "GROUP";
        public const string ItemMenuCode = "ITEM";
        public const string RoleDisplay = "Role_DS_";
        public const string RoleManage = "Role_MA_";
        //Error Log Messages.
        public const string HrEmployeeArgumentNullExceptionMessage = "The {0} hasn't in HrEmployee Table.";
        public const string HttpRequestFailedMessage = "Response StatusCode {0}, {1}";
    }
}
