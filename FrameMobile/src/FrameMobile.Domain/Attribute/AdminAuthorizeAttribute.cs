using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FrameMobile.Domain.Service;
using FrameMobile.Model.Account;
using StructureMap;

namespace FrameMobile.Domain
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttributeBase
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser != null)
            {
                var usergroup = accountService.GetUserGroup(CurrentUser.UserGroupId);
                if (usergroup != null && (usergroup.Type == 1 || usergroup.Type == 2))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (CurrentUser != null)
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
