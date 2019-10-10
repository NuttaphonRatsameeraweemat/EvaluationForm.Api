using AutoMapper;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Inbox.Bll.Interfaces;
using EVF.Inbox.Bll.Models;
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

        /// <summary>
        /// Get Task pending list from k2.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskViewModel> GetTaskList()
        {
            var result = new List<TaskViewModel>();
            var taskList = _k2Service.GetWorkList(_token.AdUser);
            foreach (var item in taskList)
            {
                result.Add(new TaskViewModel
                {
                    AllocatedUser = item.AllocatedUser,
                    Folder = item.Folder,
                    Folio = item.Folio,
                    FullName = item.FullName,
                    Name = item.Name,
                    SerialNumber = item.SerialNumber,
                    StartDate = item.StartDate
                });
            }
            return result;
        }

        /// <summary>
        /// Get Task delegate pending list from k2.
        /// </summary>
        /// <param name="fromUser">The user task delegate task.</param>
        /// <returns></returns>
        public IEnumerable<TaskViewModel> GetTaskListDelegate(string fromUser)
        {
            var result = new List<TaskViewModel>();
            var taskList = _k2Service.GetWorkList(fromUser);
            foreach (var item in taskList)
            {
                result.Add(new TaskViewModel
                {
                    AllocatedUser = item.AllocatedUser,
                    Folder = item.Folder,
                    Folio = item.Folio,
                    FullName = item.FullName,
                    Name = item.Name,
                    SerialNumber = item.SerialNumber,
                    StartDate = item.StartDate
                });
            }
            return result;
        }

        #endregion

    }
}
