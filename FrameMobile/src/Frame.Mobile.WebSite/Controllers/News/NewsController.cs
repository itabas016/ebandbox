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
    public class NewsController : NewsBaseController
    {
        public ActionResult SourceList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsSourceView>> getsourcelist = () => NewsService.GetSourceViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsourcelist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult ExtraAppList(string imsi, int cver = 0, int infver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;
            int ratio = 0;
            var actionResult = default(CommonActionResult);

            switch (infver)
            {
                case 0:
                    Func<IList<OlderNewsExtraAppView>> getolderextraapplist = () => NewsService.GetOlderExtraAppViewList(mobileParams, cver, out sver);
                    actionResult = BuildResult(this.CheckRequiredParams(imsi), getolderextraapplist);
                    break;
                case 1:
                    Func<IList<NewsExtraAppView>> getextraapplist = () => NewsService.GetExtraAppViewList(mobileParams, cver, out sver, out ratio);
                    actionResult = BuildResult(this.CheckRequiredParams(imsi), getextraapplist);
                    actionResult.Ratio = ratio;
                    break;
            }

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult InfAddressList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsInfAddressView>> getinfaddresslist = () => NewsService.GetInfAddressViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getinfaddresslist);

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

        public ActionResult RadarList(string imsi, string lcd, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<NewsRadarView>> getradarlist = () => NewsService.GetNewsRadarViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi, lcd), getradarlist);

            actionResult.ServerVerison = sver;
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

        public ActionResult NewsCollection(string imsi, string lcd, string categoryIds, long stamp, int extracver = 0, bool act = true, int startnum = 1, int num = 10)
        {
            var mobileParams = GetMobileParam();
            int totalCount = 0;
            int extraServerVersion = 0;
            int ratio = 0;

            Func<NewsCollectionView> getnewscollection = () => NewsService.GetNewsCollectionView(mobileParams, stamp, extracver, act, categoryIds, startnum, num, out extraServerVersion, out ratio, out totalCount);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi, lcd), getnewscollection);

            actionResult.Total = totalCount;
            return Content(actionResult.ToString());
        }


    }
}
