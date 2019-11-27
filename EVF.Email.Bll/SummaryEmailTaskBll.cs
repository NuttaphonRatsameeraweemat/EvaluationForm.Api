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
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEmailTaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEmailTaskBll(IUnitOfWork unitOfWork, IManageToken token, IEmailTaskBll emailTask, 
                                   IEmailService emailService, ILoggerManager logger, IConfigSetting config)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _emailTask = emailTask;
            _emailService = emailService;
            _logger = logger;
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Execute email status waiting.
        /// </summary>
        public void ExecuteEmailTaskWaiting(string status)
        {
            List<int> emailSuccess = new List<int>();
            List<int> emailFailed = new List<int>();
            this.SendingEmailTask(this.GetEmailTaskList(status), emailSuccess, emailFailed);
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

        /// <summary>
        /// Process summary task evaluation reject.
        /// </summary>
        public void ProcessSummaryTaskReject()
        {
            var result = this.GetEvaluationRejectTask();
            this.SaveEvaRejectToEmailTask(result);
        }

        /// <summary>
        /// Process summary evaluation task waiting.
        /// </summary>
        public void ProcessSummaryTaskEvaWaiting()
        {
            var result = this.GetEvaluationWaitingTask();
            this.SaveEvaWaitingToEmailTask(result);
        }

        /// <summary>
        /// Get evaluation reject task.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<SummaryEvaRejectModel>> GetEvaluationRejectTask()
        {
            var result = new Dictionary<string, List<SummaryEvaRejectModel>>();
            var model = _unitOfWork.GetRepository<Evaluation>().Get(x => x.Status == ConstantValue.WorkflowStatusReject);
            string[] vendors = model.Select(x => x.VendorNo).ToArray();
            var vendorList = _unitOfWork.GetRepository<Vendor>().GetCache(x => vendors.Contains(x.VendorNo));
            var periodItemList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var evaTemplateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();

            foreach (var item in model)
            {
                var evaTemplate = evaTemplateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId.Value);
                var vendorInfo = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo);
                var periodItemInfo = periodItemList.FirstOrDefault(x => x.Id == item.PeriodItemId);
                var company = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode);
                var valueHelp = valueHelpList.FirstOrDefault(x => x.ValueKey == item.WeightingKey);

                if (result.Any(x => x.Key == item.CreateBy))
                {
                    result[item.CreateBy].Add(
                        this.InitialRejectTask(item.DocNo, company.LongText, vendorInfo.VendorName, valueHelp.ValueText, item.TotalScore.Value,
                                               this.GetGrade(evaTemplate.GradeId.Value, item.TotalScore.Value), periodItemInfo.PeriodName));
                }
                else
                {
                    result.Add(item.CreateBy, new List<SummaryEvaRejectModel>
                        {
                            this.InitialRejectTask(item.DocNo,company.LongText,vendorInfo.VendorName,valueHelp.ValueText,item.TotalScore.Value,
                                                   this.GetGrade(evaTemplate.GradeId.Value, item.TotalScore.Value),periodItemInfo.PeriodName)
                        });
                }
            }
            return result;
        }

        /// <summary>
        /// Inital Summary evaluation reject email task model.
        /// </summary>
        /// <param name="docNo">The docuemnt evaluation number.</param>
        /// <param name="companyName">The company name.</param>
        /// <param name="vendorName">The vendor name.</param>
        /// <param name="weightingKey">The weighting name.</param>
        /// <param name="startEvaDate">The evaluation start date.</param>
        /// <param name="endEvaDate">The evaluation end date.</param>
        /// <returns></returns>
        private SummaryEvaRejectModel InitialRejectTask(string docNo, string companyName, string vendorName, string weightingKey,
                                                        int totalScore, string gradeName, string periodItemName)
        {
            return new SummaryEvaRejectModel
            {
                DocNo = docNo,
                CompanyName = companyName,
                VendorName = vendorName,
                WeightingKeyName = weightingKey,
                TotalScore = totalScore,
                GradeName = gradeName,
                PeriodItemName = periodItemName
            };
        }

        /// <summary>
        /// Save evaluation reject task to email task.
        /// </summary>
        /// <param name="model">The evaluation reject task collection.</param>
        private void SaveEvaRejectToEmailTask(Dictionary<string, List<SummaryEvaRejectModel>> model)
        {
            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeSummaryTaskReject).FirstOrDefault();
            foreach (var item in model)
            {
                string rows = this.GenerateContentData(item.Value);
                var emailTask = this.InitialSummaryEvaReject(item.Key, rows, emailTemplate);
                //Save email task status waiting.
                _emailTask.Save(emailTask);
            }

        }

        /// <summary>
        /// Generate content summary evaluation task.
        /// </summary>
        /// <param name="model">The content value.</param>
        /// <returns></returns>
        private string GenerateContentData(List<SummaryEvaRejectModel> model)
        {
            string rows = string.Empty;
            foreach (var item in model)
            {
                rows += UtilityService.GenerateBodyHtmlTable(new string[]
                {
                    item.DocNo,
                    item.VendorName,
                    item.CompanyName,
                    item.WeightingKeyName,
                    item.PeriodItemName,
                    item.TotalScore.ToString(),
                    item.GradeName
                });
            }
            return rows;
        }

        /// <summary>
        /// Initial email task model summary evaluation waiting task email.
        /// </summary>
        /// <param name="ownerTask">The owner task sending.</param>
        /// <param name="bodyContent">The body content data.</param>
        /// <param name="emailTemplate">The email template summary evaluation waiting task.</param>
        /// <returns></returns>
        private EmailTaskViewModel InitialSummaryEvaReject(string ownerTask, string bodyContent, EmailTemplate emailTemplate)
        {
            string headerTable = UtilityService.GenerateHeaderHtmlTable("ใบประเมินผู้ขาย", new string[] {
                                                                                           "เลขที่ใบประเมิน",
                                                                                           "ผู้ขาย",
                                                                                           "ผู้ซื้อ",
                                                                                           "ประเภทผู้ขาย",
                                                                                           "ชื่อรอบการประเมิน",
                                                                                           "สรุปผลคะแนน",
                                                                                           "สรุปผลเกรด"
                                                                                       });

            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == ownerTask).FirstOrDefault();
            string table = UtilityService.GenerateTable(headerTable, bodyContent);
            string content = emailTemplate.Content;
            content = content.Replace("%TO%", string.Format(ConstantValue.EmpTemplate, empInfo.FirstnameTh, empInfo.LastnameTh));
            content = content.Replace("%TABLE%", table);
            content = content.Replace("%URL%", _config.TaskUrl + "Evaluation_Group/Evaluation_MGT_Group/SummaryEvaluation");
            return new EmailTaskViewModel
            {
                Content = content,
                DocNo = "-",
                TaskCode = ConstantValue.EmailSummaryTaskRejectCode,
                TaskBy = ConstantValue.EmailTaskByBackground,
                Subject = emailTemplate.Subject,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusWaiting,
                Receivers = this.GetReceiversByEmpNo(ownerTask)
            };
        }

        /// <summary>
        /// Get evaluation waiting task.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<SummaryEvaWaitingModel>> GetEvaluationWaitingTask()
        {
            var result = new Dictionary<string, List<SummaryEvaWaitingModel>>();
            var model = _unitOfWork.GetRepository<Evaluation>().Get(x => x.Status == ConstantValue.EvaWaiting);
            string[] vendors = model.Select(x => x.VendorNo).ToArray();
            var vendorList = _unitOfWork.GetRepository<Vendor>().GetCache(x => vendors.Contains(x.VendorNo));
            var periodItemList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();

            foreach (var item in model)
            {
                var evaAssignList = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == item.Id && !x.IsAction.Value);
                foreach (var subItem in evaAssignList)
                {
                    var vendorInfo = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo);
                    var periodItemInfo = periodItemList.FirstOrDefault(x => x.Id == item.PeriodItemId);
                    var company = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode);
                    var valueHelp = valueHelpList.FirstOrDefault(x => x.ValueKey == item.WeightingKey);

                    if (result.Any(x => x.Key == subItem.AdUser))
                    {
                        result[subItem.AdUser].Add(
                            this.InitialWaitingTask(item.DocNo, company.LongText, vendorInfo.VendorName, valueHelp.ValueText,
                                                    UtilityService.DateTimeToString(periodItemInfo.StartEvaDate.Value, ConstantValue.DateTimeFormat),
                                                    UtilityService.DateTimeToString(periodItemInfo.EndEvaDate.Value, ConstantValue.DateTimeFormat)));
                    }
                    else
                    {
                        result.Add(subItem.AdUser, new List<SummaryEvaWaitingModel>
                        {
                            this.InitialWaitingTask(item.DocNo,company.LongText,vendorInfo.VendorName,valueHelp.ValueText,
                                                    UtilityService.DateTimeToString(periodItemInfo.StartEvaDate.Value,ConstantValue.DateTimeFormat),
                                                    UtilityService.DateTimeToString(periodItemInfo.EndEvaDate.Value,ConstantValue.DateTimeFormat))
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Inital Summary evaluation waiting email task model.
        /// </summary>
        /// <param name="docNo">The docuemnt evaluation number.</param>
        /// <param name="companyName">The company name.</param>
        /// <param name="vendorName">The vendor name.</param>
        /// <param name="weightingKey">The weighting name.</param>
        /// <param name="startEvaDate">The evaluation start date.</param>
        /// <param name="endEvaDate">The evaluation end date.</param>
        /// <returns></returns>
        private SummaryEvaWaitingModel InitialWaitingTask(string docNo, string companyName, string vendorName, string weightingKey,
                                                          string startEvaDate, string endEvaDate)
        {
            return new SummaryEvaWaitingModel
            {
                DocNo = docNo,
                CompanyName = companyName,
                VendorName = vendorName,
                WeightingKeyName = weightingKey,
                StartEvaDate = startEvaDate,
                EndEvaDate = endEvaDate
            };
        }


        /// <summary>
        /// Save evaluation waiting task to email task.
        /// </summary>
        /// <param name="model">The summary evaluation waiting task collection.</param>
        private void SaveEvaWaitingToEmailTask(Dictionary<string, List<SummaryEvaWaitingModel>> model)
        {
            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeSummaryTaskEvaWaiting).FirstOrDefault();
            foreach (var item in model)
            {
                string rows = this.GenerateContentData(item.Value);
                var emailTask = this.InitialSummaryEvaWaiting(item.Key, rows, emailTemplate);
                //Save email task status waiting.
                _emailTask.Save(emailTask);
            }

        }

        /// <summary>
        /// Generate content summary evaluation task.
        /// </summary>
        /// <param name="model">The content value.</param>
        /// <returns></returns>
        private string GenerateContentData(List<SummaryEvaWaitingModel> model)
        {
            string rows = string.Empty;
            foreach (var item in model)
            {
                rows += UtilityService.GenerateBodyHtmlTable(new string[]
                {
                    item.DocNo,
                    item.VendorName,
                    item.CompanyName,
                    item.WeightingKeyName,
                    item.StartEvaDate,
                    item.EndEvaDate
                });
            }
            return rows;
        }

        /// <summary>
        /// Initial email task model summary evaluation waiting task email.
        /// </summary>
        /// <param name="ownerTask">The owner task sending.</param>
        /// <param name="bodyContent">The body content data.</param>
        /// <param name="emailTemplate">The email template summary evaluation waiting task.</param>
        /// <returns></returns>
        private EmailTaskViewModel InitialSummaryEvaWaiting(string ownerTask, string bodyContent, EmailTemplate emailTemplate)
        {
            string headerTable = UtilityService.GenerateHeaderHtmlTable("ใบประเมินผู้ขาย", new string[] {
                                                                                           "เลขที่ใบประเมิน",
                                                                                           "ผู้ขาย",
                                                                                           "ผู้ซื้อ",
                                                                                           "ประเภทผู้ขาย",
                                                                                           "วันที่เริ่มต้นการประเมิน",
                                                                                           "วันที่สิ้นสุดการประเมิน"
                                                                                       });

            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == ownerTask).FirstOrDefault();
            string table = UtilityService.GenerateTable(headerTable, bodyContent);
            string content = emailTemplate.Content;
            content = content.Replace("%TO%", string.Format(ConstantValue.EmpTemplate, empInfo.FirstnameTh, empInfo.LastnameTh));
            content = content.Replace("%TABLE%", table);
            content = content.Replace("%URL%", _config.TaskUrl + "Evaluation_Group/Evaluation_MGT_Group/Evaluation");
            return new EmailTaskViewModel
            {
                Content = content,
                DocNo = "-",
                TaskCode = ConstantValue.EmailSummaryTaskEvaWaitingCode,
                TaskBy = ConstantValue.EmailTaskByBackground,
                Subject = emailTemplate.Subject,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusWaiting,
                Receivers = this.GetReceivers(ownerTask)
            };
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
            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == ownerTask).FirstOrDefault();
            string table = UtilityService.GenerateTable(headerTable, bodyContent);
            string content = emailTemplate.Content;
            content = content.Replace("%TO%", string.Format(ConstantValue.EmpTemplate, empInfo.FirstnameTh, empInfo.LastnameTh));
            content = content.Replace("%TABLE%", table);
            content = content.Replace("%URL%", _config.TaskUrl + "Inbox");
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
        /// Get receivers information.
        /// </summary>
        /// <param name="adUser">The employee identity.</param>
        /// <returns></returns>
        private List<EmailTaskReceiveViewModel> GetReceiversByEmpNo(string empNo)
        {
            var result = new List<EmailTaskReceiveViewModel>();
            var empInfo = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == empNo).FirstOrDefault();
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
