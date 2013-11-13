using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model.News;
using NCore;

namespace FrameMobile.Domain.Service
{
    public class CommonService : NewsServiceBase
    {
        public void UpdateNews(DateTime startTime, DateTime endTime)
        {
            var contentlist = new List<NewsContent>();
            var limit = ConfigKeys.UPDATE_NEWS_SAME_PUBLISH_TIME_LIMIT.ConfigValue().ToInt32();

            var dupPublishTimeList = (from l in
                                          dbContextService.Find<NewsContent>(x => x.PublishTime >= startTime && x.PublishTime <= endTime)
                                      group l by l.PublishTime into gp
                                      where gp.Count() >= limit
                                      select gp.Key);
            if (dupPublishTimeList != null && dupPublishTimeList.Count() > 0)
            {
                foreach (var item in dupPublishTimeList)
                {
                    var retlist = from l in
                                      dbContextService.Find<NewsContent>(x => x.PublishTime == item)
                                  select l;
                    contentlist = contentlist.Union(retlist).ToList();
                }
            }

            if (contentlist != null && contentlist.Count > 0)
            {
                foreach (var item in contentlist)
                {
                    var random = new Random(Guid.NewGuid().GetHashCode());

                    item.PublishTime = item.PublishTime.AddSeconds(random.Next(1, 60 * 60));
                }

                var ret = dbContextService.Update<NewsContent>(contentlist);

                NLogHelper.WriteTrace(string.Format("Update NewsContent {0} rows from {1} to {2}.", ret, startTime, endTime));
            }
        }
    }
}
