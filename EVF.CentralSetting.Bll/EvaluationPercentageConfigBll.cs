using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EVF.CentralSetting.Bll
{
    public class EvaluationPercentageConfigBll : IEvaluationPercentageConfigBll
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
        /// Initializes a new instance of the <see cref="EvaluatorGroupBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationPercentageConfigBll(IUnitOfWork unitOfWork, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation Percentage Config list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationPercentageConfigViewModel> GetList()
        {
            var result = new List<EvaluationPercentageConfigViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache();
            foreach (var item in data)
            {
                result.Add(new EvaluationPercentageConfigViewModel
                {
                    Id = item.Id,
                    UserPercentage = item.UserPercentage,
                    PurchasePercentage = item.PurchasePercentage,
                    StartDate = UtilityService.DateTimeToString(item.StartDate.Value,ConstantValue.DateTimeFormat),
                    EndDate = item.EndDate.HasValue ? UtilityService.DateTimeToString(item.EndDate.Value, ConstantValue.DateTimeFormat) : string.Empty
                });
            }
            return result;
        }

        /// <summary>
        /// Get Detail of Evaluation Percentage Config.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        public EvaluationPercentageConfigViewModel GetDetail(int id)
        {
            var data = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.Id == id).FirstOrDefault();
            return new EvaluationPercentageConfigViewModel
            {
                Id = data.Id,
                UserPercentage = data.UserPercentage,
                PurchasePercentage = data.PurchasePercentage,
                StartDate = UtilityService.DateTimeToString(data.StartDate.Value, ConstantValue.DateTimeFormat),
                EndDate = data.EndDate.HasValue ? UtilityService.DateTimeToString(data.EndDate.Value, ConstantValue.DateTimeFormat) : string.Empty
            };
        }

        /// <summary>
        /// Insert new Evaluation Percentage Config.
        /// </summary>
        /// <param name="model">The Evaluation Percentage Config information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(EvaluationPercentageConfigRequestModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaConfig = new EvaluationPercentageConfig
                {
                    UserPercentage = model.UserPercentage,
                    PurchasePercentage = model.PurchasePercentage,
                    StartDate = DateTime.Now.Date,
                    CreateBy = _token.EmpNo,
                    CreateDate = DateTime.Now
                };
                var configs = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache();
                configs.Select(c => { c.EndDate = DateTime.Now.Date.AddDays(-1); return c; }).ToList();
                _unitOfWork.GetRepository<EvaluationPercentageConfig>().Add(evaConfig);
                _unitOfWork.GetRepository<EvaluationPercentageConfig>().UpdateRange(configs);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationPercentageConfig();
            return result;
        }

        /// <summary>
        /// Update Evaluation Percentage Config.
        /// </summary>
        /// <param name="model">The Evaluation Percentage Config information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(EvaluationPercentageConfigRequestModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaConfig = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.Id == model.Id).FirstOrDefault();
                evaConfig.PurchasePercentage = model.PurchasePercentage;
                evaConfig.UserPercentage = model.UserPercentage;
                _unitOfWork.GetRepository<EvaluationPercentageConfig>().Update(evaConfig);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationPercentageConfig();
            return result;
        }

        /// <summary>
        /// Remove Evaluation Percentage Config.
        /// </summary>
        /// <param name="id">The identity Evaluation Percentage Config.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaConfig = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.Id == id).FirstOrDefault();
                _unitOfWork.GetRepository<EvaluationPercentageConfig>().Remove(evaConfig);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationPercentageConfig();
            return result;
        }

        /// <summary>
        /// Reload Cache when Evaluation Percentage Config is change.
        /// </summary>
        private void ReloadCacheEvaluationPercentageConfig()
        {
            _unitOfWork.GetRepository<EvaluationPercentageConfig>().ReCache();
        }

        #endregion

    }
}
