using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Frame.Mobile.WebSite.Filters;
using Frame.Mobile.WebSite.Models;
using FrameMobile.Web;
using FrameMobile.Model;
using FrameMobile.Domain.Service;
using StructureMap;
using FrameMobile.Model.Account;
using System.Drawing;
using FrameMobile.Core;
using System.IO;

namespace Frame.Mobile.WebSite.Controllers
{
    public class AccountController : MvcControllerBase
    {
        #region Prop

        protected override bool IsMobileInterface { get { return false; } }

        private IAccountService _accountService;
        public IAccountService AccountService
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
        public ICookieService CookieService
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

        #endregion

        #region Ctor

        public AccountController(IAccountService accountService, ICookieService cookieService)
        {
            this.AccountService = accountService;
            this.CookieService = cookieService;
        }

        #endregion

        #region Login and Logff

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginView model, string returnUrl)
        {
            if (ModelState.IsValid && AccountService.Login(model.UserName, model.Password) && Session["VerificationCode"].ToString() == model.VerificationCode.ToUpper())
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterView model)
        {
            if (ModelState.IsValid && Session["VerificationCode"].ToString() == model.VerificationCode.ToUpper())
            {
                try
                {
                    AccountService.CreateUser(model);
                    AccountService.Login(model.Name, model.Password);
                    return RedirectToAction("Manage", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        #endregion

        #region Manage

        public ActionResult Manage()
        {
            return View();
        }

        #endregion

        #region Users

        public ActionResult UserList()
        {
            var users = AccountService.GetUserList();

            return View(users);
        }

        [HttpGet]
        public ActionResult UserAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserAdd(User model)
        {
            var user = AccountService.AddUser(model);
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult UserEdit(int userId)
        {
            var user = AccountService.GetUser(userId);
            ViewData["IsUpdate"] = true;
            return View("UserAdd", user);
        }

        [HttpPost]
        public ActionResult UserEdit(User model)
        {
            var ret = AccountService.UpdateUser(model);

            return RedirectToAction("UserList");
        }

        public ActionResult UserDelete(int userId)
        {
            var ret = AccountService.DeleteUser(userId);
            return RedirectToAction("UserList");
        }

        #endregion

        #region ChangePassword

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountService.ChangePassword(model);
                    return RedirectToAction("Manage", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        #endregion

        #region Helpers

        public ActionResult VerificationCode()
        {
            var _verificationText = AuthHelper.VerificationText(4).ToUpper();
            Session["VerificationCode"] = _verificationText;

            int _width = 60, _height = 20;
            SizeF _verificationTextSize;
            Bitmap _bitmap = new Bitmap(Server.MapPath("~/Content/account/images/linentexture.jpg"), true);
            TextureBrush _brush = new TextureBrush(_bitmap);

            Font _font = new Font("Arial", 14, FontStyle.Bold);
            Bitmap _image = new Bitmap(_width, _height);
            Graphics _g = Graphics.FromImage(_image);
            //清空背景色
            _g.Clear(Color.White);
            //绘制验证码
            _verificationTextSize = _g.MeasureString(_verificationText, _font);
            _g.DrawString(_verificationText, _font, _brush, (_width - _verificationTextSize.Width) / 2, (_height - _verificationTextSize.Height) / 2);
            MemoryStream stream = new MemoryStream();
            _image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //_image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            return File(stream.ToArray(), @"image/jpeg");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
