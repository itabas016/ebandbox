using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.Common;
using FrameMobile.Model.News;
using StructureMap;
using NCore;

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

        public void UpdateServerVersion<T>() where T : MySQLModelBase
        {
            try
            {
                var config = dbContextService.Single<NewsConfig>(x => x.NameLowCase == typeof(T).Name.ToLower());
                if (config == null)
                {
                    return;
                }
                config.Version++;
                dbContextService.Update<NewsConfig>(config);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TimeStamp TimeConvert(string utcTimeString, string unixStamp)
        {
            var instance = new TimeStamp();

            instance.InputTime = string.IsNullOrEmpty(utcTimeString) ?
                DateTime.Now : utcTimeString.ToExactDateTime("yyyy-MM-dd HH:mm:ss");

            instance.InputStamp = string.IsNullOrEmpty(unixStamp) ?
                0 : unixStamp.ToInt32();

            instance.OutputStamp = instance.InputTime.UnixStamp();
            instance.OutputTime = instance.InputStamp.UTCStamp();

            return instance;
        }
    }
}
