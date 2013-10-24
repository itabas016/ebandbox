using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain.Service;
using StructureMap;

namespace FrameMobile.Domain
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IAccountService accountService = ObjectFactory.GetInstance<IAccountService>();

            ICookieService cookieService = ObjectFactory.GetInstance<ICookieService>();

            var userName = cookieService.TryGet("NewsUserName");
            var password = cookieService.TryGet("NewsPassword");
            var user = accountService.GetUser(userName);
            if (user!= null)
            {
                var usergroup = accountService.GetUserGroup(user.UserGroupId);
                if (usergroup != null && usergroup.Name.ToLower() == "administrator")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
