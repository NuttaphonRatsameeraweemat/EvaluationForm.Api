using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Vendor.Bll
{
    public class VendorBll : IVendorBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Vendor list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Data.Pocos.Vendor>, IEnumerable<VendorViewModel>>(
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache());
        }

        /// <summary>
        /// Get Detail of Vendor.
        /// </summary>
        /// <param name="vendorNo">The identity Vendor.</param>
        /// <returns></returns>
        public VendorViewModel GetDetail(string vendorNo)
        {
            var result = _mapper.Map<Data.Pocos.Vendor, VendorViewModel>(
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == vendorNo).FirstOrDefault());
            return result;
        }

        /// <summary>
        /// Get pie chart stat vendor evaluation.
        /// </summary>
        /// <param name="vendorNo">The vendor identity.</param>
        /// <returns></returns>
        public IEnumerable<VendorPieChart> GetPieChart(string vendorNo)
        {
            var result = new List<VendorPieChart>();
            var templateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var gradeItemList = _unitOfWork.GetRepository<GradeItem>().GetCache();
            var evaluations = _unitOfWork.GetRepository<Evaluation>().Get(x => x.VendorNo == vendorNo && x.Status == ConstantValue.WorkflowStatusApproved);
            foreach (var item in evaluations)
            {
                var tempTemplate = templateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId);
                var gradeItems = gradeItemList.Where(x => x.GradeId == tempTemplate.GradeId);
                var grade = gradeItems.FirstOrDefault(x => x.StartPoint <= item.TotalScore && x.EndPoint >= item.TotalScore);
                var temp = result.FirstOrDefault(x => x.GradeName == grade.GradeNameTh);
                if (temp != null)
                {
                    temp.Count = temp.Count + 1;
                }
                else result.Add(new VendorPieChart { GradeName = grade.GradeNameTh, Count = 1 });
            }
            return result;
        }

        /// <summary>
        /// Get Line chart vendor evaluation stat.
        /// </summary>
        /// <param name="vendorNo">The vendor identity.</param>
        /// <returns></returns>
        public VendorLineChart GetLineChart(string vendorNo)
        {
            var result = new VendorLineChart();
            var statData = new List<VendorLineChartData>();
            var labels = new List<string>();
            var periods = _unitOfWork.GetRepository<Period>().GetCache(orderBy: x => x.OrderBy(y => y.Year)).Take(5);
            var periodIds = periods.Select(x => x.Id).ToArray();
            var periodItems = _unitOfWork.GetRepository<PeriodItem>().GetCache(x=> periodIds.Contains(x.PeriodId.Value));
            var evaluations = _unitOfWork.GetRepository<Evaluation>().Get(x => x.VendorNo == vendorNo && x.Status == ConstantValue.WorkflowStatusApproved);
            foreach (var item in periodItems)
            {
                labels.Add(item.PeriodName);
                var temp = evaluations.FirstOrDefault(x => x.PeriodItemId == item.Id);
                if (temp != null)
                {
                    statData.Add(new VendorLineChartData { PeriodName = item.PeriodName, TotalScore = temp.TotalScore.Value });
                }
            }
            result.DataStats.AddRange(statData);
            result.AllPeriods = labels.ToArray();
            return result;
        }

        /// <summary>
        /// Get vendor evaluation history by period id.
        /// </summary>
        /// <param name="vendorNo">The vendor identity.</param>
        /// <param name="periodId">The period identity.</param>
        /// <returns></returns>
        public IEnumerable<VendorEvaluationHistoryViewModel> GetVendorEvaluationHistory(string vendorNo, int periodId)
        {
            var result = new List<VendorEvaluationHistoryViewModel>();
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == periodId);
            var periodItemIds = periodItem.Select(x => x.Id).ToArray();
            var history = _unitOfWork.GetRepository<Evaluation>().Get(x => x.VendorNo == vendorNo &&
                                                                           periodItemIds.Contains(x.PeriodItemId.Value) &&
                                                                           x.Status == ConstantValue.WorkflowStatusApproved);
            var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var gradeItemList = _unitOfWork.GetRepository<GradeItem>().GetCache();

            foreach (var item in history)
            {
                var template = evaluationTemplate.FirstOrDefault(x => x.Id == item.EvaluationTemplateId);
                result.Add(new VendorEvaluationHistoryViewModel
                {
                    PeriodName = periodItem.FirstOrDefault(x => x.Id == item.PeriodItemId)?.PeriodName,
                    WeightingKey = item.WeightingKey,
                    WeightingKeyName = string.Empty,
                    TotalScore = item.TotalScore.Value,
                    GradeName = this.GetGradeName(gradeItemList.Where(x => x.GradeId == template.GradeId), item.TotalScore.Value)
                });
            }

            return result;
        }

        /// <summary>
        /// Get GradeName.
        /// </summary>
        /// <param name="gradeItems">The list grade item for evaluation.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        private string GetGradeName(IEnumerable<GradeItem> gradeItems, int score)
        {
            return gradeItems.FirstOrDefault(x => x.StartPoint <= score && x.EndPoint >= score)?.GradeNameTh;
        }

        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel UpdateVendorContact(VendorRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == model.VendorNo).FirstOrDefault();
                data.Email = model.Email;
                data.TelNo = model.TelNo;
                data.TelExt = model.TelExt;
                data.MobileNo = model.MobileNo;
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().Update(data);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheVendor();
            return result;
        }

        /// <summary>
        /// Reload Cache when Vendor is change.
        /// </summary>
        private void ReloadCacheVendor()
        {
            _unitOfWork.GetRepository<Data.Pocos.Vendor>().ReCache();
        }

        #endregion

    }
}
