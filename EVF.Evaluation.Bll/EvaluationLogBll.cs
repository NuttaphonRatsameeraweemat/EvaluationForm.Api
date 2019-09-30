using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Evaluation.Bll
{
    public class EvaluationLogBll : IEvaluationLogBll
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
        /// Initializes a new instance of the <see cref="EvaluationLogBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationLogBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation Log.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationLogViewModel> GetEvaluationLog(int evaluationId)
        {
            return this.InitialEvaluationLogViewModel(_unitOfWork.GetRepository<EvaluationLog>().Get(x => x.EvaluationId == evaluationId &&
                                                                                                          x.AdUser == _token.AdUser));
        }

        /// <summary>
        /// Initial Evaluation Viewmodel.
        /// </summary>
        /// <param name="data">The evaluation log entity model.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationLogViewModel> InitialEvaluationLogViewModel(IEnumerable<EvaluationLog> data)
        {
            var result = new List<EvaluationLogViewModel>();
            foreach (var item in data)
            {
                var logItems = _mapper.Map<IEnumerable<EvaluationLogItem>, IEnumerable<EvaluationLogItemViewModel>>(
                    _unitOfWork.GetRepository<EvaluationLogItem>().Get(x => x.EvaluationLogId == item.Id)).ToList();

                result.Add(new EvaluationLogViewModel
                {
                    ActionDate = item.ActionDate,
                    EvaluationLogs = logItems
                });
            }
            return result;
        }

        /// <summary>
        /// Insert new evaluation log.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        public ResultViewModel Save(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluationLog = new EvaluationLog
                {
                    EvaluationId = evaluationId,
                    ActionDate = DateTime.Now,
                    EmpNo = _token.EmpNo,
                    AdUser = _token.AdUser
                };
                _unitOfWork.GetRepository<EvaluationLog>().Add(evaluationLog);
                _unitOfWork.Complete();
                this.SaveItem(evaluationLog.Id, model);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Save evaluation log item.
        /// </summary>
        /// <param name="evaluationLogId">The evaluation log identity.</param>
        /// <param name="model">The evaluation log item information value.</param>
        private void SaveItem(int evaluationLogId, IEnumerable<EvaluationLogItemViewModel> model)
        {
            var evaluationLogItems = _mapper.Map<IEnumerable<EvaluationLogItemViewModel>, IEnumerable<EvaluationLogItem>>(model);
            evaluationLogItems.Select(c => { c.EvaluationLogId = evaluationLogId; return c; }).ToList();
            _unitOfWork.GetRepository<EvaluationLogItem>().AddRange(evaluationLogItems);
        }

        #endregion

    }
}
