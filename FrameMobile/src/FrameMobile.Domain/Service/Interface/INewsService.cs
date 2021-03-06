﻿using System;
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

        IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver, out int ratio);

        IList<OlderNewsExtraAppView> GetOlderExtraAppViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsInfAddressView> GetInfAddressViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams);

        IList<NewsRadarView> GetNewsRadarViewList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsContentView> GetNewsContentViewList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount);

        IList<NewsContentView> GetContentViewList(MobileParam mobileParams, List<int> categoryIds, long stamp, bool action);

        NewsCollectionView GetNewsCollectionView(MobileParam mobileParams, long stamp, int extracver, bool action, string categoryIds, int startnum, int num, out int extrasver, out int ratio, out int totalCount);

    }
}
