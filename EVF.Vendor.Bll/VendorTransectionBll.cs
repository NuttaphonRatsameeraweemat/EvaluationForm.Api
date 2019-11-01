using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Vendor.Bll
{
    public class VendorTransectionBll : IVendorTransectionBll
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
        /// <summary>
        /// The elastic search service provider elastic search functionality.
        /// </summary>
        private readonly IElasticSearch<VendorTransectionElasticSearchModel> _elasticSearch;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorTransectionBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        /// <param name="elasticSearch">The elastic search service provider elastic search functionality.</param>
        public VendorTransectionBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token,
                                    IElasticSearch<VendorTransectionElasticSearchModel> elasticSearch)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _elasticSearch = elasticSearch;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionViewModel> GetList()
        {
            return this.InitialVendorName(_mapper.Map<IEnumerable<VendorTransaction>, IEnumerable<VendorTransectionViewModel>>(
                _unitOfWork.GetRepository<VendorTransaction>().Get()));
        }

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionViewModel> GetListSearch(VendorTransectionSearchViewModel model)
        {
            return this.InitialVendorName(this.SearchTransection(model));
        }

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionElasticSearchModel> GetListSearchElastic(VendorTransectionSearchViewModel model)
        {
            return _elasticSearch.SearchFilter(this.GetQueryFilter(model));
        }

        /// <summary>
        /// Search by criteria model search transection.
        /// </summary>
        /// <param name="model">The criteria search vendor transection model.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransectionViewModel> SearchTransection(VendorTransectionSearchViewModel model)
        {
            return _mapper.Map<IEnumerable<VendorTransaction>, IEnumerable<VendorTransectionViewModel>>(
                    this.GetTransections(model.StartDate, model.EndDate, new string[] { model.PurGroup }));
        }

        /// <summary>
        /// Initial vendor transection view model.
        /// </summary>
        /// <param name="result">The vendor transection infomation.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransectionViewModel> InitialVendorName(IEnumerable<VendorTransectionViewModel> result)
        {
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            foreach (var item in result)
            {
                item.VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.Vendor)?.VendorName;
            }
            return result;
        }

        /// <summary>
        /// Get Vendor transection.
        /// </summary>
        /// <param name="startDateString">The start transection date.</param>
        /// <param name="endDateString">The end transection date.</param>
        /// <param name="purGroup">The purGroup code.</param>
        /// <returns></returns>
        public IEnumerable<VendorTransaction> GetTransections(string startDateString, string endDateString, string[] purGroup)
        {
            var startDate = UtilityService.ConvertToDateTime(startDateString, ConstantValue.DateTimeFormat);
            var endDate = UtilityService.ConvertToDateTime(endDateString, ConstantValue.DateTimeFormat);
            return _unitOfWork.GetRepository<VendorTransaction>().Get(x => purGroup.Contains(x.PurgropCode) &&
                                                                           _token.PurchasingOrg.Contains(x.PurorgCode) &&
                                                                                   x.ReceiptDate.Value.Date >= startDate.Date &&
                                                                                   x.ReceiptDate.Value.Date <= endDate.Date);
        }

        /// <summary>
        /// Get Transection list by condition.
        /// </summary>
        /// <param name="startDateString">The start transection date.</param>
        /// <param name="endDateString">The end transection date.</param>
        /// <param name="purGroup">The purGroup code.</param>
        /// <param name="comCode">The company code.</param>
        /// <param name="purOrg">The purchase org.</param>
        /// <returns></returns>
        public IEnumerable<VendorTransaction> GetTransections(string startDateString, string endDateString, string[] purGroup, string comCode, string purOrg)
        {
            var startDate = UtilityService.ConvertToDateTime(startDateString, ConstantValue.DateTimeFormat);
            var endDate = UtilityService.ConvertToDateTime(endDateString, ConstantValue.DateTimeFormat);
            return _unitOfWork.GetRepository<VendorTransaction>().Get(x => purGroup.Contains(x.PurgropCode) &&
                                                                                   x.CompanyCode == comCode &&
                                                                                   x.PurorgCode == purOrg &&
                                                                                   x.ReceiptDate.Value.Date >= startDate.Date &&
                                                                                   x.ReceiptDate.Value.Date <= endDate.Date);
            //return _elasticSearch.SearchFilter(this.GetQueryFilter(startDate, endDate, purGroup, comCode, purOrg));
        }

        /// <summary>
        /// Get Detail of VendorTransection.
        /// </summary>
        /// <param name="id">The identity VendorTranection.</param>
        /// <returns></returns>
        public VendorTransectionViewModel GetDetail(int id)
        {
            var result = _mapper.Map<VendorTransaction, VendorTransectionViewModel>(
                _unitOfWork.GetRepository<VendorTransaction>().GetById(id));
            result.VendorName = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == result.Vendor).FirstOrDefault()?.VendorName;
            return result;
        }

        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel MarkWeightingKey(VendorTransectionRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<VendorTransaction>().GetById(model.Id);
                data.MarkWeightingKey = model.WeightingKey;
                _unitOfWork.GetRepository<VendorTransaction>().Update(data);
                _unitOfWork.Complete(scope);
            }
            //this.ReImportTransactionById(model.Id);
            return result;
        }

        /// <summary>
        /// Get query filter search descriptor.
        /// </summary>
        /// <param name="search">The search value.</param>
        /// <returns></returns>
        private Func<SearchDescriptor<VendorTransectionElasticSearchModel>, ISearchRequest> GetQueryFilter(DateTime startDate, DateTime endDate,
                                                                                                           string[] purGroup, string comCode, string purOrg)
        {
            ISearchRequest searchFunc(SearchDescriptor<VendorTransectionElasticSearchModel> s) => s
                                                                       .Index(ConstantValue.VendorTransactionIndex)
                                                                       .From(0)
                                                                       .Take(1000)
                                                                       .Query(q =>
                                                                         q.Bool(b =>
                                                                             //b.Must(m =>
                                                                             //    m.Match(mm =>
                                                                             //        mm.Field(f => purGroup.Contains(f.PurgropCode))
                                                                             //          .Field(f => _token.PurchasingOrg.Contains(f.PurorgCode))
                                                                             //    ))
                                                                             b.Filter(ff =>
                                                                                  //ff.DateRange(t => t.Field(f => f.ReceiptDate).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate)) 
                                                                                  ff.Terms(t => t.Field(f => f.PurgropCode).Terms(purGroup))
                                                                                  //ff.Term(t => t.Field(f => f.CompanyCode).Value(comCode)) &&
                                                                                  //ff.Term(t => t.Field(f => f.PurgropCode).Value(purOrg))
                                                                             )
                                                                           )
                                                                             //Filter
                                                                             //q.DateRange(t => t.Field(f => f.ReceiptDate).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate)) &&
                                                                             //q.Terms(t => t.Field(f => f.PurorgCode).Terms(purOrg)) &&
                                                                             //q.Terms(t => t.Field(f => f.CompanyCode).Terms(comCode)) &&
                                                                             //q.Terms(t => t.Field(f => f.PurgropCode).Terms(purGroup))
                                                                             )
                                                                       .Sort(m => m.Descending(f => f.Id));
            return searchFunc;
        }

        /// <summary>
        /// Get query filter search descriptor.
        /// </summary>
        /// <param name="search">The search value.</param>
        /// <returns></returns>
        private Func<SearchDescriptor<VendorTransectionElasticSearchModel>, ISearchRequest> GetQueryFilter(VendorTransectionSearchViewModel search)
        {
            var startDate = UtilityService.ConvertToDateTime(search.StartDate, ConstantValue.DateTimeFormat);
            var endDate = UtilityService.ConvertToDateTime(search.EndDate, ConstantValue.DateTimeFormat);
            ISearchRequest searchFunc(SearchDescriptor<VendorTransectionElasticSearchModel> s) => s
                                                                       .Index(ConstantValue.VendorTransactionIndex)
                                                                       //.From(0)
                                                                       //.Take(1000)
                                                                       .Query(q =>
                                                                                //q.Bool(b =>
                                                                                //    //b.Must(m =>
                                                                                //    //    m.Match(mm =>
                                                                                //    //        mm.Field(f => search.PurGroup.Contains(f.PurgropCode))
                                                                                //    //          .Field(f => _token.PurchasingOrg.Contains(f.PurorgCode))
                                                                                //    //    ))
                                                                                //    //b.Filter(ff =>
                                                                                //    //   // ff.DateRange(t => t.Field(f => f.ReceiptDate).GreaterThanOrEquals(startDate).LessThanOrEquals(endDate)) &&
                                                                                //    //    ff.Term(t => t.Field(f => f.PurgropCode).Value(search.PurGroup))
                                                                                //    //)
                                                                                //  )
                                                                                q.Match(m => m.Field(f => search.PurGroup.Contains(f.PurgropCode)))
                                                                              )
                                                                       .Sort(m => m.Descending(f => f.Id));
            return searchFunc;
        }

        /// <summary>
        /// Bulk vendor transaction status waiting to elastic.
        /// </summary>
        /// <returns></returns>
        public string BulkVendorTransaction()
        {
            string response = string.Empty;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 30, 0)))
            {
                var transactionList = _unitOfWork.GetRepository<VendorTransaction>().Get(x => x.ElasticStatus == ConstantValue.ElasticStatusWaiting);
                this.UpdateStatus(transactionList);
                var result = _mapper.Map<IEnumerable<VendorTransaction>, IEnumerable<VendorTransectionElasticSearchModel>>(transactionList);
                this.InitialVendorName(result);
                response = _elasticSearch.Bulk(result.ToList(), ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);
                _unitOfWork.Complete(scope);
            }
            return response;
        }

        /// <summary>
        /// Bulk vendor transaction status update to elastic.
        /// </summary>
        /// <returns></returns>
        public string BulkUpdateVendorTransaction()
        {
            string response = string.Empty;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 30, 0)))
            {
                var transactionList = _unitOfWork.GetRepository<VendorTransaction>().Get(x => x.ElasticStatus == ConstantValue.ElasticStatusUpdate);
                this.UpdateStatus(transactionList);
                var result = _mapper.Map<IEnumerable<VendorTransaction>, IEnumerable<VendorTransectionElasticSearchModel>>(transactionList);
                this.InitialVendorName(result);
                foreach (var item in result)
                {
                    response = response + _elasticSearch.Update(item, ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);
                }
                _unitOfWork.Complete(scope);
            }
            return response;
        }

        /// <summary>
        /// Update send to elastic status.
        /// </summary>
        /// <param name="vendorTransactions">The vendor transaction collection.</param>
        private void UpdateStatus(IEnumerable<VendorTransaction> vendorTransactions)
        {
            vendorTransactions.Select(c => { c.ElasticStatus = ConstantValue.ElasticStatusAdded; return c; }).ToList();
            _unitOfWork.GetRepository<VendorTransaction>().UpdateRange(vendorTransactions);
        }

        /// <summary>
        /// Re import to elastic by transaction id.
        /// </summary>
        /// <param name="id">The transaction identity.</param>
        /// <returns></returns>
        public string ReImportTransactionById(int id)
        {
            var transaction = _unitOfWork.GetRepository<VendorTransaction>().GetById(id);
            var result = _mapper.Map<VendorTransaction, VendorTransectionElasticSearchModel>(transaction);
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == transaction.Vendor).FirstOrDefault();
            result.VendorName = vendor.VendorName;

            _elasticSearch.Delete(id, ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);

            return _elasticSearch.Insert(result, ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);
        }

        /// <summary>
        /// Re import all vendor transaction to elastic.
        /// </summary>
        /// <returns></returns>
        public string ReImportTransaction()
        {
            _elasticSearch.DeleteAll(ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);
            var transactionList = _unitOfWork.GetRepository<VendorTransaction>().Get();
            var result = _mapper.Map<IEnumerable<VendorTransaction>, IEnumerable<VendorTransectionElasticSearchModel>>(transactionList);
            this.InitialVendorName(result);
            return _elasticSearch.Bulk(result.ToList(), ConstantValue.VendorTransactionIndex, ConstantValue.VendorTransactionType);
        }

        /// <summary>
        /// Initial vendor name.
        /// </summary>
        /// <param name="result">The transaction collection value.</param>
        private void InitialVendorName(IEnumerable<VendorTransectionElasticSearchModel> result)
        {
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var groupVendor = result.Select(x => x.Vendor).Distinct();

            foreach (var item in groupVendor)
            {
                var temp = vendorList.FirstOrDefault(x => x.VendorNo == item);
                if (temp != null)
                {
                    result.Where(x => x.Vendor == item).Select(c => { c.VendorName = temp.VendorName; return c; }).ToList();
                }
            }
        }

        #endregion

    }
}
