using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.Account;
using SubSonic.DataProviders;
using SubSonic.Query;

namespace FrameMobile.Domain.Service
{
    public class AccountService : NewsServiceBase, IAccountService
    {
        IDataProvider provider = ProviderFactory.GetProvider(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING);

        public AccountService()
        {
            UserDBInitialize();
        }

        public int CreateUser(RegisterView model)
        {
            if (model != null)
            {
                var exist = dbContextService.Exists<User>(x => x.Name == model.UserName.ToLower());
                if (!exist)
                {
                    var user = model.To<User>();
                    var ret = dbContextService.Add<User>(user);
                    return (int)ret;
                }
                return -1;
            }
            return 0;
        }

        public bool Login(string userName, string password)
        {
            var hash = password.GetMD5Hash();

            var user = dbContextService.Single<User>(x => x.Name == userName.ToLower());
            if (userName.Equals(user.Name) && hash.Equals(user.Password))
            {
                return true;
            }
            return false;
        }

        public int ChangePassword(LocalPasswordView model)
        {
            var user = dbContextService.Single<User>(x => x.Name == "");
            if (model != null && user != null)
            {
                user.Password = model.NewPassword.GetMD5Hash();
                user.LastModifiedTime = DateTime.Now;
                var ret = dbContextService.Update<User>(user);
                return ret;
            }
            return 0;
        }

        public IList<User> GetUserList()
        {
            var users = dbContextService.All<User>();
            return users.ToList();
        }

        public User GetUser(int userId)
        {
            var user = dbContextService.Single<User>(userId);
            return user;
        }

        public int AddUser(User model)
        {
            if (model!= null)
            {
                var exist = dbContextService.Exists<User>(x => x.Name == model.Name.ToLower());
                if (!exist)
                {
                    model.Password = model.Password.GetMD5Hash();
                    var ret = dbContextService.Add<User>(model);
                    return (int)ret;
                }
            }
            return 0;
        }

        public int UpdateUser(User model)
        {
            var user = dbContextService.Single<User>(model.Id);

            user.Name = model.Name;
            user.Password = model.Password.GetMD5Hash();
            user.LastModifiedTime = DateTime.Now;
            user.Email = model.Email;
            user.Comment = model.Comment;

            var ret = dbContextService.Update<User>(model);
            return ret;
        }

        public int DeleteUser(int userId)
        {
            var ret = dbContextService.Delete<User>(userId);
            return ret;
        }

        private void UserDBInitialize()
        {
            BatchQuery query = new BatchQuery(provider);
            Assembly assembly = Assembly.Load("FrameMobile.Model");
            string spacename = "FrameMobile.Model.Account";

            var migrator = new SubSonic.Schema.Migrator(assembly);
            string[] commands = migrator.MigrateFromModel(spacename, provider);

            foreach (var s in commands)
            {
                query.QueueForTransaction(new QueryCommand(s.Trim(), provider));
            }
            query.ExecuteTransaction();
        }
    }
}
