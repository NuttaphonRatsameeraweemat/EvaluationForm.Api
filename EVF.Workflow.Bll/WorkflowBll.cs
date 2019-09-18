using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Workflow.Bll
{
    public class WorkflowBll : IWorkflowBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The k2 service provides k2 service functionality.
        /// </summary>
        private readonly IK2Service _k2Service;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="k2Service">The k2 service provides k2 service functionality.</param>
        public WorkflowBll(IUnitOfWork unitOfWork, IK2Service k2Service, IManageToken token, IConfigSetting config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _k2Service = k2Service;
            _token = token;
            _config = config;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Start workflow process.
        /// </summary>
        /// <param name="dataId">The identity process.</param>
        /// <param name="processCode">The process code.</param>
        /// <param name="folio">The folio name.</param>
        /// <param name="approvalStep">The approval list.</param>
        public void Start(int dataId, string processCode, string folio, Dictionary<int, string> approvalStep)
        {
            Dictionary<string, object> dataFields = new Dictionary<string, object>
            {
                { "ActionUser", this.GetActionUser(approvalStep,2) },
                { "ProcessCode", processCode },
                { "DataID", dataId },
                { "CurrentStep", 2 },
                { "RequesterUser", _token.AdUser },
                { "RequesterCode", _token.EmpNo },
                { "RequesterPos", _token.PositionId },
                { "RequesterOrg", _token.OrgId },
                { "GoNextActivity", false }
            };
            int processIntanceId = _k2Service.StartWorkflow(this.GetProcessName(processCode), folio, dataFields);
            this.SaveWorkflowProcessInstance(processIntanceId, dataId, 2, processCode);
            this.SaveWorkflowStep(this.CombineRequesterAndApproval(processIntanceId, approvalStep));
            this.SaveWorkflowLog(this.InitialWorkflowLog(processIntanceId, 1, ConstantValue.WorkflowActionSendRequest, ConstantValue.WorkflowActionSendRequest));
        }

        /// <summary>
        /// Action workflow by outcome in workflow.
        /// </summary>
        /// <param name="processIntanceId">The process instance id workflow.</param>
        /// <param name="serialNo">The serial no identity task.</param>
        /// <param name="step">The workflow task step.</param>
        /// <param name="action">The action to execute workflow.</param>
        /// <param name="comment">The comment workflow task.</param>
        public void Action(int processIntanceId, string serialNo, int step, string action, string comment)
        {
            int nextStep = step + 1;
            string processInstanceStatus = ConstantValue.WorkflowStatusInWorkflowProcess;
            Dictionary<string, object> dataFields = new Dictionary<string, object>();
            if (this.IsWorkflowFisnish(processIntanceId, nextStep))
            {
                dataFields.Add("GoNextActivity", false);
                dataFields.Add("ActionUser", this.GetCurrentApprove(processIntanceId, nextStep));
                dataFields.Add("CurrentStep", nextStep);
            }
            else
            {
                dataFields.Add("GoNextActivity", true);
                nextStep = 0;
                processInstanceStatus = ConstantValue.WorkflowStatusComplete;
            }
            _k2Service.ActionWorkflow(serialNo, action, dataFields);
            this.UpdateWorkflowProcessInstance(processIntanceId, nextStep, processInstanceStatus);
            this.SaveWorkflowLog(this.InitialWorkflowLog(processIntanceId, step, serialNo, action, comment));
        }

        /// <summary>
        /// Action mutiple task.
        /// </summary>
        /// <param name="models">The task list information.</param>
        public void MultipleTaskAction(IEnumerable<WorkflowViewModel> models)
        {
            foreach (var item in models)
            {
                int nextStep = item.Step + 1;
                Dictionary<string, object> dataFields = new Dictionary<string, object>();
                if (this.IsWorkflowFisnish(item.ProcessInstanceId, nextStep))
                {
                    dataFields.Add("GoNextActivity", false);
                    dataFields.Add("ActionUser", this.GetCurrentApprove(item.ProcessInstanceId, nextStep));
                    dataFields.Add("CurrentStep", nextStep);
                    item.SetStatus(ConstantValue.WorkflowStatusInWorkflowProcess);
                }
                else
                {
                    dataFields.Add("GoNextActivity", true);
                    nextStep = 0;
                    item.SetStatus(ConstantValue.WorkflowStatusComplete);
                }
                item.SetDataFields(dataFields);
            }
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Insert workflow task first to last step approve.
        /// </summary>
        /// <param name="workflowActivitySteps">The requester and approval list.</param>
        private void SaveWorkflowStep(IEnumerable<WorkflowActivityStep> workflowActivitySteps)
        {
            _unitOfWork.GetRepository<WorkflowActivityStep>().AddRange(workflowActivitySteps);
        }

        /// <summary>
        /// Insert workflow log when action stamp.
        /// </summary>
        /// <param name="workflowActivityLogs"></param>
        private void SaveWorkflowLog(WorkflowActivityLog workflowActivityLogs)
        {
            _unitOfWork.GetRepository<WorkflowActivityLog>().Add(workflowActivityLogs);
        }

        /// <summary>
        /// Insert workflow process instance.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="dataId">The topic identity.</param>
        /// <param name="step">The current step.</param>
        /// <param name="processCode">The topic process.</param>
        private void SaveWorkflowProcessInstance(int processInstanceId, int dataId, int step, string processCode)
        {
            var processInstance = new WorkflowProcessInstance
            {
                ProcessInstanceId = processInstanceId,
                ProcessCode = processCode,
                DataId = dataId,
                CurrentStep = step,
                Status = ConstantValue.WorkflowStatusInWorkflowProcess
            };
            _unitOfWork.GetRepository<WorkflowProcessInstance>().Add(processInstance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="step"></param>
        /// <param name="status"></param>
        private void UpdateWorkflowProcessInstance(int processInstanceId, int step, string status)
        {
            var processInstance = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.ProcessInstanceId == processInstanceId).FirstOrDefault();
            processInstance.CurrentStep = step;
            processInstance.Status = status;
            _unitOfWork.GetRepository<WorkflowProcessInstance>().Update(processInstance);
        }

        /// <summary>
        /// Combine requester and approval workflow step.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="approvalStep">The approval list.</param>
        /// <returns></returns>
        private IEnumerable<WorkflowActivityStep> CombineRequesterAndApproval(int processInstanceId, Dictionary<int, string> approvalStep)
        {
            var result = new List<WorkflowActivityStep>
            {
                new WorkflowActivityStep
                {
                    Step = 1,
                    Activity = ConstantValue.ActivityRequest,
                    ProcessInstanceId = processInstanceId,
                    ActionUser = _token.AdUser,
                    ActionUserCode = _token.EmpNo,
                    ActionUserOrg = _token.OrgId,
                    ActionUserPosition = _token.PositionId
                }
            };
            result.AddRange(this.GetApprovalInfo(processInstanceId, approvalStep));
            return result;
        }

        /// <summary>
        /// Initial approval information.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="approvalStep">The approval list.</param>
        /// <returns></returns>
        private IEnumerable<WorkflowActivityStep> GetApprovalInfo(int processInstanceId, Dictionary<int, string> approvalStep)
        {
            var result = new List<WorkflowActivityStep>();
            string[] adUserList = approvalStep.Select(x => x.Value).ToArray();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache(x => adUserList.Contains(x.Aduser));
            foreach (var item in approvalStep)
            {
                var temp = empList.FirstOrDefault(x => x.Aduser == item.Value);
                result.Add(new WorkflowActivityStep
                {
                    Step = item.Key + 1,
                    Activity = ConstantValue.ActivityApprove,
                    ProcessInstanceId = processInstanceId,
                    ActionUser = temp.Aduser,
                    ActionUserCode = temp.EmpNo,
                    ActionUserOrg = temp.OrgId,
                    ActionUserPosition = temp.PositionId
                });
            }

            return result;
        }

        /// <summary>
        /// Initial log workflow action.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="step">The current step log.</param>
        /// <param name="serailNo">The serial no task action.</param>
        /// <param name="action">The action log.</param>
        /// <param name="commment">The comment action.</param>
        /// <returns></returns>
        private WorkflowActivityLog InitialWorkflowLog(int processInstanceId, int step, string serailNo, string action, string commment = null)
        {
            return new WorkflowActivityLog
            {
                ProcessInstanceId = processInstanceId,
                Step = step,
                SerialNo = serailNo,
                Action = action,
                ActionDate = DateTime.Now,
                ActionUser = _token.AdUser,
                ActionUserCode = _token.EmpNo,
                ActionUserOrg = _token.OrgId,
                ActionUserPosition = _token.PositionId,
                AllocatedUser = this.GetCurrentApprove(processInstanceId, step),
                Comment = commment
            };
        }

        /// <summary>
        /// Get current approve step.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="step">The target step.</param>
        /// <returns></returns>
        private string GetCurrentApprove(int processInstanceId, int step)
        {
            string result = string.Empty;
            var data = _unitOfWork.GetRepository<WorkflowActivityStep>().Get(x => x.ProcessInstanceId == processInstanceId &&
                                                                                       x.Step == step).FirstOrDefault();
            return data?.ActionUser;
        }

        /// <summary>
        /// Validate workflow is finish or not.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instances id.</param>
        /// <param name="nextStep">The workflow next step.</param>
        /// <returns></returns>
        private bool IsWorkflowFisnish(int processInstanceId, int nextStep)
        {
            int maxStep = _unitOfWork.GetRepository<WorkflowActivityStep>().Get(x => x.ProcessInstanceId == processInstanceId).Max(x => x.Step);
            if (nextStep > maxStep)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get workflow process name.
        /// </summary>
        /// <param name="processCode">The workflow process code.</param>
        /// <returns></returns>
        private string GetProcessName(string processCode)
        {
            string result = string.Empty;
            switch (processCode)
            {
                case ConstantValue.EvaluationProcessCode:
                    result = _config.SpeEvaluationProcess;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Get action user from approval list.
        /// </summary>
        /// <param name="approvals">The approval list.</param>
        /// <param name="step">The target step.</param>
        /// <returns></returns>
        private string GetActionUser(Dictionary<int, string> approvals, int step)
        {
            string result = string.Empty;
            result = approvals.FirstOrDefault(x => x.Key == step).Value;
            return result;
        }

        #endregion

    }
}
