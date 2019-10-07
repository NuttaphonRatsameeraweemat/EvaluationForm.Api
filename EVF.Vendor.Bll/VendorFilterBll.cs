﻿using AutoMapper;
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
    public class VendorFilterBll : IVendorFilterBll
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
        /// Initializes a new instance of the <see cref="VendorFilterBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorFilterBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get VendorFilter list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorFilterViewModel> GetList(int periodItemId)
        {
            return this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().Get(x => x.PeriodItemId == periodItemId));
        }

        /// <summary>
        /// Get VendorFilter list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorFilterViewModel> GetVendorFilters(VendorFilterCriteriaViewModel model)
        {
            var result = new List<VendorFilterViewModel>();
            if (this.IsPurchaseAdmin(model.PurchasingOrg))
            {
                //check admin if true get all vendor in purOrg false get only equal assign to
                result = this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().Get(
                                                        x => x.PeriodItemId == model.PeriodItemId &&
                                                            x.CompanyCode == model.CompanyCode &&
                                                            x.PurchasingOrg == model.PurchasingOrg &&
                                                            x.WeightingKey == model.WeightingKey)).ToList();
            }
            else result = this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().Get(
                                                        x => x.PeriodItemId == model.PeriodItemId &&
                                                            x.CompanyCode == model.CompanyCode &&
                                                            x.PurchasingOrg == model.PurchasingOrg &&
                                                            x.WeightingKey == model.WeightingKey &&
                                                            x.AssignTo == _token.AdUser)).ToList();
            return result;
        }

        /// <summary>
        /// Check current user purchasing org user type admin or not.
        /// </summary>
        /// <param name="purOrg">The purchase org.</param>
        /// <returns></returns>
        private bool IsPurchaseAdmin(string purOrg)
        {
            var data = _unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.PuchaseOrg == purOrg &&
                                                                                  x.AdUser == _token.AdUser).FirstOrDefault();
            return data.Type == ConstantValue.PurchasingTypeAdmin;
        }

        /// <summary>
        /// Initial mapping entity model to view model.
        /// </summary>
        /// <param name="vendorFilters">The vendor filter entity model data.</param>
        /// <returns></returns>
        private IEnumerable<VendorFilterViewModel> InitialVendorFilterViewModel(IEnumerable<VendorFilter> vendorFilters)
        {
            var result = new List<VendorFilterViewModel>();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var periodList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            var purOrgList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeVendorSendingStatus);
            foreach (var item in vendorFilters)
            {
                var temp = empList.FirstOrDefault(x => x.Aduser == item.AssignTo);
                result.Add(new VendorFilterViewModel
                {
                    Id = item.Id,
                    AssignTo = item.AssignTo,
                    AssignToName = string.Format(ConstantValue.EmpTemplate, temp?.FirstnameTh, temp?.LastnameTh),
                    CompanyCode = item.CompanyCode,
                    CompanyName = comList.FirstOrDefault(x => x.SapcomCode == item.CompanyCode)?.LongText,
                    IsSending = item.IsSending.Value,
                    SendingStatus = item.IsSending.Value ? valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorSending)?.ValueText :
                                                           valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorWaiting)?.ValueText,
                    PeriodItemId = item.PeriodItemId,
                    PeriodItemName = periodList.FirstOrDefault(x => x.Id == item.PeriodItemId)?.PeriodName,
                    PurchasingOrg = item.PurchasingOrg,
                    PurchasingName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    SendEvaluationDate = item.SendingEvaDate,
                    VendorNo = item.VendorNo,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    WeightingKey = item.WeightingKey
                });
            }
            return result;
        }

        /// <summary>
        /// Get Detail of Vendor.
        /// </summary>
        /// <param name="vendorNo">The identity Vendor.</param>
        /// <returns></returns>
        public VendorFilterViewModel GetDetail(int id)
        {
            return this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().GetById(id));
        }

        /// <summary>
        /// Initial mapping entity model to view model.
        /// </summary>
        /// <param name="vendorFilter">The vendor filter entity model data.</param>
        /// <returns></returns>
        private VendorFilterViewModel InitialVendorFilterViewModel(VendorFilter vendorFilter)
        {
            var result = new VendorFilterViewModel();
            var emp = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == vendorFilter.AssignTo).FirstOrDefault();
            var company = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == vendorFilter.CompanyCode).FirstOrDefault();
            var period = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == vendorFilter.PeriodItemId).FirstOrDefault();
            var purOrg = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == vendorFilter.PurchasingOrg).FirstOrDefault();
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == vendorFilter.VendorNo).FirstOrDefault();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeVendorSendingStatus);
            return new VendorFilterViewModel
            {
                Id = vendorFilter.Id,
                AssignTo = vendorFilter.AssignTo,
                AssignToName = string.Format(ConstantValue.EmpTemplate, emp?.FirstnameTh, emp?.LastnameTh),
                CompanyCode = vendorFilter.CompanyCode,
                CompanyName = company?.LongText,
                IsSending = vendorFilter.IsSending.Value,
                SendingStatus = vendorFilter.IsSending.Value ? valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorSending)?.ValueText :
                                                           valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorWaiting)?.ValueText,
                PeriodItemId = vendorFilter.PeriodItemId,
                PeriodItemName = period?.PeriodName,
                PurchasingOrg = vendorFilter.PurchasingOrg,
                PurchasingName = purOrg?.PurchaseName,
                SendEvaluationDate = vendorFilter.SendingEvaDate,
                VendorNo = vendorFilter.VendorNo,
                VendorName = vendor?.VendorName,
                WeightingKey = vendorFilter.WeightingKey
            };
        }

        /// <summary>
        /// Create VendorFilter.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(VendorFilterRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _mapper.Map<VendorFilterRequestViewModel, VendorFilter>(model);
                data.CreateBy = _token.EmpNo;
                data.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<VendorFilter>().Add(data);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Update sending status and log timestamp.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel UpdateStatus(VendorFilter model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                model.IsSending = true;
                model.SendingEvaDate = DateTime.Now;
                _unitOfWork.GetRepository<VendorFilter>().Update(model);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        #endregion

    }
}
