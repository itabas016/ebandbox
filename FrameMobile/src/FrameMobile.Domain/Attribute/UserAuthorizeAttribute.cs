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

            if (accountService.Authentication(UserName, Password) == 0)
            {
                if (CurrentUser != null)
                {
                    var userGroupIds = CurrentUser.UserGroupIds.GetIds();
                    foreach (var userGroupId in userGroupIds)
                    {
                        if (userGroupId == 1)//super admin
                        {
                            return true;
                        }
                        if (userGroupId != 0 && userGroupId != 1 && !string.IsNullOrEmpty(UserGroupTypes))
                        {
                            var usergroup = accountService.GetUserGroup(userGroupId);
                            if (usergroup != null && UserGroupTypes.Contains(usergroup.Type))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            else return false;
        }
    }
}
