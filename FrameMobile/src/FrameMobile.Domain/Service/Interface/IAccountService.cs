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
        int ChangePassword(LocalPasswordView model);
        IList<User> GetUserList();
        User GetUser(int userId);
        int AddUser(User model);
        int UpdateUser(User model);
        int DeleteUser(int userId);
    }
}
