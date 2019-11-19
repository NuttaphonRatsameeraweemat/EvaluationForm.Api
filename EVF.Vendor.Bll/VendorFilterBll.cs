using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The vendor transection manager provides vendor transection functionality.
        /// </summary>
        private readonly IVendorTransectionBll _vendorTransection;
        /// <summary>
        /// The email task provides email task functionality.
        /// </summary>
        private readonly IEmailTaskBll _emailTask;
        /// <summary>
        /// The email service provides email service functionality.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILoggerManager _logger;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorFilterBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorFilterBll(IUnitOfWork unitOfWork, ILoggerManager logger, IManageToken token, IVendorTransectionBll vendorTransection,
                               IEmailTaskBll emailTask, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _vendorTransection = vendorTransection;
            _emailTask = emailTask;
            _emailService = emailService;
            _logger = logger;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get VendorFilter list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorFilterViewModel> GetList(int periodItemId)
        {
            return this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().Get(x => x.PeriodItemId == periodItemId &&
                                                                                                        _token.PurchasingOrg.Contains(x.PurchasingOrg)));
        }

        /// <summary>
        /// Get VendorFilter list condition current user is admin can see all vendor.
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
                                                            x.WeightingKey == model.WeightingKey &&
                                                            !x.IsSending.Value)).ToList();
            }
            else result = this.InitialVendorFilterViewModel(_unitOfWork.GetRepository<VendorFilter>().Get(
                                                        x => x.PeriodItemId == model.PeriodItemId &&
                                                            x.CompanyCode == model.CompanyCode &&
                                                            x.PurchasingOrg == model.PurchasingOrg &&
                                                            x.WeightingKey == model.WeightingKey &&
                                                            x.AssignTo == _token.AdUser &&
                                                            !x.IsSending.Value)).ToList();
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
        /// Initial mapping collection entity model to collection view model.
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
            var weightingKeyList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
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
                    IsSending = item.IsSending ?? false,
                    SendingStatus = item.IsSending.HasValue && item.IsSending.Value ? valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorSending)?.ValueText :
                                                           valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorWaiting)?.ValueText,
                    PeriodItemId = item.PeriodItemId,
                    PeriodItemName = periodList.FirstOrDefault(x => x.Id == item.PeriodItemId)?.PeriodName,
                    PurchasingOrg = item.PurchasingOrg,
                    PurchasingName = purOrgList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    SendEvaluationDate = item.SendingEvaDate,
                    VendorNo = item.VendorNo,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    WeightingKey = item.WeightingKey,
                    WeightingKeyName = weightingKeyList.FirstOrDefault(x => x.ValueKey == item.WeightingKey)?.ValueText
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
            var weightingKeyList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            return new VendorFilterViewModel
            {
                Id = vendorFilter.Id,
                AssignTo = vendorFilter.AssignTo,
                AssignToName = string.Format(ConstantValue.EmpTemplate, emp?.FirstnameTh, emp?.LastnameTh),
                CompanyCode = vendorFilter.CompanyCode,
                CompanyName = company?.LongText,
                IsSending = vendorFilter.IsSending ?? false,
                SendingStatus = vendorFilter.IsSending.HasValue && vendorFilter.IsSending.Value ? valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorSending)?.ValueText :
                                                           valueHelpList.FirstOrDefault(x => x.ValueKey == ConstantValue.VendorWaiting)?.ValueText,
                PeriodItemId = vendorFilter.PeriodItemId,
                PeriodItemName = period?.PeriodName,
                PurchasingOrg = vendorFilter.PurchasingOrg,
                PurchasingName = purOrg?.PurchaseName,
                SendEvaluationDate = vendorFilter.SendingEvaDate,
                VendorNo = vendorFilter.VendorNo,
                VendorName = vendor?.VendorName,
                WeightingKey = vendorFilter.WeightingKey,
                WeightingKeyName = weightingKeyList.FirstOrDefault(x => x.ValueKey == vendorFilter.WeightingKey)?.ValueText
            };
        }

        /// <summary>
        /// Create VendorFilter collection.
        /// </summary>
        /// <param name="model">The vendor filter collection value.</param>
        /// <returns></returns>
        public ResultViewModel SaveList(VendorFilterCollectionRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var vendorFilters = new List<VendorFilter>();
                var createDate = DateTime.Now;
                foreach (var item in model.VendorFilterItems)
                {
                    vendorFilters.Add(new VendorFilter
                    {
                        AssignTo = item.AssignTo,
                        CompanyCode = model.CompanyCode,
                        IsSending = false,
                        PeriodItemId = model.PeriodItemId,
                        PurchasingOrg = model.PurchasingOrg,
                        VendorNo = item.VendorNo,
                        WeightingKey = model.WeightingKey,
                        CreateBy = _token.EmpNo,
                        CreateDate = createDate
                    });
                }
                _unitOfWork.GetRepository<VendorFilter>().AddRange(vendorFilters);
                _unitOfWork.Complete(scope);
            }
            this.SendEmail(model);
            return result;
        }

        /// <summary>
        /// Change Assign to VendorFilter.
        /// </summary>
        /// <param name="model">The Vendor filter assign to information.</param>
        /// <returns></returns>
        public ResultViewModel ChangeAssignTo(VendorFilterEditRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<VendorFilter>().GetById(model.Id);
                data.AssignTo = model.AssignTo;
                _unitOfWork.GetRepository<VendorFilter>().Update(data);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Delete VendorFilter.
        /// </summary>
        /// <param name="id">The Vendor filter identity.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<VendorFilter>().Remove(_unitOfWork.GetRepository<VendorFilter>().GetById(id));
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Update sending status and log timestamp.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <param name="comCode">The company code identity.</param>
        /// <param name="purOrg">The purchasing org identity.</param>
        /// <param name="weightingKey">The weighting key evaluation.</param>
        /// <param name="vendorNo">The vendor no identity.</param>
        public void UpdateStatus(int periodItemId, string comCode, string purOrg, string weightingKey, string vendorNo)
        {
            var data = _unitOfWork.GetRepository<VendorFilter>().Get(x => x.PeriodItemId == periodItemId &&
                                                                        x.CompanyCode == comCode &&
                                                                        x.PurchasingOrg == purOrg &&
                                                                        x.WeightingKey == weightingKey &&
                                                                        x.VendorNo == vendorNo).FirstOrDefault();
            data.IsSending = true;
            data.SendingBy = _token.EmpNo;
            data.SendingEvaDate = DateTime.Now;
            _unitOfWork.GetRepository<VendorFilter>().Update(data);
        }

        /// <summary>
        /// Filter Vendor by condition and total sales.
        /// </summary>
        /// <param name="model">The criteria vendor filter.</param>
        /// <returns></returns>
        public IEnumerable<VendorFilterResponseViewModel> SearchVendor(VendorFilterSearchViewModel model)
        {
            var result = new List<VendorFilterResponseViewModel>();
            var purGroups = _unitOfWork.GetRepository<PurGroupWeightingKey>().GetCache(x => x.WeightingKey == model.WeightingKey && x.EvaStatus.Value).Select(x => x.PurGroup).ToArray();
            var transectionList = _vendorTransection.GetTransections(model.StartDate, model.EndDate, purGroups, model.ComCode, model.PurchaseOrg);
            transectionList = this.FilterCondition(transectionList, model.WeightingKey);
            var vendorInfo = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var exitsVendors = this.GetExistVendorFilter(model);
            var vendors = transectionList.Where(x => !exitsVendors.Contains(x.Vendor))
                                         .Select(x => x.Vendor).Distinct().ToArray();
            foreach (var item in vendors)
            {
                var summary = transectionList.Where(x => x.Vendor == item).Sum(x => x.QuantityReceived);

                result.Add(new VendorFilterResponseViewModel
                {
                    VendorNo = item,
                    VendorName = vendorInfo.FirstOrDefault(x => x.VendorNo == item)?.VendorName,
                    TotalSales = summary.Value
                });
            }
            switch (model.Condition)
            {
                case ConstantValue.VendorConditionMoreThan:
                    //filter total sales morethan value in model.
                    result = result.Where(x => x.TotalSales >= model.TotalSales).ToList();
                    break;
            }
            return result.OrderByDescending(x => x.TotalSales);
        }

        /// <summary>
        /// Get exits vendor in condition.
        /// </summary>
        /// <param name="model">The criteria vendor filter.</param>
        /// <returns></returns>
        private string[] GetExistVendorFilter(VendorFilterSearchViewModel model)
        {
            return _unitOfWork.GetRepository<VendorFilter>().Get(x => x.PeriodItemId == model.PeriodItemId &&
                                                                         x.CompanyCode == model.ComCode &&
                                                                         x.PurchasingOrg == model.PurchaseOrg &&
                                                                         x.WeightingKey == model.WeightingKey).Select(x => x.VendorNo).ToArray();
        }

        /// <summary>
        /// Filter vendor transection mark weighting key.
        /// </summary>
        /// <param name="transectionList">The vendor transection list.</param>
        /// <param name="weightingKey">The weighting key.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransaction> FilterCondition(IEnumerable<VendorTransaction> transectionList, string weightingKey)
        {
            switch (weightingKey)
            {
                case "A3":
                case "A4":
                    transectionList = transectionList.Where(x => x.MarkWeightingKey == weightingKey);
                    break;
                default:
                    transectionList = transectionList.Where(x => x.MarkWeightingKey == null || x.MarkWeightingKey == weightingKey);
                    break;
            }
            return transectionList;
        }

        /// <summary>
        /// Send email to user vendor filter information.
        /// </summary>
        /// <param name="model">The vendor filter collection.</param>
        private void SendEmail(VendorFilterCollectionRequestViewModel model)
        {
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var assignList = model.VendorFilterItems.Select(x => x.AssignTo).Distinct();

            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache(x => assignList.Contains(x.Aduser));
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == model.PeriodItemId).FirstOrDefault();
            var companyInfo = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == model.CompanyCode).FirstOrDefault();
            string weightingKey = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey &&
                                                                                    x.ValueKey == model.WeightingKey).FirstOrDefault()?.ValueText;
            var purchaseInfo = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == model.PurchasingOrg).FirstOrDefault();
            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeVendorFilterNotice).FirstOrDefault();

            foreach (var item in assignList)
            {
                var vendorFilters = model.VendorFilterItems.Where(x => x.AssignTo == item);
                var empInfo = empList.FirstOrDefault(x => x.Aduser == item);
                string rows = this.GenerateContentData(vendorList, vendorFilters);

                string subject = emailTemplate.Subject;
                string content = emailTemplate.Content;

                subject = subject.Replace("%PERIODNAME%", periodItem.PeriodName);

                content = content.Replace("%EMPNAME%", string.Format(ConstantValue.EmpTemplate, empInfo?.FirstnameTh, empInfo?.LastnameTh));
                content = content.Replace("%PERIODITEMNAME%", periodItem.PeriodName);
                content = content.Replace("%COMNAME%", companyInfo?.LongText);
                content = content.Replace("%PURCHASENAME%", purchaseInfo?.PurchaseName);
                content = content.Replace("%WEIGHTINGKEY%", weightingKey);
                content = content.Replace("%TABLE%", this.GenerateTable(rows));

                string status = ConstantValue.EmailTaskStatusSending;
                try
                {
                    _emailService.SendEmail(new EmailModel
                    {
                        Body = content,
                        Receiver = empInfo.Email,
                        Subject = subject,
                    });
                }
                catch (Exception ex)
                {
                    status = ConstantValue.EmailTaskStatusError;
                    _logger.LogError(ex, "Vendor filter is sending email error :");
                }

                _emailTask.Save(new Email.Bll.Models.EmailTaskViewModel
                {
                    DocNo = "-",
                    Content = $"{content}",
                    Subject = subject,
                    TaskCode = ConstantValue.EmailVendorFilterNoticeCode,
                    Receivers = new List<Email.Bll.Models.EmailTaskReceiveViewModel>
                {
                    new Email.Bll.Models.EmailTaskReceiveViewModel
                    {
                        Email = empInfo.Email,
                        FullName = string.Format(ConstantValue.EmpTemplate,empInfo?.FirstnameTh,empInfo?.LastnameTh),
                        ReceiverType = ConstantValue.ReceiverTypeTo
                    }
                },
                    TaskBy = _token.AdUser,
                    TaskDate = DateTime.Now,
                    Status = status
                });

            }
        }

        /// <summary>
        /// Generate table display data.
        /// </summary>
        /// <param name="bodyContent">The body content for generate table.</param>
        /// <returns></returns>
        private string GenerateTable(string bodyContent)
        {
            string headers = UtilityService.GenerateHeaderHtmlTable("ข้อมูลผู้ขาย", new string[] { "รหัสผู้ขาย", "ชื่อผู้ขาย" });
            return UtilityService.GenerateTable(headers, bodyContent);
        }

        /// <summary>
        /// Generate content data rows table.
        /// </summary>
        /// <param name="vendorList">The vendor information collection.</param>
        /// <param name="vendorFilters">The vendor filter collection.</param>
        /// <returns></returns>
        private string GenerateContentData(IEnumerable<Data.Pocos.Vendor> vendorList,
                                           IEnumerable<VendorFilterItemRequestViewModel> vendorFilters)
        {
            string rows = string.Empty;
            foreach (var item in vendorFilters)
            {
                var vendorInfo = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo);
                rows += UtilityService.GenerateBodyHtmlTable(new string[]
                {
                    item.VendorNo,
                    vendorInfo?.VendorName,
                });
            }
            return rows;
        }

        #endregion

    }
}
