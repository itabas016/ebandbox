using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public interface INewsService
    {
        IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams);

        IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams);

        IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int categoryId, int startnum, int num, out int totalCount);
    }
}
