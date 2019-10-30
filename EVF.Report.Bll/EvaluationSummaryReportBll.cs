using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EVF.Report.Bll
{
    public class EvaluationSummaryReportBll : IEvaluationSummaryReportBll
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationSummaryReportBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationSummaryReportBll(IUnitOfWork unitOfWork, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _token = token;
        }

        #endregion

        #region [Methods]

        public void ExportSummaryReport(EvaluationSummaryReportRequestModel model)
        {

        }

        private void GetData(EvaluationSummaryReportRequestModel model)
        {
            var whereClause = this.BuildDynamicWhereClause(model);
        }

        private Expression<Func<Evaluation, bool>> BuildDynamicWhereClause(EvaluationSummaryReportRequestModel model)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.True<Evaluation>(); // true -where(true) return all
            if (!string.IsNullOrEmpty(model.ComCode))
            {
                predicate = predicate.And(s => s.ComCode == model.ComCode);
            }
            if (!string.IsNullOrEmpty(model.PurchaseOrg))
            {
                predicate = predicate.And(s => s.PurchasingOrg == model.PurchaseOrg);
            }
            if (!string.IsNullOrEmpty(model.WeightingKey))
            {
                predicate = predicate.And(s => s.WeightingKey == model.WeightingKey);
            }

            if (model.PeriodItemId.HasValue)
            {
                predicate = predicate.And(s => s.PeriodItemId == model.PeriodItemId);
            }
            else if (model.PeriodId.HasValue)
            {
                var periodItemIds = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == model.PeriodId).Select(x => x.Id).ToArray();
                predicate = predicate.And(s => periodItemIds.Contains(s.PeriodItemId.Value));
            }
            
            return predicate;
        }

        #endregion

    }
}
