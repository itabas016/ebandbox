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

            if (httpContext.Request.Cookies["User"] == null) return false;

            HttpCookie _cookie = httpContext.Request.Cookies["User"];

            string _userName = _cookie["UserName"];
            string _password = _cookie["Password"];

            if (_userName == "" || _password == "") return false;

            if (accountService.Authentication(_userName, _password) == 0) return true;

            else return false;
        }
    }
}
