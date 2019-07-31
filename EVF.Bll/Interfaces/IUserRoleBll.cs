using EVF.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
{
    public interface IUserRoleBll
    {
        IEnumerable<UserRoleViewModel> GetList();
        UserRoleViewModel GetDetail(string adUser);
        ResultViewModel Save(UserRoleViewModel model);
        ResultViewModel Edit(UserRoleViewModel model);
    }
}
