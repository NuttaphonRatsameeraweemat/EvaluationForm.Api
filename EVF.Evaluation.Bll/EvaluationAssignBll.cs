using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

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
        /// Get EvaluationAssign.
        /// </summary>
        /// <param name="evaluationAssignId">The identity evaluation and assign.</param>
        /// <returns></returns>
        public EvaluationAssignViewModel GetEvaluator(int evaluationAssignId)
        {
            var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.Id == evaluationAssignId).FirstOrDefault();
            return this.InitialEvaluationAssignViewModel(data,
                _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == data.EmpNo).FirstOrDefault());
        }

        /// <summary>
        /// Get EvaluationAssign list.
        /// </summary>
        /// <param name="evaluationId">The identity of evaluation.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationAssignViewModel> GetEvaluators(int evaluationId)
        {
            var result = new List<EvaluationAssignViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId);
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            foreach (var item in data)
            {
                result.Add(this.InitialEvaluationAssignViewModel(item, empList.FirstOrDefault(x => x.EmpNo == item.EmpNo)));
            }
            return result;
        }

        /// <summary>
        /// Initial Evaluation Assign Viewmodel.
        /// </summary>
        /// <param name="item">The evaluation entity model.</param>
        /// <param name="emp">The employee entity model.</param>
        /// <returns></returns>
        private EvaluationAssignViewModel InitialEvaluationAssignViewModel(EvaluationAssign item, Hremployee emp)
        {
            return new EvaluationAssignViewModel
            {
                Id = item.Id,
                EmpNo = item.EmpNo,
                AdUser = item.AdUser,
                EvaluationId = item.EvaluationId,
                IsReject = item.IsReject.Value,
                IsAction = item.IsAction.Value,
                UserType = item.UserType,
                FullName = string.Format(ConstantValue.EmpTemplate, emp?.FirstnameTh, emp?.LastnameTh)
            };
        }

        /// <summary>
        /// Insert new evaluation assign list.
        /// </summary>
        /// <param name="evaluationId">The evaluation id.</param>
        /// <param name="purchasingAdUser">The purchasing aduser.</param>
        /// <param name="userList">The evaluator user list.</param>
        public void SaveList(int evaluationId, string purchasingAdUser, string[] userList)
        {
            var result = new List<EvaluationAssign>();
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();

            result.Add(this.InitialEvaluationAssign(evaluationId, empList.FirstOrDefault(x => x.Aduser == purchasingAdUser), ConstantValue.UserTypePurchasing));
            foreach (var item in userList)
            {
                if (!result.Any(x => x.AdUser == item))
                {
                    var temp = empList.FirstOrDefault(x => x.Aduser == item);
                    result.Add(this.InitialEvaluationAssign(evaluationId, temp, ConstantValue.UserTypeEvaluator));
                }
            }

            _unitOfWork.GetRepository<EvaluationAssign>().AddRange(result);
        }

        /// <summary>
        /// Validate evaluation save and edit before add data.
        /// </summary>
        /// <param name="model">The evaluation assign value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(EvaluationAssignRequestViewModel model)
        {
            var result = new ResultViewModel();

            if (model.Id != 0)
            {
                var evaAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == model.EvaluationId && x.Id != model.Id);
                if (evaAssign.Any(x => x.AdUser == model.ToAdUser))
                {
                    result = UtilityService.InitialResultError(MessageValue.DuplicateEvaluationAssign, (int)System.Net.HttpStatusCode.BadRequest);
                }
            }
            else
            {
                var evaAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == model.EvaluationId);
                if (evaAssign.Any(x => x.AdUser == model.ToAdUser))
                {
                    result = UtilityService.InitialResultError(MessageValue.DuplicateEvaluationAssign, (int)System.Net.HttpStatusCode.BadRequest);
                }
            }

            return result;
        }

        /// <summary>
        /// Add new evaluator to evaluation form task.
        /// </summary>
        /// <param name="model">The information value evaluator.</param>
        /// <returns></returns>
        public ResultViewModel Save(EvaluationAssignRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = this.InitialEvaluationAssign(model.EvaluationId,
                    _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == model.ToAdUser).FirstOrDefault(),
                    ConstantValue.UserTypeEvaluator);
                _unitOfWork.GetRepository<EvaluationAssign>().Add(data);
                this.UpdateEvaluationStatus(model.EvaluationId);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Edit new evaluator to evaluation form task.
        /// </summary>
        /// <param name="model">The information value evaluator.</param>
        /// <returns></returns>
        public ResultViewModel Edit(EvaluationAssignRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<EvaluationAssign>().GetById(model.Id);
                var emp = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == model.ToAdUser).FirstOrDefault();
                data.EmpNo = emp?.EmpNo;
                data.AdUser = emp?.Aduser;
                data.IsReject = false;
                data.ReasonReject = null;
                _unitOfWork.GetRepository<EvaluationAssign>().Update(data);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Remove evaluation assign task.
        /// </summary>
        /// <param name="id">The evaluation assign identity.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var deleteData = _unitOfWork.GetRepository<EvaluationAssign>().GetById(id);
                _unitOfWork.GetRepository<EvaluationAssign>().Remove(deleteData);
                this.IsEvaluationFinish(deleteData.EvaluationId.Value, deleteData.Id);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Initial EvaluationAssign Model.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        /// <param name="emp">The employee information.</param>
        /// <param name="userType">The user type.</param>
        /// <param name="isReject">The reject is true or not.</param>
        /// <returns></returns>
        private EvaluationAssign InitialEvaluationAssign(int evaluationId, Hremployee emp, string userType, bool isReject = false, bool isAction = false)
        {
            return new EvaluationAssign
            {
                EvaluationId = evaluationId,
                AdUser = emp?.Aduser,
                EmpNo = emp?.EmpNo,
                IsReject = isReject,
                IsAction = isAction,
                UserType = userType
            };
        }

        /// <summary>
        /// Validate evaluation all user is action or not.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        private void IsEvaluationFinish(int evaluationId, int id)
        {
            var evaluationAssigns = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId && x.Id != id);
            if (!evaluationAssigns.Any(x => !x.IsAction.Value))
            {
                var evaluation = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.Id == evaluationId).FirstOrDefault();
                evaluation.Status = ConstantValue.EvaComplete;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(evaluation);
            }
        }

        /// <summary>
        /// Validate evaluation all user is action or not.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        private void UpdateEvaluationStatus(int evaluationId)
        {
            var evaluation = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.Id == evaluationId).FirstOrDefault();
            evaluation.Status = ConstantValue.EvaWaiting;
            _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(evaluation);
        }

        #endregion

    }
}
