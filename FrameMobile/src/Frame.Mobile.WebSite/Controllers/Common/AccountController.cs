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
using FrameMobile.Web;
using FrameMobile.Model;
using FrameMobile.Domain.Service;
using StructureMap;
using FrameMobile.Model.Account;
using System.Drawing;
using FrameMobile.Core;
using System.IO;
using FrameMobile.Domain;
using NCore;

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

        const int CookieTimeoutSeconds = 1209600;

        public string UserName
        {
            get
            {
                return CookieService.TryGet("NewsUserName");
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

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView model, string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/NewsUI/NewsManage";

            if (ModelState.IsValid && AccountService.Login(model.UserName, model.Password) && Session["VerificationCode"].ToString() == model.VerificationCode.ToUpper())
            {
                CookieService.Set("NewsUserName", model.UserName, CookieTimeoutSeconds);
                CookieService.Set("NewsPassword", model.Password.GetMD5Hash(), CookieTimeoutSeconds);
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult LogOff()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult LackOfPermission()
        {
            return View();
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterView model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            var isInvited = AccountService.AuthInvitationCode(model.InvitationCode);
            if (ModelState.IsValid && Session["VerificationCode"].ToString() == model.VerificationCode.ToUpper() && isInvited)
            {
                try
                {
                    var userId = AccountService.CreateUser(model);
                    if (userId > 0)
                    {
                        AccountService.ExpireInvitationCode(model.InvitationCode);
                    }
                    var ret = AccountService.Login(model.Name, model.Password);
                    if (ret)
                    {
                        return RedirectToAction("Manage", "Account", new { userName = model.Name });
                    }
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

        public ActionResult Manage(string userName)
        {
            var user = AccountService.GetUser(userName);

            return View(user);
        }

        #endregion

        #region Users

        [AdminAuthorize]
        public ActionResult UserList()
        {
            var users = AccountService.GetUserList();
            var userGrouplist = AccountService.GetUserGroupList();

            ViewData["userlist"] = users;
            ViewData["UserGrouplist"] = userGrouplist.GetSelectList();

            return View();
        }

        public ActionResult UserEdit(int userId)
        {
            return RedirectToAction("ChangeInfo", "Account", new { userId = userId });
        }

        public ActionResult UserDelete(int userId)
        {
            var ret = AccountService.DeleteUser(userId);
            return RedirectToAction("UserList");
        }

        #endregion

        #region UserGroup

        public ActionResult UserGroupList()
        {
            var userGrouplist = AccountService.GetUserGroupList();
            ViewData["userGrouplist"] = userGrouplist;
            return View();
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult UserGroupAdd()
        {
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult UserGroupAdd(UserGroup model)
        {
            var ret = AccountService.AddUserGroup(model);
            return RedirectToAction("UserGroupList");
        }

        [AdminAuthorize]
        [HttpGet]
        public ActionResult UserGroupEdit(int userGroupId)
        {
            var userGroup = AccountService.GetUserGroup(userGroupId);
            ViewData["IsUpdate"] = true;
            return View("UserGroupAdd", userGroup);
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult UserGroupEdit(UserGroup model)
        {
            var ret = AccountService.UpdateUserGroup(model);
            return RedirectToAction("UserGroupList");
        }

        [AdminAuthorize]
        public ActionResult UserGroupDelete(int userGroupId)
        {
            var ret = AccountService.DeleteUserGroup(userGroupId);
            return RedirectToAction("UserGroupList");
        }

        #endregion

        #region ChangeInfo

        [HttpGet]
        public ActionResult ChangeInfo(int? userId)
        {
            var userGrouplist = AccountService.GetUserGroupList();
            ViewData["UserGrouplist"] = userGrouplist.GetSelectList();
            var userGroupName = string.Empty;
            var currentuser = userId.HasValue ? AccountService.GetUser(userId.Value) : AccountService.GetUser(UserName);
            var userGroup = AccountService.GetUserGroup(currentuser.UserGroupId);
            if (userGroup != null)
            {
                userGroupName = userGroup.Name;

                if (userGroup.Name.ToLower() == "administrator")
                {
                    ViewData["IsAdmin"] = true;
                }
            }
            ViewData["UserGroupName"] = userGroupName;
            return View(currentuser);
        }

        [HttpPost]
        public ActionResult ChangeInfo(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountService.ChangeInfo(model);
                    return RedirectToAction("Manage", "Account", new { userName = model.Name });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        #endregion

        #region ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountService.ChangePassword(model, UserName);
                    CookieService.Remove("NewsPassword");
                    return RedirectToAction("Manage", "Account", new { userName = UserName });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        #endregion

        #region InvitationCode

        [AdminAuthorize]
        public ActionResult InvitationCode()
        {
            var code = string.Empty;
            var invitationCode = AccountService.GenerateInvitationCode(out code);

            return Content(code);
        }

        [AdminAuthorize]
        public ActionResult InvitationCodeList()
        {
            var codelist = AccountService.GetInvitationCodelist();
            ViewData["invitationCodelist"] = codelist;
            return View();
        }

        [AdminAuthorize]
        public ActionResult InvitationCodeDelete(int invitationCodeId)
        {
            var ret = AccountService.InvitationCodeDelete(invitationCodeId);
            return RedirectToAction("InvitationCodelist");
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
