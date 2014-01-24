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
                var userGroupIds = CurrentUser.UserGroupIds.GetIds();
                foreach (var userGroupId in userGroupIds)
                {
                    if (userGroupId != 0)
                    {
                        var usergroup = accountService.GetUserGroup(userGroupId);
                        if (usergroup != null && UserGroups.Contains(usergroup.Name))
                        {
                            return true;
                        }
                    }
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
