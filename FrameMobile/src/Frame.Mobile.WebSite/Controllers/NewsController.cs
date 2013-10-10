﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Web;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    public class NewsController : MvcControllerBase
    {
        public INewsService NewsService
        {
            get
            {
                if (_newsService == null)
                    _newsService = ObjectFactory.GetInstance<INewsService>();

                return _newsService;
            }
            set
            {
                _newsService = value;
            }
        }
        private INewsService _newsService;

        public ActionResult TimeConvert(long stamp, string time)
        {
            var ret = _newsService.TimeConvert(time, stamp);
            return Content(ret);
        }

        public ActionResult SourceList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsSourceView>> getsourcelist = () => NewsService.GetSourceViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsourcelist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult ExtraAppList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsExtraAppView>> getextraapplist = () => NewsService.GetExtraAppViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getextraapplist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult CategoryList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsCategoryView>> getcategorylist = () => NewsService.GetCategoryViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getcategorylist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult SubCategoryList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<NewsSubCategoryView>> getsubcategorylist = () => NewsService.GetSubCategoryViewList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsubcategorylist);

            return Content(actionResult.ToString());
        }

        public ActionResult NewsList(string imsi, string lcd, string categoryIds, long stamp, bool act = true, int startnum = 1, int num = 10)
        {
            var mobileParams = GetMobileParam();
            int totalCount = 0;

            Func<IList<NewsContentView>> gettoutiaocontentlist = () => NewsService.GetNewsContentViewList(mobileParams, stamp, act, categoryIds, startnum, num, out totalCount);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi, lcd), gettoutiaocontentlist);

            actionResult.Total = totalCount;
            return Content(actionResult.ToString());
        }
    }
}
