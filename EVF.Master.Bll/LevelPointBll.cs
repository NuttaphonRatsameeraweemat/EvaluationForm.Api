using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;

namespace EVF.Master.Bll
{
    public class LevelPointBll : ILevelPointBll
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
        /// Initializes a new instance of the <see cref="LevelPointBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public LevelPointBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get level point list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LevelPointViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<LevelPoint>, IEnumerable<LevelPointViewModel>>(
                   _unitOfWork.GetRepository<LevelPoint>().GetCache());
        }

        /// <summary>
        /// Get Detail of level point.
        /// </summary>
        /// <param name="id">The identity of level point group.</param>
        /// <returns></returns>
        public LevelPointViewModel GetDetail(int id)
        {
            var data = _mapper.Map<LevelPoint, LevelPointViewModel>(
                   _unitOfWork.GetRepository<LevelPoint>().GetById(id));
            data.LevelPointItems = this.GetLevelPointItem(id).ToList();
            return data;
        }

        /// <summary>
        /// Get level point group item.
        /// </summary>
        /// <param name="periodId">The identity level point group.</param>
        /// <returns></returns>
        private IEnumerable<LevelPointItemViewModel> GetLevelPointItem(int levelpointId)
        {
            return _mapper.Map<IEnumerable<LevelPointItem>, IEnumerable<LevelPointItemViewModel>>(
                   _unitOfWork.GetRepository<LevelPointItem>().GetCache(x => x.LevelPointId == levelpointId,
                                                                        x => x.OrderBy(y => y.Sequence)));
        }

        /// <summary>
        /// Insert new level point group.
        /// </summary>
        /// <param name="model">The level point information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(LevelPointViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var levelPointGroup = _mapper.Map<LevelPointViewModel, LevelPoint>(model);
                this.SetIsDefault(model);
                levelPointGroup.CreateBy = _token.EmpNo;
                levelPointGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<LevelPoint>().Add(levelPointGroup);
                _unitOfWork.Complete();
                this.SaveItem(levelPointGroup.Id, model.LevelPointItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheLevelPoint();
            return result;
        }

        /// <summary>
        /// Insert level point group item list.
        /// </summary>
        /// <param name="levelPointId">The identity of level point group.</param>
        /// <param name="levelPointItems">The identity of level point items.</param>
        private void SaveItem(int levelPointId, IEnumerable<LevelPointItemViewModel> levelPointItems)
        {
            var data = _mapper.Map<IEnumerable<LevelPointItemViewModel>, IEnumerable<LevelPointItem>>(levelPointItems);
            data.Select(c => { c.LevelPointId = levelPointId; return c; }).ToList();
            _unitOfWork.GetRepository<LevelPointItem>().AddRange(data);
        }

        /// <summary>
        /// Update level point group.
        /// </summary>
        /// <param name="model">The level point information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(LevelPointViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                this.SetIsDefault(model);
                var levelPointGroup = _unitOfWork.GetRepository<LevelPoint>().GetById(model.Id);
                levelPointGroup.Name = model.Name;
                levelPointGroup.WeightingKey = model.WeightingKey;
                levelPointGroup.IsDefault = model.IsDefault;
                levelPointGroup.LastModifyBy = _token.EmpNo;
                levelPointGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<LevelPoint>().Update(levelPointGroup);
                this.EditItem(levelPointGroup.Id, model.LevelPointItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheLevelPoint();
            return result;
        }

        /// <summary>
        /// Update level point group items.
        /// </summary>
        /// <param name="levelPointId">The identity of level point group.</param>
        /// <param name="levelPointItems">The identity of level point items.</param>
        private void EditItem(int levelPointId, IEnumerable<LevelPointItemViewModel> levelPointItems)
        {
            levelPointItems.Select(c => { c.LevelPointId = levelPointId; return c; }).ToList();
            var data = _unitOfWork.GetRepository<LevelPointItem>().GetCache(x => x.LevelPointId == levelPointId);

            var levelPointItemAdd = levelPointItems.Where(x => x.Id == 0);
            var levelPointItemDelete = data.Where(x => !levelPointItems.Any(y => x.Id == y.Id));

            var levelPointItemUpdate = _mapper.Map<IEnumerable<LevelPointItemViewModel>, IEnumerable<LevelPointItem>>(levelPointItems);
            levelPointItemUpdate = levelPointItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(levelPointId, levelPointItemAdd);
            this.DeleteItem(levelPointItemDelete);
            _unitOfWork.GetRepository<LevelPointItem>().UpdateRange(levelPointItemUpdate);
        }

        /// <summary>
        /// Remove level point group.
        /// </summary>
        /// <param name="id">The identity of level point group.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<LevelPoint>().Remove(
                    _unitOfWork.GetRepository<LevelPoint>().GetById(id));
                this.DeleteItem(_unitOfWork.GetRepository<LevelPointItem>().GetCache(x => x.LevelPointId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheLevelPoint();
            return result;
        }

        /// <summary>
        /// Remove level point group items.
        /// </summary>
        /// <param name="model">The level point group items.</param>
        private void DeleteItem(IEnumerable<LevelPointItem> model)
        {
            _unitOfWork.GetRepository<LevelPointItem>().RemoveRange(model);
        }

        /// <summary>
        /// Set other default false when update this level point is true.
        /// </summary>
        /// <param name="model">The level point information value.</param>
        private void SetIsDefault(LevelPointViewModel model)
        {
            if (model.IsDefault)
            {
                var data = _unitOfWork.GetRepository<LevelPoint>().GetCache(x => x.IsDefault != null && x.IsDefault.Value).FirstOrDefault();
                if (data != null && data.Id != model.Id)
                {
                    data.IsDefault = false;
                    _unitOfWork.GetRepository<LevelPoint>().Update(data);
                }
            }
        }


        /// <summary>
        /// Validate level point is using in evaluation template or not.
        /// </summary>
        /// <param name="id">The level point identity.</param>
        /// <returns></returns>
        public bool IsUse(int id)
        {
            var levelPoint = _unitOfWork.GetRepository<LevelPoint>().GetCache(x => x.Id == id).FirstOrDefault();
            return levelPoint.IsUse.Value;
        }

        /// <summary>
        /// Set flag is use in level point.
        /// </summary>
        /// <param name="ids">The level point identity.</param>
        /// <param name="isUse">The flag is using.</param>
        public void SetIsUse(int id, bool isUse)
        {
            var data = _unitOfWork.GetRepository<LevelPoint>().GetCache(x => x.Id == id).FirstOrDefault();
            data.IsUse = isUse;
            _unitOfWork.GetRepository<LevelPoint>().Update(data);
        }


        /// <summary>
        /// Reload Cache when levelPoint and levelPointItem is change.
        /// </summary>
        private void ReloadCacheLevelPoint()
        {
            _unitOfWork.GetRepository<LevelPoint>().ReCache();
            _unitOfWork.GetRepository<LevelPointItem>().ReCache();
        }

        #endregion

    }
}
