using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public interface INewsService
    {
        IList<NewsConfigView> GetConfigViewList(MobileParam mobileParams);

        IList<NewsSourceView> GetSourceViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsInfAddressView> GetInfAddressViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams);

        IList<NewsContentView> GetNewsContentViewList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount);
    }
}
