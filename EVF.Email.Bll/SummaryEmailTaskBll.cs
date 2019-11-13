using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
using EVF.Email.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Email.Bll
{
    public class SummaryEmailTaskBll : ISummaryEmailTaskBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The email task provides email task functionality.
        /// </summary>
        private readonly IEmailTaskBll _emailTask;
        /// <summary>
        /// The email service provides email service functionality.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILoggerManager _logger;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEmailTaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEmailTaskBll(IUnitOfWork unitOfWork, IManageToken token, IEmailTaskBll emailTask, IEmailService emailService, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _emailTask = emailTask;
            _emailService = emailService;
            _logger = logger;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Execute email status waiting.
        /// </summary>
        public void ExecuteEmailTaskWaiting()
        {
            List<int> emailSuccess = new List<int>();
            List<int> emailFailed = new List<int>();
            this.SendingEmailTask(this.GetEmailTaskList(ConstantValue.EmailTaskStatusWaiting), emailSuccess, emailFailed);
            _emailTask.UpdateEmailTaskStatus(emailSuccess.ToArray(), ConstantValue.EmailTaskStatusSending);
            _emailTask.UpdateEmailTaskStatus(emailFailed.ToArray(), ConstantValue.EmailTaskStatusError);
        }

        /// <summary>
        /// Execute email status error.
        /// </summary>
        public void ExecuteEmailTaskError()
        {
            List<int> emailSuccess = new List<int>();
            List<int> emailFailed = new List<int>();
            this.SendingEmailTask(this.GetEmailTaskList(ConstantValue.EmailTaskStatusError), emailSuccess, emailFailed);
            _emailTask.UpdateEmailTaskStatus(emailSuccess.ToArray(), ConstantValue.EmailTaskStatusSending);
            _emailTask.UpdateEmailTaskStatus(emailFailed.ToArray(), ConstantValue.EmailTaskStatusError);
        }

        /// <summary>
        /// Send email task.
        /// </summary>
        /// <param name="emailTask">The email task collection data.</param>
        /// <param name="emailSuccess">The email identity case success.</param>
        /// <param name="emailFailed">The email identity case failed.</param>
        private void SendingEmailTask(IEnumerable<EmailTaskViewModel> emailTask,
                                      List<int> emailSuccess, List<int> emailFailed)
        {
            foreach (var item in emailTask)
            {
                try
                {
                    _emailService.SendEmail(new Helper.Models.EmailModel
                    {
                        Subject = item.Subject,
                        Body = item.Content,
                        Receiver = string.Join(";", item.Receivers.Select(x => x.Email))
                    });
                    //send success.
                    emailSuccess.Add(item.Id);
                }
                catch (Exception ex)
                {
                    //send failed.
                    emailFailed.Add(item.Id);
                    _logger.LogError(ex, "Error Sending email failed : ");
                }

            }
        }

        /// <summary>
        /// Process summary task email.
        /// </summary>
        public void ProcessSummaryTask()
        {
            var model = this.GetWaitingTask();
            this.SaveToEmailTask(model);
        }

        public void ProcessSummaryTaskReject()
        {

        }

        public void ProcessSummaryTaskEvaWaiting()
        {

        }

        /// <summary>
        /// Get all pending task workflow.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<int>> GetWaitingTask()
        {
            var result = new Dictionary<string, List<int>>();
            var processInstances = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.Status != ConstantValue.WorkflowStatusComplete);
            var processInstancesIds = processInstances.Select(x => x.ProcessInstanceId).ToArray();

            var workflowStepList = _unitOfWork.GetRepository<WorkflowActivityStep>().Get(x => processInstancesIds.Contains(x.ProcessInstanceId));

            foreach (var item in processInstances)
            {
                var workflowStep = workflowStepList.FirstOrDefault(x => x.Step == item.CurrentStep);
                if (workflowStep != null)
                {
                    if (result.Any(x => x.Key == workflowStep.ActionUser))
                    {
                        result[workflowStep.ActionUser].Add(item.DataId.Value);
                    }
                    else result.Add(workflowStep.ActionUser, new List<int> { item.DataId.Value });
                }
            }
            return result;
        }

        /// <summary>
        /// Save pending task to email task status waiting.
        /// </summary>
        /// <param name="model">The owner task and all task oending.</param>
        private void SaveToEmailTask(Dictionary<string, List<int>> model)
        {
            var vendorList = _unitOfWork.GetRepository<Vendor>().GetCache();
            var purchaseOrgs = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var valueHelps = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            var evaTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeSummaryTask)
                                                                          .FirstOrDefault();

            foreach (var item in model)
            {
                string rows = this.GenerateContentData(item.Value, evaTemplate, vendorList, purchaseOrgs, valueHelps);
                var emailTask = this.InitialSummaryTaskModelContent(item.Key, rows, emailTemplate);
                //Save email task status waiting.
                _emailTask.Save(emailTask);
            }

        }

        /// <summary>
        /// Generate content display information task.
        /// </summary>
        /// <param name="taskDataId">The task id.</param>
        /// <param name="evaTemplate">The evaluation template.</param>
        /// <param name="vendorList">The vendor collection data.</param>
        /// <param name="purchaseOrgs">The purchase org collection data.</param>
        /// <param name="valueHelps">The valueHelp weighting key collection.</param>
        /// <returns></returns>
        private string GenerateContentData(List<int> taskDataId, IEnumerable<EvaluationTemplate> evaTemplate,
                                         IEnumerable<Vendor> vendorList, IEnumerable<PurchaseOrg> purchaseOrgs,
                                         IEnumerable<ValueHelp> valueHelps)
        {
            var taskList = _unitOfWork.GetRepository<Evaluation>().Get(x => taskDataId.Contains(x.Id));
            string rows = string.Empty;
            foreach (var item in taskList)
            {
                string gradeName = this.GetGrade(evaTemplate.FirstOrDefault(x => x.Id == item.EvaluationTemplateId).GradeId.Value, item.TotalScore.Value);
                rows += UtilityService.GenerateBodyHtmlTable(new string[]
                {
                    item.DocNo,
                    vendorList.FirstOrDefault(x=>x.VendorNo == item.VendorNo)?.VendorName,
                    purchaseOrgs.FirstOrDefault(x=>x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    valueHelps.FirstOrDefault(x=>x.ValueKey == item.WeightingKey)?.ValueText,
                    item.TotalScore.Value.ToString(),
                    gradeName
                });
            }
            return rows;
        }

        /// <summary>
        /// Initial email task model summary task email.
        /// </summary>
        /// <param name="ownerTask">The owner task sending.</param>
        /// <param name="bodyContent">The body content data.</param>
        /// <param name="emailTemplate">The email template summary task.</param>
        /// <returns></returns>
        private EmailTaskViewModel InitialSummaryTaskModelContent(string ownerTask, string bodyContent, EmailTemplate emailTemplate)
        {
            string headerTable = UtilityService.GenerateHeaderHtmlTable("ใบประเมินผู้ขาย", new string[] {
                                                                                           "เลขที่ใบประเมิน",
                                                                                           "ผู้ขาย",
                                                                                           "ผู้ซื้อ",
                                                                                           "ประเภทผู้ขาย",
                                                                                           "สรุปผลคะแนน",
                                                                                           "สรุปผลเกรด"
                                                                                       });

            string table = UtilityService.GenerateTable(headerTable, bodyContent);
            string content = emailTemplate.Content;
            content = content.Replace("%TABLE%", table);
            return new EmailTaskViewModel
            {
                Content = content,
                DocNo = "-",
                TaskCode = ConstantValue.EmailSummaryTaskCode,
                TaskBy = ConstantValue.EmailTaskByBackground,
                Subject = emailTemplate.Subject,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusWaiting,
                Receivers = this.GetReceivers(ownerTask)
            };
        }

        /// <summary>
        /// Get Grade from total score.
        /// </summary>
        /// <param name="gradeId">The grade identity.</param>
        /// <param name="totalScore">The total score evaluation.</param>
        /// <returns></returns>
        private string GetGrade(int gradeId, int totalScore)
        {
            var gradeInfo = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId);
            var gradePoint = gradeInfo.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore);
            return gradePoint.GradeNameTh;
        }

        /// <summary>
        /// Get receivers information.
        /// </summary>
        /// <param name="adUser">The employee identity.</param>
        /// <returns></returns>
        private List<EmailTaskReceiveViewModel> GetReceivers(string adUser)
        {
            var result = new List<EmailTaskReceiveViewModel>();
            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == adUser).FirstOrDefault();
            result.Add(new EmailTaskReceiveViewModel
            {
                Email = empInfo.Email,
                FullName = string.Format(ConstantValue.EmpTemplate, empInfo?.FirstnameTh, empInfo?.LastnameTh),
                ReceiverType = ConstantValue.ReceiverTypeTo
            });
            return result;
        }

        /// <summary>
        /// Get email task collection filter by status.
        /// </summary>
        /// <param name="status">The filter status value.</param>
        /// <returns></returns>
        public IEnumerable<EmailTaskViewModel> GetEmailTaskList(string status)
        {
            var result = new List<EmailTaskViewModel>();
            var emailtask = _unitOfWork.GetRepository<EmailTask>().Get(x => x.Status == status);
            var emailTaskIds = emailtask.Select(x => x.Id).ToArray();
            var emailtaskcontent = _unitOfWork.GetRepository<EmailTaskContent>().Get(x => emailTaskIds.Contains(x.EmailTaskId));
            var emailtaskReceiver = _unitOfWork.GetRepository<EmailTaskReceiver>().Get(x => emailTaskIds.Contains(x.EmailTaskId));
            foreach (var task in emailtask)
            {
                var item = new EmailTaskViewModel
                {
                    Id = task.Id,
                    DocNo = task.DocNo,
                    Subject = task.Subject,
                    TaskBy = task.TaskBy,
                    TaskCode = task.TaskCode,
                    TaskDate = task.TaskDate
                };

                item.Content = emailtaskcontent.FirstOrDefault(x => x.EmailTaskId == task.Id).Content ?? string.Empty;
                item.Receivers = MappingEmailReceiver(emailtaskReceiver.Where(x => x.EmailTaskId == task.Id));
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Mapping entity receiver model to receiver view model.
        /// </summary>
        /// <param name="rawData">The entity email receivers collection data.</param>
        /// <returns></returns>
        private static List<EmailTaskReceiveViewModel> MappingEmailReceiver(IEnumerable<EmailTaskReceiver> rawData)
        {
            var result = new List<EmailTaskReceiveViewModel>();
            foreach (var item in rawData)
            {
                result.Add(new EmailTaskReceiveViewModel
                {
                    Email = item.Email,
                    FullName = item.FullName,
                    ReceiverType = item.ReceiverType
                });
            }
            return result;
        }

        #endregion

    }
}
