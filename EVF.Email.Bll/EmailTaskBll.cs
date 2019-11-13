using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
using EVF.Email.Bll.Models;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EVF.Email.Bll
{
    public class EmailTaskBll : IEmailTaskBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EmailTaskBll(IUnitOfWork unitOfWork, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Save email task action.
        /// </summary>
        /// <param name="model">The email task information.</param>
        public void Save(EmailTaskViewModel model)
        {
            using (var scope = new TransactionScope())
            {
                int emailTaskId = this.SaveEmailTask(model);
                this.SaveEmailTaskContent(emailTaskId, model.Content);
                this.SaveEmailTaskReceiver(emailTaskId, model.Receivers);
                _unitOfWork.Complete(scope);
            }
        }

        /// <summary>
        /// Update email task status.
        /// </summary>
        /// <param name="ids">The email task identitys.</param>
        /// <param name="status">The status change.</param>
        public void UpdateEmailTaskStatus(int[] ids, string status)
        {
            using (var scope = new TransactionScope())
            {
                var emailTask = _unitOfWork.GetRepository<EmailTask>().Get(x => ids.Contains(x.Id));
                emailTask.Select(c => { c.Status = status; return c; }).ToList();
                _unitOfWork.Complete(scope);
            }
        }

        /// <summary>
        /// Save email task.
        /// </summary>
        /// <param name="model">The email task information.</param>
        /// <returns>email task identity.</returns>
        private int SaveEmailTask(EmailTaskViewModel model)
        {
            var emailtask = this.InitialEmailTask(model);
            _unitOfWork.GetRepository<EmailTask>().Add(emailtask);
            _unitOfWork.Complete();
            return emailtask.Id;
        }

        /// <summary>
        /// Save email content.
        /// </summary>
        /// <param name="emailTaskId">The email task identity.</param>
        /// <param name="content">The email content.</param>
        private void SaveEmailTaskContent(int emailTaskId,string content)
        {
            var emailtaskcontent = new EmailTaskContent
            {
                Content = content
            };
            emailtaskcontent.EmailTaskId = emailTaskId;
            _unitOfWork.GetRepository<EmailTaskContent>().Add(emailtaskcontent);
        }

        /// <summary>
        /// Save email task receiver.
        /// </summary>
        /// <param name="emailTaskId">The email task identity.</param>
        /// <param name="receivers">The receiver email task.</param>
        private void SaveEmailTaskReceiver(int emailTaskId, IEnumerable<EmailTaskReceiveViewModel> receivers)
        {
            var emailtaskReceiver = new List<EmailTaskReceiver>();
            foreach (var item in receivers)
            {
                var temp = new EmailTaskReceiver
                {
                    Email = item.Email,
                    FullName = item.FullName,
                    ReceiverType = item.ReceiverType
                };
                emailtaskReceiver.Add(temp);
            }
            emailtaskReceiver.Select(c => { c.EmailTaskId = emailTaskId; return c; }).ToList();
            _unitOfWork.GetRepository<EmailTaskReceiver>().AddRange(emailtaskReceiver);
        }

        /// <summary>
        /// Initial email task entity model.
        /// </summary>
        /// <param name="model">The email task view model.</param>
        /// <returns></returns>
        private EmailTask InitialEmailTask(EmailTaskViewModel model)
        {
            return new EmailTask
            {
                DocNo = model.DocNo,
                TaskBy = _token.AdUser,
                TaskCode = model.TaskCode,
                TaskDate = DateTime.Now,
                Subject = model.Subject,
                Status = model.Status
            };
        }

        #endregion
        
    }
}
