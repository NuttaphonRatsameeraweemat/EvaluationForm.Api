using AutoMapper;
using EVF.Data.Pocos;
using EVF.Tranfer.Service.Data.Pocos;

namespace EVF.Tranfer.Service.Api.Extensions
{
    public class MappingProfiles : Profile
    {

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfiles" /> class.
        /// </summary>
        public MappingProfiles()
        {
            CreateMap<ZSPE_02, EvaluationSapResult>();
            CreateMap<EvaluationSapResult, ZSPE_02>();
            CreateMap<SPE_TRANSAC_PO_QA, VendorTransection>();
            CreateMap<VendorTransection, SPE_TRANSAC_PO_QA>();
        }

        #endregion

    }
}
