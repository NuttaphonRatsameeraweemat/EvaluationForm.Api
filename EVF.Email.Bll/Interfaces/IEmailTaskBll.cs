using EVF.Email.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Interfaces
{
    public interface IEmailTaskBll
    {
        /// <summary>
        /// Save email task action.
        /// </summary>
        /// <param name="model">The email task information.</param>
        void Save(EmailTaskViewModel model);
        /// <summary>
        /// Update email task status.
        /// </summary>
        /// <param name="ids">The email task identitys.</param>
        /// <param name="status">The status change.</param>
        void UpdateEmailTaskStatus(int[] ids, string status);
    }
}
