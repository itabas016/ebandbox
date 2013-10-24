using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Account;

namespace FrameMobile.Domain.Service
{
    public interface IAccountService
    {
        int CreateUser(RegisterView model);
        bool Login(string userName, string password);
        int ChangePassword(LocalPasswordView model, string userName);
        int ChangeInfo(User model);
        IList<User> GetUserList();
        User GetUser(int userId);
        User GetUser(string userName);
        int AddUser(User model);
        int UpdateUser(User model);
        int DeleteUser(int userId);
        int Authentication(string userName, string password);
        IList<UserGroup> GetUserGroupList();
        UserGroup GetUserGroup(int userGroupId);
        int AddUserGroup(UserGroup model);
        int UpdateUserGroup(UserGroup model);
        int DeleteUserGroup(int userGroupId);
    }
}
