using AutoMapper;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EVF.CentralSetting.Bll
{
    public class ValueHelpBll : IValueHelpBll
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueHelpBll" /> class.
        /// </summary>
        public ValueHelpBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get ValueHelp List by type.
        /// </summary>
        /// <param name="type">The type of value.</param>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> Get(string type)
        {
            return _mapper.Map<IEnumerable<ValueHelp>, IEnumerable<ValueHelpViewModel>>(
                _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == type, x => x.OrderBy(y => y.Sequence)));
        }

        /// <summary>
        /// Get PurGroup ValueHelp List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValueHelpViewModel> GetPurGroup()
        {
            string[] weightingKeys = new string[] { "A2", "A3", "A4", "A5" };
            var result = new List<ValueHelpViewModel>();
            var purGroup = _unitOfWork.GetRepository<PurGroupWeightingKey>().GetCache(x => x.EvaStatus.Value && weightingKeys.Contains(x.WeightingKey));
            foreach (var item in purGroup)
            {
                if (!result.Any(x => x.ValueKey == item.PurGroup))
                {
                    result.Add(new ValueHelpViewModel { ValueKey = item.PurGroup, ValueText = string.Format("{0} - {1}", item.PurGroup, item.Description) });
                }
            }
            return result;
        }

        #endregion


    }
}
