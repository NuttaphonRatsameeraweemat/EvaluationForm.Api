using EVF.Evaluation.Bll.Interfaces;
using EVF.Helper.Components;
using EVF.Helper.Models;
using EVF.Inbox.Bll.Interfaces;
using EVF.Inbox.Bll.Models;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll
{
    public class TaskActionBll : ITaskActionBll
    {

        #region [Fields]

        /// <summary>
        /// The summary evaluation manager provides summary evaluation functionality.
        /// </summary>
        private readonly ISummaryEvaluationBll _summaryEvaluation;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskActionBll" /> class.
        /// </summary>
        /// <param name="summaryEvaluation">The summary evaluation manager provides summary evaluation functionality.</param>
        public TaskActionBll(ISummaryEvaluationBll summaryEvaluation)
        {
            _summaryEvaluation = summaryEvaluation;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Process task action k2.
        /// </summary>
        /// <param name="model">The task action information.</param>
        /// <param name="action">The action value.</param>
        /// <returns></returns>
        public ResultViewModel ActionTask(TaskActionViewModel model, string action)
        {
            var result = new ResultViewModel();
            switch (model.ProcessCode)
            {
                case ConstantValue.EvaluationProcessCode:
                    result = _summaryEvaluation.SubmitAction(this.InitialWorkflowViewModel(model, action));
                    break;
            }
            return result;
        }

        public void ActionMultiTask()
        {

        }

        /// <summary>
        /// Inital Workflow view model.
        /// </summary>
        /// <param name="model">The task action model.</param>
        /// <param name="action">The action value.</param>
        /// <returns></returns>
        private WorkflowViewModel InitialWorkflowViewModel(TaskActionViewModel model, string action)
        {
            return new WorkflowViewModel
            {
                Action = action,
                Comment = model.Comment,
                DataId = model.DataId,
                ProcessInstanceId = this.GetProcessInstancesId(model.SerialNumber),
                SerialNo = model.SerialNumber,
                Step = model.Step
            };
        }

        /// <summary>
        /// Get process instance id from serial number.
        /// </summary>
        /// <param name="serialNumber">The serial number action.</param>
        /// <returns></returns>
        private int GetProcessInstancesId(string serialNumber)
        {
            var stringSpilt = serialNumber.Split('_', StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToInt32(stringSpilt[0]);
        }

        #endregion

    }
}
