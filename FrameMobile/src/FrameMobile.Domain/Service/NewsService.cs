using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public class NewsService : INewsService
    {
        [ServiceCache]
        public IList<NewsSourceView> GetSourceList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        [ServiceCache]
        public IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int categoryId, int startnum, int num, out int totalCount)
        {
            throw new NotImplementedException();
        }
    }
}
