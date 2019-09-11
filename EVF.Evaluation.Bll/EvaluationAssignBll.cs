using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Evaluation.Bll
{
    public class EvaluationAssignBll : IEvaluationAssignBll
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
        /// Initializes a new instance of the <see cref="EvaluationAssignBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationAssignBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get EvaluationAssign list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationAssignViewModel> GetEvaluators(int evaluationId)
        {
            var result = new List<EvaluationAssignViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId);
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            foreach (var item in data)
            {
                var temp = empList.FirstOrDefault(x => x.EmpNo == item.EmpNo);
                result.Add(new EvaluationAssignViewModel
                {
                    Id = item.Id,
                    EmpNo = item.EmpNo,
                    AdUser = item.AdUser,
                    EvaluationId = item.EvaluationId,
                    IsReject = item.IsReject.Value,
                    UserType = item.UserType,
                    FullName = string.Format(ConstantValue.EmpTemplate, temp?.FirstnameTh, temp?.LastnameTh)
                });
            }
            return result;
        }

        public void Save(int evaluationId, string purchasingAdUser, string[] userList)
        {
            var result = new List<EvaluationAssign>();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();

            result.Add(this.InitialEvaluationAssign(evaluationId, empList.FirstOrDefault(x => x.Aduser == purchasingAdUser), ConstantValue.UserTypePurchasing));

        }

        /// <summary>
        /// Initial EvaluationAssign Model.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        /// <param name="emp">The employee information.</param>
        /// <param name="userType">The user type.</param>
        /// <param name="isReject">The reject is true or not.</param>
        /// <returns></returns>
        private EvaluationAssign InitialEvaluationAssign(int evaluationId, Hremployee emp, string userType, bool isReject = false)
        {
            return new EvaluationAssign
            {
                EvaluationId = evaluationId,
                AdUser = emp?.Aduser,
                EmpNo = emp?.EmpNo,
                IsReject = isReject,
                UserType = userType
            };
        }

        #endregion

    }
}
