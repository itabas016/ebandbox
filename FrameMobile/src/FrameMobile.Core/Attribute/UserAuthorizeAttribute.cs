using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FrameMobile.Core
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 核心【验证用户是否登陆】
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //检查Cookies["User"]是否存在
            if (httpContext.Request.Cookies["User"] == null) return false;
            //验证用户名密码是否正确
            HttpCookie _cookie = httpContext.Request.Cookies["User"];
            string _userName = _cookie["UserName"];
            string _password = _cookie["Password"];
            if (_userName == "" || _password == "") return false;

            //if (_userRsy.Authentication(_userName, _password) == 0) return true;
            else return false;
        }
    }
}
