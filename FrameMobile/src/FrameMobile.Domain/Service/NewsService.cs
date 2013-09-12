using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;
using NCore;

namespace FrameMobile.Domain.Service
{
    public class NewsService : INewsService
    {
        private IDbContextService _dbContextService;
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceList(MobileParam mobileParams)
        {
            var sourcelist = dbContextService.Find<NewsSource>(x => x.Status == 1);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache]
        public IList<NewsLoadModeView> GetLoadModeList(MobileParam mobileParams)
        {
            var loadmodelist = dbContextService.Find<NewsLoadMode>(x => x.Status == 1);
            return loadmodelist.To<IList<NewsLoadModeView>>();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams)
        {
            var categorylist = dbContextService.Find<NewsCategory>(x => x.Status == 1);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache]
        public IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int newsId, bool action, int categoryId, int startnum, int num, out int totalCount)
        {
            var contentlist = new List<TouTiaoContentModel>();

            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.CategoryId == categoryId && x.Status == 1);

            foreach (var item in subcategorylist)
            {
                if (action)
                {
                    var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == item.Id && x.Id > newsId && x.Status == 1);
                    contentlist = contentlist.Union(subcategorycontentlist).ToList();
                }
                else
                {
                    var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == item.Id && x.Id < newsId && x.Status == 1);
                    contentlist = contentlist.Union(subcategorycontentlist).ToList();
                }
            }
            totalCount = contentlist.Count;
            var result = contentlist.To<IList<TouTiaoContentView>>();
            return result.Skip(startnum - 1).Take(num).ToList();
        }


        private string GetImageURLByResloution(MobileParam mobileParams, long newsId)
        {
            if (string.IsNullOrEmpty(mobileParams.Resolution))
            {
                return string.Empty;
            }
            var lcdArray = mobileParams.Resolution.ToLower().Split('x');
            var width = lcdArray[0].ToInt32();
            var height = lcdArray[1].ToInt32();

            var newsImageInfo = dbContextService.Single<NewsImageInfo>(x => x.NewsId == newsId);
            if (newsImageInfo != null)
            {
                if (width > Const.NEWS_HD_RESOLUTION_WIDTH)
                    return newsImageInfo.HDURL;
                return newsImageInfo.NormalURL;
            }
            return string.Empty;
        }
    }
}
