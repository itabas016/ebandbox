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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute : AuthorizeAttributeBase
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UserName == "" || Password == "") return false;

            if (accountService.Authentication(UserName, Password) == 0) return true;

            else return false;
        }
    }
}
