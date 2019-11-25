using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Workflow.Bll
{
    public class WorkflowActivityBll : IWorkflowActivityBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowActivityBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public WorkflowActivityBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get workflow activity logs.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public IEnumerable<WorkflowActivityViewModel> GetWorkflowActivity(int id)
        {
            var result = new List<WorkflowActivityViewModel>();

            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeEvaStatus);

            var processInstances = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.DataId == id && x.ProcessCode == ConstantValue.EvaluationProcessCode);

            foreach (var item in processInstances)
            {
                var workflowSteps = _unitOfWork.GetRepository<WorkflowActivityStep>().GetCache(x => x.ProcessInstanceId == item.ProcessInstanceId &&
                                                                                                x.Step > 1);

                foreach (var workflowStep in workflowSteps)
                {
                    var workflowLog = _unitOfWork.GetRepository<WorkflowActivityLog>().Get(x => x.ProcessInstanceId == workflowStep.ProcessInstanceId &&
                                                                                              x.Step == workflowStep.Step).FirstOrDefault();

                    if (workflowLog != null)
                    {
                        result.Add(new WorkflowActivityViewModel
                        {
                            ActionDate = UtilityService.DateTimeToString(workflowLog.ActionDate.Value, ConstantValue.DateTimeFormat),
                            FullName = this.GetFullName(workflowLog.AllocatedUser, workflowLog.ActionUser, empList),
                            Reason = workflowLog.Comment,
                            Status = workflowLog.Action == ConstantValue.WorkflowActionApprove ? "อนุมัติ" : "ไม่อนุมัติ"
                        });
                    }
                    else
                    {
                        result.Add(new WorkflowActivityViewModel
                        {
                            ActionDate = string.Empty,
                            FullName = this.GetFullName("", workflowStep.ActionUser, empList),
                            Reason = "",
                            Status = "รอการอนุมัติ"
                        });
                    }

                }

            }

            return result;
        }

        /// <summary>
        /// Get employee action name.
        /// </summary>
        /// <param name="actionUser">The action user.</param>
        /// <param name="allocatedUser">The owner task.</param>
        /// <param name="empList">The employee collection.</param>
        /// <returns></returns>
        private string GetFullName(string actionUser, string allocatedUser, IEnumerable<Hremployee> empList)
        {
            string result = string.Empty;
            var emp = empList.FirstOrDefault(x => x.Aduser == allocatedUser);
            if (!string.IsNullOrEmpty(actionUser) && !string.Equals(actionUser, allocatedUser, StringComparison.OrdinalIgnoreCase))
            {
                var actionEmp = empList.FirstOrDefault(x => x.Aduser == actionUser);
                result += $"{string.Format($"คุณ{ConstantValue.EmpTemplate}", actionEmp?.FirstnameTh, actionEmp?.LastnameTh)} อนุมัติแทน ";
            }
            result += string.Format($"คุณ{ConstantValue.EmpTemplate}", emp?.FirstnameTh, emp?.LastnameTh);
            return result;
        }

        #endregion

    }
}
