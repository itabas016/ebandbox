using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
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

        #region Register Login

        public int CreateUser(RegisterView model)
        {
            if (model != null)
            {
                var exist = dbContextService.Exists<User>(x => x.Name == model.Name.ToLower());
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
            if (user != null && userName.Equals(user.Name) && hash.Equals(user.Password))
            {
                user.LastLoginTime = DateTime.Now;
                var ret = dbContextService.Update<User>(user);
                return true;
            }
            return false;
        }

        #endregion

        #region Change Info Password

        public int ChangePassword(LocalPasswordView model, string userName)
        {
            var user = dbContextService.Single<User>(x => x.Name == userName);
            if (model != null && user != null)
            {
                user.Password = model.NewPassword.GetMD5Hash();
                user.LastModifiedTime = DateTime.Now;
                var ret = dbContextService.Update<User>(user);
                return ret;
            }
            return 0;
        }

        public int ChangeInfo(User model)
        {
            var user = dbContextService.Single<User>(x => x.Name == model.Name);
            if (model != null && user != null && user.Password == model.Password.GetMD5Hash())
            {
                user.UserGroupId = model.UserGroupId;
                user.Email = model.Email;
                user.PostCode = model.PostCode;
                user.QQ = model.QQ;
                user.Tel = model.Tel;
                user.Address = model.Address;
                user.SecurityQuestion = model.SecurityQuestion;
                user.SecurityAnswer = model.SecurityAnswer;
                user.LastModifiedTime = DateTime.Now;
                user.Comment = model.Comment;
                var ret = dbContextService.Update<User>(user);
                return ret;
            }
            return 0;
        }

        #endregion

        #region User

        public IList<User> GetUserList()
        {
            var users = dbContextService.All<User>().ToList();
            return users;
        }

        public User GetUser(int userId)
        {
            var user = dbContextService.Single<User>(userId);
            return user;
        }

        public User GetUser(string userName)
        {
            var user = dbContextService.Single<User>(x => x.Name == userName);
            return user;
        }

        public int AddUser(User model)
        {
            if (model != null)
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

        #endregion

        #region UserGroup

        public IList<UserGroup> GetUserGroupList()
        {
            var userGrouplist = dbContextService.All<UserGroup>().ToList();

            return userGrouplist;
        }

        public UserGroup GetUserGroup(int userGroupId)
        {
            var userGroup = dbContextService.Single<UserGroup>(userGroupId);

            return userGroup;
        }

        public int AddUserGroup(UserGroup model)
        {
            if (model != null)
            {
                var exist = dbContextService.Exists<UserGroup>(x => x.Name == model.Name.ToLower());
                if (!exist)
                {
                    var ret = dbContextService.Add<UserGroup>(model);
                    return (int)ret;
                }
            }
            return 0;
        }

        public int UpdateUserGroup(UserGroup model)
        {
            var userGroup = dbContextService.Single<UserGroup>(model.Id);

            userGroup.Name = model.Name;
            userGroup.Description = model.Description;

            var ret = dbContextService.Update<UserGroup>(model);
            return ret;
        }

        public int DeleteUserGroup(int userGroupId)
        {
            var ret = dbContextService.Delete<UserGroup>(userGroupId);
            return ret;
        }

        #endregion

        #region Inviation Code

        public IList<InvitationCode> GetInvitationCodelist()
        {
            var codelist = dbContextService.All<InvitationCode>().ToList();
            return codelist;
        }

        public bool AuthInvitationCode(string invitationCode)
        {
            var isInvited = dbContextService.Exists<InvitationCode>(x => x.Code == invitationCode && x.Status == 1);

            return isInvited;
        }

        public void ExpireInvitationCode(string invitationCode)
        {
            var code = dbContextService.Single<InvitationCode>(x => x.Code == invitationCode);
            code.Status = 0;

            var ret = dbContextService.Update<InvitationCode>(code);
        }

        public int GenerateInvitationCode(out string output)
        {
            var code = AuthHelper.VerificationText(6);
            output = string.Empty;

            var exist = dbContextService.Exists<InvitationCode>(x => x.Code == code);
            if (!exist)
            {
                var invitationCode = new InvitationCode()
                {
                    Code = code
                };

                var ret = dbContextService.Add<InvitationCode>(invitationCode);
                output = invitationCode.Code;
                return (int)ret;
            }

            return 0;
        }

        public int InvitationCodeDelete(int invitationCodeId)
        {
            var ret = dbContextService.Delete<InvitationCode>(invitationCodeId);

            return ret;
        }

        #endregion

        #region Helper

        public int Authentication(string userName, string password)
        {
            var _user = dbContextService.Single<User>(x => x.Name == userName);
            if (_user == null) return 1;
            else if (_user.Password != password) return 2;
            else return 0;
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

        #endregion
    }
}
