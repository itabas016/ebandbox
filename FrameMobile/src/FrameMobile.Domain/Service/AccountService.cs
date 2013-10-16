using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public class AccountService : NewsServiceBase, IAccountService
    {
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

        public void Login(string userName, string password)
        {

        }
    }
}
