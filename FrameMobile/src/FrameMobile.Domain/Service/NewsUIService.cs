using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;

namespace FrameMobile.Domain.Service
{
    public class NewsUIService : NewsServiceBase, INewsUIService
    {
        public IList<NewsSource> GetNewsSourceList()
        {
            var sourcelist = dbContextService.All<NewsSource>().ToList();
            return sourcelist;
        }

        public IList<NewsCategory> GetNewsCategoryList()
        {
            var categorylist = dbContextService.All<NewsCategory>().ToList();
            return categorylist;
        }

        public IList<NewsSubCategory> GetNewsSubCategoryList()
        {
            var subcategorylist = dbContextService.All<NewsSubCategory>().ToList();
            return subcategorylist;
        }

        public IList<NewsExtraApp> GetNewsExtraAppList()
        {
            var extraapplist = dbContextService.All<NewsExtraApp>().ToList();
            return extraapplist;
        }
    }
}
