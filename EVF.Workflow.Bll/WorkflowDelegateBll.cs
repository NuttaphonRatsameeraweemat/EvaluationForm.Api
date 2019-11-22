using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Workflow.Bll
{
    public class WorkflowDelegateBll : IWorkflowDelegateBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The k2 service provides k2 service functionality.
        /// </summary>
        private readonly IK2Service _k2Service;
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
        /// Initializes a new instance of the <see cref="WorkflowDelegateBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="k2Service">The k2 service provides k2 service functionality.</param>
        public WorkflowDelegateBll(IUnitOfWork unitOfWork, IK2Service k2Service, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _k2Service = k2Service;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get workflow delegate list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkflowDelegateViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<WorkflowDelegate>, IEnumerable<WorkflowDelegateViewModel>>(
                _unitOfWork.GetRepository<WorkflowDelegate>().Get());
        }

        /// <summary>
        /// Get workflow delegate detail item.
        /// </summary>
        /// <param name="id">The identity of workflow delegate.</param>
        /// <returns></returns>
        public WorkflowDelegateViewModel GetDetail(int id)
        {
            return _mapper.Map<WorkflowDelegate, WorkflowDelegateViewModel>(
                _unitOfWork.GetRepository<WorkflowDelegate>().GetById(id));
        }

        /// <summary>
        /// Get workflow delegate detail item.
        /// </summary>
        /// <param name="id">The identity of workflow delegate.</param>
        /// <returns></returns>
        public WorkflowDelegateRequestModel GetDelegateInbox()
        {
            var data = _unitOfWork.GetRepository<WorkflowDelegate>().Get(x => x.FromUser == _token.AdUser).FirstOrDefault();

            return (data != null) ? new WorkflowDelegateRequestModel
            {
                Id = data.Id,
                ToUser = data.ToUser,
                StartDate = UtilityService.DateTimeToString(data.StartDate.Value, ConstantValue.DateTimeFormat),
                EndDate = UtilityService.DateTimeToString(data.EndDate.Value, ConstantValue.DateTimeFormat)
            } : null;
        }

        /// <summary>
        /// Save deletegate task fromuser touser.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        public ResultViewModel SaveDelegateFromInbox(WorkflowDelegateRequestModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = new WorkflowDelegate
                {
                    FromUser = _token.AdUser,
                    ToUser = model.ToUser,
                    StartDate = UtilityService.ConvertToDateTime(model.StartDate, ConstantValue.DateTimeFormat),
                    EndDate = UtilityService.ConvertToDateTime(model.EndDate, ConstantValue.DateTimeFormat)
                };
                _unitOfWork.GetRepository<WorkflowDelegate>().Add(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingCreate, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Save deletegate task fromuser touser.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        public ResultViewModel SaveDelegate(WorkflowDelegateViewModel model)
        {
            var result = new ResultViewModel();
            model.StartDate = UtilityService.ConvertToDateTime(model.StartDateString, ConstantValue.DateTimeFormat);
            model.EndDate = UtilityService.ConvertToDateTime(model.EndDateString, ConstantValue.DateTimeFormat);
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _mapper.Map<WorkflowDelegateViewModel, WorkflowDelegate>(model);
                _unitOfWork.GetRepository<WorkflowDelegate>().Add(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingCreate, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Update delegate task user.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        public ResultViewModel UpdateDelegate(WorkflowDelegateViewModel model)
        {
            var result = new ResultViewModel();
            model.StartDate = UtilityService.ConvertToDateTime(model.StartDateString, ConstantValue.DateTimeFormat);
            model.EndDate = UtilityService.ConvertToDateTime(model.EndDateString, ConstantValue.DateTimeFormat);
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _mapper.Map<WorkflowDelegateViewModel, WorkflowDelegate>(model);
                _unitOfWork.GetRepository<WorkflowDelegate>().Update(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingEdit, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Update delegate task user.
        /// </summary>
        /// <param name="model">The information delegate task.</param>
        /// <returns></returns>
        public ResultViewModel UpdateDelegateInbox(WorkflowDelegateRequestModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = new WorkflowDelegate
                {
                    Id = model.Id,
                    FromUser = _token.AdUser,
                    ToUser = model.ToUser,
                    StartDate = UtilityService.ConvertToDateTime(model.StartDate, ConstantValue.DateTimeFormat),
                    EndDate = UtilityService.ConvertToDateTime(model.EndDate, ConstantValue.DateTimeFormat)
                };
                _unitOfWork.GetRepository<WorkflowDelegate>().Update(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingEdit, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Remove delegate workflw task.
        /// </summary>
        /// <param name="id">The identity of delegate.</param>
        /// <returns></returns>
        public ResultViewModel RemoveDelegate(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<WorkflowDelegate>().GetById(id);
                _unitOfWork.GetRepository<WorkflowDelegate>().Remove(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingDelete, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Remove delegate workflw task.
        /// </summary>
        /// <param name="id">The identity of delegate.</param>
        /// <returns></returns>
        public ResultViewModel RemoveDelegateInbox()
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<WorkflowDelegate>().Get(x => x.FromUser == _token.AdUser).FirstOrDefault();
                _unitOfWork.GetRepository<WorkflowDelegate>().Remove(data);
                _k2Service.SetOutofOffice(data.FromUser, data.ToUser, ConstantValue.K2SharingDelete, data.StartDate.Value, data.EndDate.Value);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        #endregion

    }
}
