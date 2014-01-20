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
    public class WallPaperController : ThemeBaseController
    {
        public ActionResult CategoryList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<WallPaperCategoryView>> getcategorylist = () => WallPaperService.GetCategoryViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getcategorylist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult SubCategoryList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<WallPaperSubCategoryView>> getsubcategorylist = () => WallPaperService.GetSubCategoryViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsubcategorylist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult TopicList(string imsi, int cver = 0)
        {
            var mobileParams = GetMobileParam();
            int sver = 0;

            Func<IList<WallPaperTopicView>> gettopiclist = () => WallPaperService.GetTopicViewList(mobileParams, cver, out sver);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), gettopiclist);

            actionResult.ServerVerison = sver;
            return Content(actionResult.ToString());
        }

        public ActionResult WallPaperList(string imsi, int categoryId = 0, int topicId = 0, int subcategoryId = 0, int sort = 0, int startnum = 1, int num = 10)
        {
            var mobileParams = GetMobileParam();
            int totalCount = 0;

            Func<IList<WallPaperView>> getwallpaperlist = () => WallPaperService.GetWallPaperViewList(mobileParams, categoryId, topicId, subcategoryId, sort, startnum, num, out totalCount);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getwallpaperlist);

            actionResult.Total = totalCount;
            return Content(actionResult.ToString());
        }

        public ActionResult WallPaperDetail(string imsi, int id)
        {
            var mobileParams = GetMobileParam();

            Func<WallPaperView> getwallpaper = () => WallPaperService.GetWallPaperViewDetail(mobileParams, id);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getwallpaper);

            return Content(actionResult.ToString());
        }

    }
}
