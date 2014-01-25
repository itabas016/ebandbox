using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Domain.Service;
using FrameMobile.Model.Account;
using StructureMap;

namespace FrameMobile.Domain
{
    public class AuthorizeAttributeBase : AuthorizeAttribute
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

        public string UserName { get { return cookieService.TryGet("UserName"); } }

        public string Password { get { return cookieService.TryGet("Password"); } }

        public User CurrentUser { get { return accountService.GetUser(UserName); } }

        public string UserGroups { get; set; }

        public string UserGroupTypes { get; set; }

        #endregion
    }
}
