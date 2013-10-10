﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.News;

namespace FrameMobile.Domain.Service
{
    public interface INewsUIService
    {
        IList<NewsSource> GetNewsSourceList();
        IList<NewsCategory> GetNewsCategoryList();
        IList<NewsSubCategory> GetNewsSubCategoryList();
        IList<NewsExtraApp> GetNewsExtraAppList();
    }
}
