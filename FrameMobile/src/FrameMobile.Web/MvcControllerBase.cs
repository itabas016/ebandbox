using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model;
using StructureMap;
using NCore;
using System.IO;
using System.Web;
using AutoMapper;

namespace FrameMobile.Web
{
    public abstract class MvcControllerBase : Controller
    {
        #region Prop

        public virtual IRequestRepository RequestRepository
        {
            get
            {
                if (_requestRepository == null) _requestRepository = ObjectFactory.GetInstance<IRequestRepository>();

                return _requestRepository;
            }
            set { _requestRepository = value; }
        }
        private IRequestRepository _requestRepository;

        protected virtual Encoding DefaultEncoding { get { return Encoding.UTF8; } }

        protected internal virtual bool ShouldCheckSignature { get { return false; } }

        protected internal virtual bool IsMobileInterface { get { return true; } }

        #endregion

        #region Ctor

        public MvcControllerBase()
        {

        }

        #endregion

        #region Execute

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsMobileInterface)
            {
                var mobileParam = this.GetMobileParam();

                var encoding = GetClientInterfaceEnconding(mobileParam);
                Response.ContentEncoding = encoding;
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var mobileParam = this.GetMobileParam();

            HttpContext.Items.Remove(HeaderKeys.POST);
            base.OnActionExecuted(filterContext);
        }

        #endregion

        #region Build Result

        public virtual CommonActionResult BuildResult<T>(Func<bool> checkParameterAction, Func<T> getViewModelActions, bool? checkSignature = null)
            where T : IViewModel
        {
            var result = BuildResult<T>(checkParameterAction, () =>
            {
                var ret = new List<T>();

                var value = getViewModelActions();

                // if the return value is null, we think the record does not exist
                if (value != null && value is T)
                    ret.Add(value);
                return ret;
            }, checkSignature);

            result.UseSingleJsonResult = true;

            return result;
        }

        public virtual CommonActionResult BuildResult<T>(Func<bool> checkParameterAction, Func<IList<T>> getViewModelsActions, bool? checkSignature = null)
            where T : IViewModel
        {
            var defaultViewModels = new List<IViewModel>();
            var actionResult = new CommonActionResult(this.RequestRepository, defaultViewModels);
            actionResult.CommonResult = new CommonResult();

            try
            {
                CheckParamAndGetResult<T>(checkParameterAction, getViewModelsActions, actionResult);
            }
            catch (Exception ex)
            {
                actionResult.CommonResult.result = ResultCode.System_Error;
                LogHelper.Error(string.Format("BuildResult Error:{1}{0}URL:{2}{0}Stacktrace{3}", Environment.NewLine, ex.Message, this.RequestRepository.RawUrl, ex.StackTrace));
            }

            return actionResult;
        }

        private static void CheckParamAndGetResult<T>(Func<bool> checkParameterAction, Func<IList<T>> getViewModelsActions, CommonActionResult actionResult) where T : IViewModel
        {
            if (checkParameterAction())
            {
                var viewModels = getViewModelsActions();
                if (viewModels.Any()) { actionResult.CommonResult.result = ResultCode.Successful; }
                else actionResult.CommonResult.result = ResultCode.No_Record_Found;

                if (viewModels != null && viewModels.Any()) viewModels.ToList().ForEach(s => actionResult.ViewModels.Add(s));
            }
            else
            {
                actionResult.CommonResult.result = ResultCode.Invalid_Parameter;
            }
        }

        protected virtual ActionResult BuildRedirectResult(Func<bool> checkParameterAction, Func<string> getRedirectUrlAction, bool isCheckSign = false)
        {
            string redirectUrl = string.Empty;

            try
            {
                if (checkParameterAction())
                {
                    redirectUrl = getRedirectUrlAction();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("BuildRedirectResult" + ex.Message);
                LogHelper.Info("BuildRedirectResult" + ex.StackTrace);
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                return Redirect(redirectUrl);
            }
            else
            {
                return null;
            }
        }

        protected virtual SecurityAcitonResult BuildResult<T>(Func<T> viewModelActions)
            where T : ISecurityViewModel
        {
            var actionResult = new SecurityAcitonResult();

            try
            {
                var configdata = viewModelActions();
                if (configdata != null)
                {
                    var config = new SecurityConfigData
                    {
                        Data = configdata.JsonResult,
                        Version = configdata.Version,
                        Rate=configdata.Rate
                    };
                    actionResult.Result = 0;
                    actionResult.ConfigData = config;
                }
                else
                {
                    actionResult.Result = -2;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return actionResult;
        }

        #endregion

        #region Param Helper

        protected internal Encoding GetClientInterfaceEnconding(MobileParam param)
        {
            var encoding = default(Encoding);
            if (!param.Encoding.IsNullOrEmpty())
            {
                var name = param.Encoding.ToUpper();
                switch (name)
                {
                    case EncodingNames.UTF8:
                    case "UTF8":
                        encoding = Encoding.UTF8;
                        break;
                }
            }
            if (encoding == null)
            {
                encoding = this.DefaultEncoding;
            }

            return encoding;
        }

        protected virtual bool ValidateModel(List<string> ignoreFields = null)
        {
            if (!this.ModelState.IsValid)
            {
                string errMsg = string.Empty;
                foreach (var k in this.ModelState.Keys)
                {
                    if (ignoreFields == null || !ignoreFields.Contains(k))
                    {
                        foreach (var err in this.ModelState[k].Errors)
                        {
                            errMsg += string.Format("<label>{0}</label><br/>", err.ErrorMessage);
                        }
                    }
                }
                TempData["errorMsg"] = errMsg;

                if (string.IsNullOrWhiteSpace(errMsg))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual MobileParam GetMobileParam()
        {
            var param = new MobileParam(this.RequestRepository);
            param.RequestBody = RequestRepository.PostedDataString;

            return param;
        }

        protected virtual Func<bool> CheckRequiredParams(string imsi)
        {
            return () =>
            {
                var ret = true;
                if (string.IsNullOrEmpty(imsi)) ret = false;

                return ret;
            };
        }

        protected virtual Func<bool> CheckRequiredParams(string imsi, string lcd)
        {
            return () =>
            {
                var ret = true;
                if (string.IsNullOrEmpty(imsi) || string.IsNullOrEmpty(lcd)) ret = false;

                return ret;
            };
        }

        protected virtual Func<bool> CheckRequiredParams(string imsi, string lcd, string mf)
        {
            return () =>
            {
                var ret = true;
                if (string.IsNullOrEmpty(imsi) || string.IsNullOrEmpty(lcd) || string.IsNullOrEmpty(mf)) ret = false;

                return ret;
            };
        }

        #endregion

        #region Resource Helper

        protected string SaveNewsResourceFile(string subFolderName, string dirPath, HttpPostedFileBase file, string fileName)
        {
            fileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName;
            var filePath = GetNewsResourceFilePath(subFolderName, dirPath, fileName);

            file.SaveAs(filePath);
            return filePath;
        }

        protected string SaveThemeResourceFile(string subFolderName, string dirPath, HttpPostedFileBase file, string fileName)
        {
            fileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName;
            var filePath = GetThemeResourceFilePath(subFolderName, dirPath, fileName);

            file.SaveAs(filePath);
            return filePath;
        }

        protected string GetResourcePathNewsBase()
        {
            return Server.MapPath(ConfigKeys.TYD_NEWS_RESOURCES_DIR_ROOT.ConfigValue());
        }

        protected string GetResourcePathThemeBase()
        {
            return Server.MapPath(ConfigKeys.TYD_THEME_RESOURCES_DIR_ROOT.ConfigValue());
        }

        protected string GetThemeLogoPath(string fileName = "")
        {
            return GetThemeResourceFilePath("Logos", fileName);
        }

        protected string GetRadarCategoryLogoPath(string fileName = "")
        {
            return GetNewsResourceFilePath("Logos", fileName);
        }

        protected string GetOriginalWallPaperPath(string fileName = "")
        {
            return GetThemeResourceFilePath("Originals", fileName);
        }

        protected string GetThumbnailWallPaperPath(string fileName = "")
        {
            return GetThemeResourceFilePath("Thumbnails", fileName);
        }

        protected string GetThemeResourceFilePath(string subFolderName, string fileName = "")
        {
            string dirPath = string.Format("{0}{1}\\", GetResourcePathThemeBase(), subFolderName);
            MakeSureDirExist(dirPath);
            return string.Format("{0}{1}", dirPath, fileName);
        }

        protected string GetNewsResourceFilePath(string subFolderName, string fileName = "")
        {
            string dirPath = string.Format("{0}{1}\\", GetResourcePathNewsBase(), subFolderName);
            MakeSureDirExist(dirPath);
            return string.Format("{0}{1}", dirPath, fileName);
        }

        protected string GetThemeResourceFilePath(string subFolderName, string dirPath, string fileName)
        {
            var filePath = string.Empty;
            if (ConfigKeys.USING_SHARED_RESOURCE_FOLDER.ConfigValue().ToBoolean())
            {
                filePath = GetThemeResourceFilePath(subFolderName, fileName);
            }
            else
            {
                MakeSureDirExist(dirPath);
                filePath = Path.Combine(dirPath, fileName);
            }

            return filePath;
        }

        protected string GetNewsResourceFilePath(string subFolderName, string dirPath, string fileName)
        {
            var filePath = string.Empty;
            if (ConfigKeys.USING_SHARED_RESOURCE_FOLDER.ConfigValue().ToBoolean())
            {
                filePath = GetNewsResourceFilePath(subFolderName, fileName);
            }
            else
            {
                MakeSureDirExist(dirPath);
                filePath = Path.Combine(dirPath, fileName);
            }

            return filePath;
        }

        private void MakeSureDirExist(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        #endregion
    }
}
