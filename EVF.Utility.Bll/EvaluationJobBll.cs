using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Components;
using EVF.Helper.Models;
using EVF.Utility.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EVF.Utility.Bll
{
    public class EvaluationJobBll : IEvaluationJobBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationJobBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public EvaluationJobBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Execute evaluation process end of evaluation period.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel ExecuteEvaluationProcess()
        {
            var result = new ResultViewModel();
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new System.TimeSpan(0, 30, 0)))
            {
                var data = _unitOfWork.GetRepository<Evaluation>().Get(x => x.Status == ConstantValue.EvaWaiting);
                var periodItemIds = data.Select(x => x.PeriodItemId.Value).Distinct().ToArray();

                var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => periodItemIds.Contains(x.Id));
                List<int> periodExpireIds = new List<int>();
                foreach (var item in periodItem)
                {
                    if (DateTime.Now.Date > item.EndEvaDate.Value.Date)
                    {
                        periodExpireIds.Add(item.Id);
                    }
                }

                data = data.Where(x => periodExpireIds.Contains(x.PeriodItemId.Value));
                data.Select(c => { c.Status = ConstantValue.EvaExpire; return c; }).ToList();
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        #endregion

    }
}
