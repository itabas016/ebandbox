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
    public class WallPaperController : MvcControllerBase
    {
        public IWallPaperService WallPaperService
        {
            get
            {
                if (_wallPaperService == null)
                    _wallPaperService = ObjectFactory.GetInstance<IWallPaperService>();

                return _wallPaperService;
            }
            set
            {
                _wallPaperService = value;
            }
        }
        private IWallPaperService _wallPaperService;

        public ActionResult CategoryList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<WallPaperCategoryView>> getcategorylist = () => WallPaperService.GetCategoryViewList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getcategorylist);

            return Content(actionResult.ToString());
        }

        public ActionResult SubCategoryList(string imsi)
        {
            var mobileParams = GetMobileParam();

            Func<IList<WallPaperSubCategoryView>> getsubcategorylist = () => WallPaperService.GetSubCategoryViewList(mobileParams);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getsubcategorylist);

            return Content(actionResult.ToString());
        }

        public ActionResult WallPaperList(string imsi, string categoryIds, string subcategoryIds, int startnum = 1, int num = 10)
        {
            var mobileParams = GetMobileParam();
            int totalCount = 0;

            Func<IList<WallPaperView>> getwallpaperlist = () => WallPaperService.GetWallPaperViewList(mobileParams, categoryIds, subcategoryIds, startnum, num, out totalCount);

            var actionResult = BuildResult(this.CheckRequiredParams(imsi), getwallpaperlist);

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
