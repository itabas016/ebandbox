using FrameMobile.Domain.Service;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace FrameMobile.Domain
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NewsAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            IAccountService accountService = ObjectFactory.GetInstance<IAccountService>();

            ICookieService cookieService = ObjectFactory.GetInstance<ICookieService>();

            var userName = cookieService.TryGet("NewsUserName");
            var currentUser = accountService.GetUser(userName);

            base.HandleUnauthorizedRequest(filterContext);

            if (currentUser != null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary 
                                           {
                                               { "action", "LackOfPermission" },
                                               { "controller", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName }
                                           });
            }
        }
    }
}
