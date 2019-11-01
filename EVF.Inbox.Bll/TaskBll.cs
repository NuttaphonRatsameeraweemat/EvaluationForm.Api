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
        public IEnumerable<TaskViewModel> GetTaskList(string fromUser)
        {
            var result = new List<TaskViewModel>();
            var taskList = _k2Service.GetWorkList(fromUser);
            foreach (var item in taskList)
            {
                result.Add(new TaskViewModel
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
        private void GetInformationTask(IEnumerable<TaskViewModel> result)
        {
            var evaIds = result.Where(x => x.ProcessCode == ConstantValue.EvaluationProcessCode).Select(x => x.DataId).ToArray();
            var evaInfoList = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaIds.Contains(x.Id));
            var templateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var purOrgList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var gradeItemList = _unitOfWork.GetRepository<GradeItem>().GetCache();
            var processCode = _unitOfWork.GetRepository<WorkflowProcess>().GetCache();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache();

            foreach (var item in result)
            {
                switch (item.ProcessCode)
                {
                    case ConstantValue.EvaluationProcessCode:
                        var temp = evaInfoList.FirstOrDefault(x => x.Id == item.DataId);
                        var template = templateList.FirstOrDefault(x => x.Id == temp.EvaluationTemplateId);
                        item.DocNo = temp.DocNo;
                        item.TotalScore = temp.TotalScore.Value;
                        item.VendorName = vendorList.FirstOrDefault(x => x.VendorNo == temp.VendorNo)?.VendorName;
                        item.PurchaseOrgName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == temp.PurchasingOrg)?.PurchaseName;
                        item.GradeName = this.GetGrade(template.GradeId.Value, temp.TotalScore.Value);
                        item.ProcessName = processCode.FirstOrDefault(x => x.ProcessCode == item.ProcessCode)?.ProcessName;
                        item.GradeId = template.GradeId.Value;
                        item.EvaluationTemplateId = template.Id;
                        item.WeightingKeyName = valueHelpList.FirstOrDefault(x => x.ValueKey == temp.WeightingKey)?.ValueText;
                        break;
                }
            }
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

        #endregion

    }
}
