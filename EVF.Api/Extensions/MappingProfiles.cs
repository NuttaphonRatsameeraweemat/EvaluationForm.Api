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
            CreateMap<AppCompositeRole, RoleViewModel>();
            CreateMap<RoleViewModel, AppCompositeRole>();
            CreateMap<Hremployee, UserRoleViewModel>();
            CreateMap<UserRoleViewModel, Hremployee>();
            CreateMap<Performance, PerformanceViewModel>();
            CreateMap<PerformanceViewModel, Performance>();
            CreateMap<PerformanceGroup, PerformanceGroupViewModel>();
            CreateMap<PerformanceGroupViewModel, PerformanceGroup>();
            CreateMap<ValueHelp, ValueHelpViewModel>();
            CreateMap<ValueHelpViewModel, ValueHelp>();
            CreateMap<Hrcompany, HrCompanyViewModel>();
            CreateMap<HrCompanyViewModel, Hrcompany>();
            CreateMap<Hrorg, HrOrgViewModel>();
            CreateMap<HrOrgViewModel, Hrorg>();
            CreateMap<Hremployee, HrEmployeeViewModel>();
            CreateMap<HrEmployeeViewModel, Hremployee>();
            CreateMap<Approval, ApprovalViewModel>();
            CreateMap<ApprovalViewModel, Approval>();
            CreateMap<ApprovalItem, ApprovalItemViewModel>();
            CreateMap<ApprovalItemViewModel, ApprovalItem>();
        }

        #endregion
        
    }
}
