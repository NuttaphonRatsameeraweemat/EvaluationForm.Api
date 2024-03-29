﻿using AutoMapper;
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
    public class ApprovalBll : IApprovalBll
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
        /// Initializes a new instance of the <see cref="ApprovalBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public ApprovalBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Approval list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApprovalViewModel> GetList()
        {
            return this.InitialList(_unitOfWork.GetRepository<Approval>().GetCache());
        }

        /// <summary>
        /// Initial Approval ViewModel.
        /// </summary>
        /// <param name="data">The approval data.</param>
        /// <returns></returns>
        private IEnumerable<ApprovalViewModel> InitialList(IEnumerable<Approval> data)
        {
            var purList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();

            var result = new List<ApprovalViewModel>();
            foreach (var item in data)
            {
                var tempPur = purList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg);
                result.Add(new ApprovalViewModel
                {
                    Id = item.Id,
                    PurchasingOrg = item.PurchasingOrg,
                    PurchasingOrgName = tempPur?.PurchaseName
                });
            }
            return result;
        }

        /// <summary>
        /// Get Detail of approval.
        /// </summary>
        /// <param name="id">The identity approval.</param>
        /// <returns></returns>
        public ApprovalViewModel GetDetail(int id)
        {
            var result = _mapper.Map<Approval, ApprovalViewModel>(_unitOfWork.GetRepository<Approval>().GetCache(x => x.Id == id).FirstOrDefault());
            result.ApprovalList.AddRange(_mapper.Map<IEnumerable<ApprovalItem>, IEnumerable<ApprovalItemViewModel>>(
                _unitOfWork.GetRepository<ApprovalItem>().GetCache(x => x.ApprovalId == id, y => y.OrderBy(x => x.Step))));
            return result;
        }

        /// <summary>
        /// Insert new approval list.
        /// </summary>
        /// <param name="model">The approval information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(ApprovalViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var approval = _mapper.Map<ApprovalViewModel, Approval>(model);
                approval.CreateBy = _token.EmpNo;
                approval.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Approval>().Add(approval);
                _unitOfWork.Complete();
                this.SaveItem(approval.Id, _mapper.Map<IEnumerable<ApprovalItemViewModel>, IEnumerable<ApprovalItem>>(model.ApprovalList));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheApproval();
            return result;
        }

        /// <summary>
        /// Insert new approval item.
        /// </summary>
        /// <param name="approvalId">The approval id.</param>
        /// <param name="approvalItems">The approval items.</param>
        private void SaveItem(int approvalId, IEnumerable<ApprovalItem> approvalItems)
        {
            approvalItems.Select(c => { c.ApprovalId = approvalId; return c; }).ToList();
            _unitOfWork.GetRepository<ApprovalItem>().AddRange(approvalItems);
        }

        /// <summary>
        /// Update approval.
        /// </summary>
        /// <param name="model">The approval information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(ApprovalViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Approval>().GetCache(x => x.Id == model.Id).FirstOrDefault();
                data.PurchasingOrg = model.PurchasingOrg;
                data.LastModifyBy = _token.EmpNo;
                data.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Approval>().Update(data);
                this.EditItem(data.Id, model.ApprovalList);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheApproval();
            return result;
        }

        /// <summary>
        /// Edit approval items.
        /// </summary>
        /// <param name="approvalId">The identity approval.</param>
        /// <param name="approvalList">The approval items.</param>
        private void EditItem(int approvalId, IEnumerable<ApprovalItemViewModel> approvalList)
        {
            var data = _unitOfWork.GetRepository<ApprovalItem>().GetCache(x => x.ApprovalId == approvalId);

            var addItem = approvalList.Where(x => x.Id == 0);
            var tempUpdate = approvalList.Where(x => data.Any(y => x.Id == y.Id));
            var removeItem = data.Where(x => !approvalList.Any(y => x.Id == y.Id));

            var updateItem = _mapper.Map<IEnumerable<ApprovalItemViewModel>, IEnumerable<ApprovalItem>>(tempUpdate);
            updateItem.Select(c => { c.ApprovalId = approvalId; return c; }).ToList();
            _unitOfWork.GetRepository<ApprovalItem>().UpdateRange(updateItem);

            this.SaveItem(approvalId, _mapper.Map<IEnumerable<ApprovalItemViewModel>, IEnumerable<ApprovalItem>>(addItem));
            this.DeleteItem(removeItem);
        }

        /// <summary>
        /// Remove approval.
        /// </summary>
        /// <param name="id">The identity approval.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var approval = _unitOfWork.GetRepository<Approval>().GetCache(x => x.Id == id);
                this.DeleteItem(_unitOfWork.GetRepository<ApprovalItem>().GetCache(x => x.ApprovalId == id));
                _unitOfWork.GetRepository<Approval>().RemoveRange(approval);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheApproval();
            return result;
        }

        /// <summary>
        /// Remove approval item list.
        /// </summary>
        /// <param name="approvalItems">The approval items.</param>
        private void DeleteItem(IEnumerable<ApprovalItem> approvalItems)
        {
            _unitOfWork.GetRepository<ApprovalItem>().RemoveRange(approvalItems);
        }

        /// <summary>
        /// Reload Cache when Approval is change.
        /// </summary>
        private void ReloadCacheApproval()
        {
            _unitOfWork.GetRepository<Approval>().ReCache();
            _unitOfWork.GetRepository<ApprovalItem>().ReCache();
        }

        #endregion

    }
}
