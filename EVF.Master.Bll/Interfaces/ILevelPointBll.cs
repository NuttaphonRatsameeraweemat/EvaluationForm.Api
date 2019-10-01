using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface ILevelPointBll
    {
        /// <summary>
        /// Get level point list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LevelPointViewModel> GetList();
        /// <summary>
        /// Get Detail of level point.
        /// </summary>
        /// <param name="id">The identity of level point group.</param>
        /// <returns></returns>
        LevelPointViewModel GetDetail(int id);
        /// <summary>
        /// Insert new level point group.
        /// </summary>
        /// <param name="model">The level point information value.</param>
        /// <returns></returns>
        ResultViewModel Save(LevelPointViewModel model);
        /// <summary>
        /// Update level point group.
        /// </summary>
        /// <param name="model">The level point information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(LevelPointViewModel model);
        /// <summary>
        /// Remove level point group.
        /// </summary>
        /// <param name="id">The identity of level point group.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Validate level point is using in evaluation template or not.
        /// </summary>
        /// <param name="id">The level point identity.</param>
        /// <returns></returns>
        bool IsUse(int id);
        /// <summary>
        /// Set flag is use in level point.
        /// </summary>
        /// <param name="ids">The level point identity.</param>
        /// <param name="isUse">The flag is using.</param>
        void SetIsUse(int id, bool isUse);
    }
}
