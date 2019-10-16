using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Utility.Bll.Interfaces;
using System;

namespace EVF.Utility.Bll
{
    public class CacheBll : ICacheBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public CacheBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region [Methods]
        
        /// <summary>
        /// Re Inital Cache.
        /// </summary>
        /// <returns></returns>
        public string ReInitialCache()
        {
            var startTime = DateTime.Now;
            //Start recache.
            this.ReCahceHr();
            this.ReCacheVendor();
            this.ReCacheMaster();
            this.ReCacheCentralSetting();
            this.ReCacheAuthorization();
            //End recache.
            var endTime = DateTime.Now;
            var diffTime = endTime - startTime;
            return string.Format("Initial Time: {0} seconds, At {1} - {2}", 
                diffTime.Seconds.ToString(), startTime.ToString("dd/MM/yyyy HH:mm:ss"), 
                                             endTime.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        /// <summary>
        /// Re all cache table hr.
        /// </summary>
        private void ReCahceHr()
        {
            _unitOfWork.GetRepository<Hremployee>().ReCache();
            _unitOfWork.GetRepository<Hrcompany>().ReCache();
            _unitOfWork.GetRepository<Hrorg>().ReCache();
            _unitOfWork.GetRepository<HrorgRelation>().ReCache();
            _unitOfWork.GetRepository<Hrposition>().ReCache();
        }

        /// <summary>
        /// Re all cache table vendor.
        /// </summary>
        private void ReCacheVendor()
        {
            _unitOfWork.GetRepository<Vendor>().ReCache();
        }

        /// <summary>
        /// Re all cahce table setup master evaluation.
        /// </summary>
        private void ReCacheMaster()
        {
            _unitOfWork.GetRepository<Kpi>().ReCache();
            _unitOfWork.GetRepository<KpiGroup>().ReCache();
            _unitOfWork.GetRepository<KpiGroupItem>().ReCache();
            _unitOfWork.GetRepository<Criteria>().ReCache();
            _unitOfWork.GetRepository<CriteriaGroup>().ReCache();
            _unitOfWork.GetRepository<CriteriaItem>().ReCache();
            _unitOfWork.GetRepository<EvaluationTemplate>().ReCache();
            _unitOfWork.GetRepository<Grade>().ReCache();
            _unitOfWork.GetRepository<GradeItem>().ReCache();
            _unitOfWork.GetRepository<LevelPoint>().ReCache();
            _unitOfWork.GetRepository<LevelPointItem>().ReCache();
            _unitOfWork.GetRepository<Period>().ReCache();
            _unitOfWork.GetRepository<PeriodItem>().ReCache();
            _unitOfWork.GetRepository<SapFields>().ReCache();
        }

        /// <summary>
        /// Re all cache table central setting.
        /// </summary>
        private void ReCacheCentralSetting()
        {
            _unitOfWork.GetRepository<Approval>().ReCache();
            _unitOfWork.GetRepository<ApprovalItem>().ReCache();
            _unitOfWork.GetRepository<EvaluatorGroup>().ReCache();
            _unitOfWork.GetRepository<EvaluatorGroupItem>().ReCache();
            _unitOfWork.GetRepository<HolidayCalendar>().ReCache();
            _unitOfWork.GetRepository<PurchaseOrg>().ReCache();
            _unitOfWork.GetRepository<PurchaseOrgItem>().ReCache();
            _unitOfWork.GetRepository<PurGroupWeightingKey>().ReCache();
            _unitOfWork.GetRepository<ValueHelp>().ReCache();
            _unitOfWork.GetRepository<EvaluationPercentageConfig>().ReCache();
        }

        /// <summary>
        /// Re all cache table authorization.
        /// </summary>
        private void ReCacheAuthorization()
        {
            _unitOfWork.GetRepository<AuthorityCompany>().ReCache();
            _unitOfWork.GetRepository<AppCompositeRole>().ReCache();
            _unitOfWork.GetRepository<AppCompositeRoleItem>().ReCache();
            _unitOfWork.GetRepository<AppMenu>().ReCache();
            _unitOfWork.GetRepository<UserRoles>().ReCache();
        }

        #endregion

    }
}
