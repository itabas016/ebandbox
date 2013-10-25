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
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        #region Prop

        private IAccountService _accountService;
        public IAccountService accountService
        {
            get
            {
                if (_accountService == null)
                {
                    _accountService = ObjectFactory.GetInstance<IAccountService>();
                }
                return _accountService;
            }
            set
            {
                _accountService = value;
            }
        }

        private ICookieService _cookieService;
        public ICookieService cookieService
        {
            get
            {
                if (_cookieService == null)
                {
                    _cookieService = ObjectFactory.GetInstance<ICookieService>();
                }
                return _cookieService;
            }
            set
            {
                _cookieService = value;
            }
        }

        public string UserName { get { return cookieService.TryGet("NewsUserName"); } }

        public string Password { get { return cookieService.TryGet("NewsPassword"); } }

        public User CurrentUser { get { return accountService.GetUser(UserName); } }

        #endregion

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser != null)
            {
                var usergroup = accountService.GetUserGroup(CurrentUser.UserGroupId);
                if (usergroup != null && usergroup.Name.ToLower() == "administrator")
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
