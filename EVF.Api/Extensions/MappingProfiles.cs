using AutoMapper;
using EVF.Bll.Models;
using EVF.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }

        #endregion
        
    }
}
