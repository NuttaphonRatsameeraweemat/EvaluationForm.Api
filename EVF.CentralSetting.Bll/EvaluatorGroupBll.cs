using AutoMapper;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.CentralSetting.Bll
{
    public class EvaluatorGroupBll : IEvaluatorGroupBll
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
        /// Initializes a new instance of the <see cref="EvaluatorGroupBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluatorGroupBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get EvaluatorGroup list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluatorGroupViewModel> GetList()
        {
            var data = _mapper.Map<IEnumerable<EvaluatorGroup>, IEnumerable<EvaluatorGroupViewModel>>(
                _unitOfWork.GetRepository<EvaluatorGroup>().GetCache());
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            foreach (var item in data)
            {
                var temp = periodItem.FirstOrDefault(x => x.Id == item.PeriodItemId);
                item.PeriodItemName = temp?.PeriodName;
            }
            return data;
        }

        /// <summary>
        /// Get EvaluatorGroup list by period item id.
        /// </summary>
        /// <param name="periodItems">The identity period item.</param>
        /// <returns></returns>
        public IEnumerable<EvaluatorGroupViewModel> GetEvaluatorGroups(int periodItems)
        {
            var data = _mapper.Map<IEnumerable<EvaluatorGroup>, IEnumerable<EvaluatorGroupViewModel>>(
                _unitOfWork.GetRepository<EvaluatorGroup>().GetCache(x=>x.PeriodItemId == periodItems));
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            foreach (var item in data)
            {
                var temp = periodItem.FirstOrDefault(x => x.Id == item.PeriodItemId);
                item.PeriodItemName = temp?.PeriodName;
            }
            return data;
        }

        /// <summary>
        /// Get Detail of EvaluatorGroup.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        public EvaluatorGroupViewModel GetDetail(int id)
        {
            var result = _mapper.Map<EvaluatorGroup, EvaluatorGroupViewModel>(_unitOfWork.GetRepository<EvaluatorGroup>().GetCache(x => x.Id == id).FirstOrDefault());
            result.AdUserList = _unitOfWork.GetRepository<EvaluatorGroupItem>().GetCache(x => x.EvaluatorGroupId == result.Id).Select(x => x.AdUser).ToArray();
            return result;
        }

        /// <summary>
        /// Insert new EvaluatorGroup list.
        /// </summary>
        /// <param name="model">The EvaluatorGroup information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(EvaluatorGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluatorGroup = _mapper.Map<EvaluatorGroupViewModel, EvaluatorGroup>(model);
                evaluatorGroup.CreateBy = _token.EmpNo;
                evaluatorGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<EvaluatorGroup>().Add(evaluatorGroup);
                _unitOfWork.Complete();
                this.SaveItem(evaluatorGroup.Id, model.AdUserList);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluatorGroup();
            return result;
        }

        /// <summary>
        /// Insert new EvaluatorGroup item.
        /// </summary>
        /// <param name="evaluatorGroupId">The EvaluatorGroup id.</param>
        /// <param name="adUsers">The employee aduser list.</param>
        private void SaveItem(int evaluatorGroupId, string[] adUsers)
        {
            var evaluatorGroupItems = new List<EvaluatorGroupItem>();
            foreach (var item in adUsers)
            {
                evaluatorGroupItems.Add(new EvaluatorGroupItem { EvaluatorGroupId = evaluatorGroupId, AdUser = item });
            }
            _unitOfWork.GetRepository<EvaluatorGroupItem>().AddRange(evaluatorGroupItems);
        }

        /// <summary>
        /// Update EvaluatorGroup.
        /// </summary>
        /// <param name="model">The EvaluatorGroup information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(EvaluatorGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<EvaluatorGroup>().GetCache(x => x.Id == model.Id).FirstOrDefault();
                data.EvaluatorGroupName = model.EvaluatorGroupName;
                data.LastModifyBy = _token.EmpNo;
                data.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<EvaluatorGroup>().Update(data);
                this.EditItem(data.Id, model.AdUserList);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluatorGroup();
            return result;
        }

        /// <summary>
        /// Edit EvaluatorGroup items.
        /// </summary>
        /// <param name="evaluatorGroupId">The identity EvaluatorGroup.</param>
        /// <param name="adUsers">The employee aduser list.</param>
        private void EditItem(int evaluatorGroupId, string[] adUsers)
        {
            this.SaveItem(evaluatorGroupId, adUsers);
            this.DeleteItem(_unitOfWork.GetRepository<EvaluatorGroupItem>().GetCache(x => x.EvaluatorGroupId == evaluatorGroupId));
        }

        /// <summary>
        /// Remove EvaluatorGroup.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluatorGroup = _unitOfWork.GetRepository<EvaluatorGroup>().GetCache(x => x.Id == id);
                this.DeleteItem(_unitOfWork.GetRepository<EvaluatorGroupItem>().GetCache(x => x.EvaluatorGroupId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluatorGroup();
            return result;
        }

        /// <summary>
        /// Remove EvaluatorGroup item list.
        /// </summary>
        /// <param name="evaluatorGroupItems">The EvaluatorGroup items.</param>
        private void DeleteItem(IEnumerable<EvaluatorGroupItem> evaluatorGroupItems)
        {
            _unitOfWork.GetRepository<EvaluatorGroupItem>().RemoveRange(evaluatorGroupItems);
        }

        /// <summary>
        /// Reload Cache when EvaluatorGroup is change.
        /// </summary>
        private void ReloadCacheEvaluatorGroup()
        {
            _unitOfWork.GetRepository<EvaluatorGroup>().ReCache();
            _unitOfWork.GetRepository<EvaluatorGroupItem>().ReCache();
        }

        #endregion

    }
}
