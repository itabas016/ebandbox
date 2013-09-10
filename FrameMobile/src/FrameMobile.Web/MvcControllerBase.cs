﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model;
using StructureMap;
using NCore;

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

        #endregion
    }
}