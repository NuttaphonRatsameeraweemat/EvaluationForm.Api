using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Inbox.Bll.Interfaces;
using EVF.Inbox.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Inbox.Bll
{
    public class TaskBll : ITaskBll
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
        /// Initializes a new instance of the <see cref="TaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="k2Service">The k2 service provides k2 service functionality.</param>
        public TaskBll(IUnitOfWork unitOfWork, IK2Service k2Service, IManageToken token, IConfigSetting config, IMapper mapper)
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
        /// Get Task pending list from k2.
        /// </summary>
        /// <returns></returns>
        public TaskViewModel GetTaskList(string fromUser)
        {
            var result = new TaskViewModel();
            var taskList = _k2Service.GetWorkList(fromUser);
            foreach (var item in taskList)
            {
                result.TaskList.Add(new TaskListModel
                {
                    SerialNumber = item.SerialNumber,
                    ProcessCode = UtilityService.GetValueDictionaryToString(item.DataFields, ConstantValue.DataFieldsKeyProcessCode),
                    Step = UtilityService.GetValueDictionaryToInt(item.DataFields, ConstantValue.DataFieldsKeyCurrentStep),
                    DataId = UtilityService.GetValueDictionaryToInt(item.DataFields, ConstantValue.DataFieldsKeyDataID),
                    ReceiveDate = UtilityService.GetValueDictionaryToDateTime(item.DataFields, ConstantValue.DataFieldsKeyReceivedDate)
                });
            }
            this.GetInformationTask(result);
            return result;
        }

        /// <summary>
        /// Get display information task.
        /// </summary>
        /// <param name="result">The task result.</param>
        private void GetInformationTask(TaskViewModel result)
        {
            if (result.TaskList.Count > 0)
            {
                var evaIds = result.TaskList.Where(x => x.ProcessCode == ConstantValue.EvaluationProcessCode).Select(x => x.DataId).ToArray();
                var evaInfoList = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaIds.Contains(x.Id));
                var templateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
                var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
                var purOrgList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
                var gradeItemList = _unitOfWork.GetRepository<GradeItem>().GetCache();
                var processCode = _unitOfWork.GetRepository<WorkflowProcess>().GetCache();
                var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache();

                foreach (var item in result.TaskList)
                {
                    var temp = evaInfoList.FirstOrDefault(x => x.Id == item.DataId);
                    var template = templateList.FirstOrDefault(x => x.Id == temp.EvaluationTemplateId);
                    string[] gradeInfo = this.GetGrade(template.GradeId.Value, temp.TotalScore.Value);
                    item.DocNo = temp.DocNo;
                    item.TotalScore = temp.TotalScore.Value;
                    item.VendorName = vendorList.FirstOrDefault(x => x.VendorNo == temp.VendorNo)?.VendorName;
                    item.PurchaseOrgName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == temp.PurchasingOrg)?.PurchaseName;
                    item.GradeName = gradeInfo[1];
                    item.GradeNameEn = gradeInfo[2];
                    item.ProcessName = processCode.FirstOrDefault(x => x.ProcessCode == item.ProcessCode)?.ProcessName;
                    item.GradeId = template.GradeId.Value;
                    item.GradeItemId = Convert.ToInt32(gradeInfo[0]);
                    item.EvaluationTemplateId = template.Id;
                    item.WeightingKeyName = valueHelpList.FirstOrDefault(x => x.ValueKey == temp.WeightingKey)?.ValueText;
                }

                var gradeGroup = result.TaskList.Select(x => x.GradeId).Distinct();

                var gradeList = _unitOfWork.GetRepository<GradeItem>().GetCache(x => gradeGroup.Contains(x.GradeId.Value),
                                                                                x => x.OrderByDescending(y => y.StartPoint));

                foreach (var item in gradeList)
                {
                    var grade = result.TaskList.FirstOrDefault(x => x.GradeItemId == item.Id);
                    if (grade != null)
                    {
                        result.TaskOverView.Add(new TaskOverViewModel
                        {
                            GradeTh = grade.GradeName,
                            GradeEn = grade.GradeNameEn,
                            GradeItemId = item.Id,
                            Count = result.TaskList.Where(x => x.GradeItemId == item.Id).Count(),
                            Color = this.GetColorCondition(item.Id, item.GradeId.Value, gradeList)
                        });
                    }
                }

            }
        }

        private string GetColorCondition(int gradeItemId, int gradeId, IEnumerable<GradeItem> gradeItems)
        {
            string result = string.Empty;
            int color = 0;
            var gradeList = gradeItems.Where(x => x.GradeId == gradeId).OrderByDescending(x => x.StartPoint);
            int countGrade = gradeList.Count();
            foreach (var item in gradeList)
            {
                if (item.Id == gradeItemId)
                {
                    result = this.GetColor(color, countGrade - 1);
                    break;
                }
                color++;
            }
            return result;
        }

        private string GetColor(int colorNumber, int maxLength)
        {
            string result = string.Empty;
            if (colorNumber == 0)
            {
                //Execellent color
                result = "#2ECC71";
            }
            else if (colorNumber == maxLength)
            {
                //Bad color
                result = "#E74C3C";
            }
            else
            {
                //Nomal color
                result = "#FFFF00";
            }
            return result;
        }

        /// <summary>
        /// Get Grade from total score.
        /// </summary>
        /// <param name="gradeId">The grade identity.</param>
        /// <param name="totalScore">The total score evaluation.</param>
        /// <returns></returns>
        private string[] GetGrade(int gradeId, int totalScore)
        {
            var gradeInfo = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId);
            var gradePoint = gradeInfo.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore);
            return new string[] { gradePoint.Id.ToString(), gradePoint.GradeNameTh, gradePoint.GradeNameEn };
        }

        #endregion

    }
}
