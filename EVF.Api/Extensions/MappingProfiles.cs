using AutoMapper;
using EVF.Authorization.Bll.Models;
using EVF.CentralSetting.Bll.Models;
using EVF.Hr.Bll.Models;
using EVF.Data.Pocos;
using EVF.Master.Bll.Models;

namespace EVF.Api.Extensions
{
    public class MappingProfiles : Profile
    {

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfiles" /> class.
        /// </summary>
        public MappingProfiles()
        {
            this.MappingMasterModel();
            this.MappingAuthorizationModel();
            this.MappingCentralSettingModel();
            this.MappingHrModel();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Create auto mapper profile master model.
        /// </summary>
        public void MappingMasterModel()
        {
            CreateMap<Kpi, KpiViewModel>();
            CreateMap<KpiViewModel, Kpi>();
            CreateMap<KpiGroup, KpiGroupViewModel>();
            CreateMap<KpiGroupViewModel, KpiGroup>();
            CreateMap<KpiGroupItem, KpiGroupItemViewModel>();
            CreateMap<KpiGroupItemViewModel, KpiGroupItem>();
            CreateMap<Period, PeriodViewModel>();
            CreateMap<PeriodViewModel, Period>();
            CreateMap<PeriodItem, PeriodItemViewModel>();
            CreateMap<PeriodItemViewModel, PeriodItem>();
            CreateMap<Grade, GradeViewModel>();
            CreateMap<GradeViewModel, Grade>();
            CreateMap<GradeItem, GradeItemViewModel>();
            CreateMap<GradeItemViewModel, GradeItem>();
            CreateMap<LevelPoint, LevelPointViewModel>();
            CreateMap<LevelPointViewModel, LevelPoint>();
            CreateMap<LevelPointItem, LevelPointItemViewModel>();
            CreateMap<LevelPointItemViewModel, LevelPointItem>();
            CreateMap<Criteria, CriteriaViewModel>();
            CreateMap<CriteriaViewModel, Criteria>();
            CreateMap<CriteriaGroup, CriteriaGroupViewModel>();
            CreateMap<CriteriaGroupViewModel, CriteriaGroup>();
            CreateMap<CriteriaItem, CriteriaItemViewModel>();
            CreateMap<CriteriaItemViewModel, CriteriaItem>();
        }

        /// <summary>
        /// Create auto mapper profile central setting model.
        /// </summary>
        public void MappingCentralSettingModel()
        {
            CreateMap<Approval, ApprovalViewModel>();
            CreateMap<ApprovalViewModel, Approval>();
            CreateMap<ApprovalItem, ApprovalItemViewModel>();
            CreateMap<ApprovalItemViewModel, ApprovalItem>();
            CreateMap<ValueHelp, ValueHelpViewModel>();
            CreateMap<ValueHelpViewModel, ValueHelp>();
        }

        /// <summary>
        /// Create auto mapper profile humam resource model.
        /// </summary>
        public void MappingHrModel()
        {
            CreateMap<Hrcompany, HrCompanyViewModel>();
            CreateMap<HrCompanyViewModel, Hrcompany>();
            CreateMap<Hrorg, HrOrgViewModel>();
            CreateMap<HrOrgViewModel, Hrorg>();
            CreateMap<Hremployee, HrEmployeeViewModel>();
            CreateMap<HrEmployeeViewModel, Hremployee>();
        }

        /// <summary>
        /// Create auto mapper profile authorization model.
        /// </summary>
        public void MappingAuthorizationModel()
        {
            CreateMap<AppCompositeRole, RoleViewModel>();
            CreateMap<RoleViewModel, AppCompositeRole>();
            CreateMap<Hremployee, UserRoleViewModel>();
            CreateMap<UserRoleViewModel, Hremployee>();
        }

        #endregion

    }
}
