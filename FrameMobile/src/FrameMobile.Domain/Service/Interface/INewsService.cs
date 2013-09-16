﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public interface INewsService
    {
        IList<NewsConfigView> GetConfigList(MobileParam mobileParams);

        IList<NewsSourceView> GetSourceList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsExtraAppView> GetExtraAppList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams, int cver, out int sver);

        IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams);

        IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int newsId, bool action, string categoryIds, int startnum, int num, out int totalCount);
    }
}
