using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public interface INewsService
    {
        string TimeConvert(string timeformat, long stamp);

        IList<NewsConfigView> GetConfigList(MobileParam mobileParams);

        IList<NewsSourceView> GetSourceList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsExtraAppView> GetExtraAppList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams);

        IList<NewsContentView> GetNewsContentList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount);
    }
}
