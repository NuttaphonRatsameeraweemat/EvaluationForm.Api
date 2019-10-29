using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
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
            if (!string.IsNullOrEmpty(model.ComCode))
            {

            }
        }

        #endregion

    }
}
