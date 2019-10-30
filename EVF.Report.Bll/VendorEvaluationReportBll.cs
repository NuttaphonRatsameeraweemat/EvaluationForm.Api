using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Report.Bll
{
    public class VendorEvaluationReportBll : IVendorEvaluationReportBll
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
        /// Initializes a new instance of the <see cref="VendorEvaluationReportBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorEvaluationReportBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <param name="periodItemId">The period item identity target.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationReportViewModel> GetList()
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x =>  x.Status == ConstantValue.WorkflowStatusApproved);
            return this.InitalViewModel(data);

        }

        /// <summary>
        /// Get evaluation list status approved only.
        /// </summary>
        /// <param name="periodItemId">The period item identity target.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationReportViewModel> GetList(int periodItemId)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.PeriodItemId == periodItemId &&
                                                                        x.Status == ConstantValue.WorkflowStatusApproved);
            return this.InitalViewModel(data);

        }

        /// <summary>
        /// Inital evaluation data to view model.
        /// </summary>
        /// <param name="data">The evaluation collection data.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationReportViewModel> InitalViewModel(IEnumerable<Data.Pocos.Evaluation> data)
        {
            var result = new List<EvaluationReportViewModel>();
            var vendors = data.Select(x => x.VendorNo).ToArray();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => vendors.Contains(x.VendorNo));
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var purOrgList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            var gradeList = _unitOfWork.GetRepository<GradeItem>().GetCache();
            var templateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();

            foreach (var item in data)
            {
                result.Add(new EvaluationReportViewModel
                {
                    Id = item.Id,
                    CompanyName = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode)?.LongText,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    PurchaseOrgName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    WeightingKey = valueHelp.FirstOrDefault(x => x.ValueKey == item.WeightingKey)?.ValueText,
                    TotalScore = item.TotalScore.Value,
                    GradeName = this.GetGradeName(item.TotalScore.Value, gradeList, templateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId))
                });
            }

            return result;
        }

        /// <summary>
        /// Get Grade from total score.
        /// </summary>
        /// <param name="totalScore">The total score evaluation.</param>
        /// <param name="gradeList">The grade item collection.</param>
        /// <param name="evaluationTemplate">The evaluation template.</param>
        /// <returns></returns>
        private string GetGradeName(int totalScore, IEnumerable<GradeItem> gradeList, EvaluationTemplate evaluationTemplate)
        {
            string result = string.Empty;
            var grades = gradeList.Where(x => x.GradeId == evaluationTemplate.GradeId);
            return grades.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore)?.GradeNameTh;
        }

        public void EvaluationExportReport(int id)
        {

        }

        private VendorEvaluationRequestModel GenerateDataReport(int id)
        {
            var result = new VendorEvaluationRequestModel();

            return result;
        }

        public void EvaluationSendEmail(int id)
        {

        }

        #endregion

    }
}
