using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll
{
    public class SummaryEmailTaskBll : ISummaryEmailTaskBll
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
        /// <summary>
        /// The email task provides email task functionality.
        /// </summary>
        private readonly IEmailTaskBll _emailTask;
        /// <summary>
        /// The email service provides email service functionality.
        /// </summary>
        private readonly IEmailService _emailService;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEmailTaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEmailTaskBll(IUnitOfWork unitOfWork, IManageToken token, IEmailTaskBll emailTask, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _emailTask = emailTask;
            _emailService = emailService;
        }

        #endregion

        #region [Methods]

        public void ExecuteEmailTaskWaiting()
        {

        }

        public void ProcessSummaryTask()
        {

        }

        public void ProcessSummaryTaskReject()
        {

        }

        public void ProcessSummaryTaskEvaWaiting()
        {
            
        }

        #endregion

    }
}
