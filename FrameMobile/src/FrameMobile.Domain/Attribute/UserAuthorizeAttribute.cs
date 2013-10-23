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
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IAccountService accountService = ObjectFactory.GetInstance<IAccountService>();

            ICookieService cookieService = ObjectFactory.GetInstance<ICookieService>();

            var userName = cookieService.TryGet("NewsUserName");
            var password = cookieService.TryGet("NewsPassword");

            if (userName == "" || password == "") return false;

            if (accountService.Authentication(userName, password) == 0) return true;

            else return false;
        }
    }
}
