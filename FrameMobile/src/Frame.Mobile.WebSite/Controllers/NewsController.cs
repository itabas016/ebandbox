using System;
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

        public ActionResult SourceList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<NewsSourceView>> getsourcelist = () => NewsService.GetSourceList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsourcelist);

            return Content(actionResult.ToString());
        }

        public ActionResult LoadModeList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<NewsLoadModeView>> getloadmodelist = () => NewsService.GetLoadModeList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getloadmodelist);

            return Content(actionResult.ToString());
        }

        public ActionResult CategoryList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<NewsCategoryView>> getcategorylist = () => NewsService.GetCategoryList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getcategorylist);

            return Content(actionResult.ToString());
        }

        public ActionResult SubCategoryList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<NewsSubCategoryView>> getsubcategorylist = () => NewsService.GetSubCategoryList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsubcategorylist);

            return Content(actionResult.ToString());
        }

        public ActionResult NewsList(string imsi, int categoryId, int startnum = 1, int num = 10)
        {
            var mobileParams = GetMobileParam();
            int totalCount = 0;

            Func<IList<TouTiaoContentView>> gettoutiaocontentlist = () => NewsService.GetTouTiaoContentList(mobileParams, categoryId, startnum, num, out totalCount);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), gettoutiaocontentlist);

            actionResult.Total = totalCount;
            return Content(actionResult.ToString());
        }
    }
}
