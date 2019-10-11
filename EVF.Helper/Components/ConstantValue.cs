using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Components
{
    public static class ConstantValue
    {
        //Claims Type
        public const string ClamisName = "EmpName";
        public const string ClamisEmpNo = "EmpNo";
        public const string ClamisOrg = "OrgId";
        public const string ClamisPosition = "PositionId";
        public const string ClamisComCode = "ComCode";
        public const string ClamisPurchasing = "PurOrg";
        public const string ClamisEncrypt = "UlZaR1gxTkZRMUpGVkE9PQ==";
        //Response Header Content Type Format
        public const string ContentTypeJson = "application/json";
        public const string BasicAuthentication = "BasicAuthentication";
        //User Evaluator Type
        public const string UserTypePurchasing = "P";
        public const string UserTypeEvaluator = "U";
        //Date format
        public const string DateTimeFormat = "yyyy-MM-dd";
        //Template format.
        public const string EmpTemplate = "{0} {1}";
        //Http Method Type.
        public const string HttpMethodPost = "POST";
        //Value Help Constant value type
        public const string ValueTypeActiveStatus = "ACTIVE_STATUS";
        public const string ValueTypeSAPScoreFields = "SAPScoreFields";
        public const string ValueTypePeriodRound = "PeriodRound";
        public const string ValueTypeEvaStatus = "EvaStatus";
        public const string ValueTypeVendorSendingStatus = "VendorSendingStatus";
        public const string ValueTypePurchaseUserType = "PurchaseUserType";
        public const string ValueTypeWeightingKey = "WeightingKey";
        public const string ValueTypeLevelPointCalculate = "LevelPointCalculate";
        public const string ValueTypeVendorFilterCondition = "VendorFilterCondition";
        //Regular expresstion format date
        public const string RegexDateFormat = @"^[0-9]{4}-[0-9]{2}-[0-9]{2}$";
        public const string RegexYearFormat = @"^[0-9]{4}$";
        //Role and Menu.
        public const string RootMenuCode = "ROOT";
        public const string GroupMenuCode = "GROUP";
        public const string ItemMenuCode = "ITEM";
        public const string RoleDisplay = "Role_DS_";
        public const string RoleManage = "Role_MA_";
        //Evaluation Status
        public const string EvaWaiting = "EvaWaiting";
        public const string EvaComplete = "EvaComplete";
        public const string EvaExpire = "EvaExpire";
        //Sending vendor status
        public const string VendorWaiting = "WAITING";
        public const string VendorSending = "SEND";
        //Vendor Condition
        public const string VendorConditionMoreThan = "MoreThan";
        public const string VendorConditionOther = "Other";
        //Purchasing Type
        public const string PurchasingTypeAdmin = "A";
        public const string PurchasingTypeUser = "U";
        //Activity Workflow
        public const string ActivityRequest = "SendRequest";
        public const string ActivityApprove = "Approve";
        public const string WorkflowStatusInWorkflowProcess = "InWfProcess";
        public const string WorkflowStatusApproved = "Approved";
        public const string WorkflowStatusReject = "Reject";
        public const string WorkflowStatusComplete = "Complete";
        public const string WorkflowStatusSendEmail = "SendEmail";
        public const string WorkflowStatusPrint = "Print";
        //K2 set out of office action
        public const string K2SharingCreate = "CREATE";
        public const string K2SharingEdit = "EDIT";
        public const string K2SharingDelete = "DELETE";
        //Workflow Action
        public const string WorkflowActionSendRequest = "SendRequest";
        public const string WorkflowActionApprove = "Approve";
        public const string WorkflowActionReject = "Reject";
        //Workflow Process Code
        public const string EvaluationProcessCode = "EvaluationProcess";
        //Data Fields key
        public const string DataFieldsKeyActionUser = "ActionUser";
        public const string DataFieldsKeyProcessCode = "ProcessCode";
        public const string DataFieldsKeyDataID = "DataID";
        public const string DataFieldsKeyCurrentStep = "CurrentStep";
        public const string DataFieldsKeyRequesterUser = "RequesterUser";
        public const string DataFieldsKeyRequesterCode = "RequesterCode";
        public const string DataFieldsKeyRequesterPos = "RequesterPos";
        public const string DataFieldsKeyRequesterOrg = "RequesterOrg";
        public const string DataFieldsKeyGoNextActivity = "GoNextActivity";
        public const string DataFieldsKeyReceivedDate = "ReceivedDate2";
        //Error Log Messages.
        public const string HrEmployeeArgumentNullExceptionMessage = "The {0} hasn't in HrEmployee Table.";
        public const string HttpRequestFailedMessage = "Response StatusCode {0}, {1}";
        public const string DateIncorrectFormat = "The date value can't be empty and support only 'yyyy-MM-dd' format.";
        public const string YearIncorrectFormat = "The year value can't be empty and support only 'yyyy' format.";
        public const string ArgullmentNullOrEmptyMessage = "parameter can't be null or empty.";
        public const string HttpBadRequestMessage = "The request model is invalid.";
    }
}
