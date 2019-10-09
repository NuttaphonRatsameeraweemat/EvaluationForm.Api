using AutoMapper;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Inbox.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Inbox.Bll
{
    public class TaskBll : ITaskBll
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
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="k2Service">The k2 service provides k2 service functionality.</param>
        public TaskBll(IUnitOfWork unitOfWork, IK2Service k2Service, IManageToken token, IConfigSetting config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _k2Service = k2Service;
            _token = token;
            _config = config;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        public void GetTaskList()
        {

        }

        #endregion

    }
}
