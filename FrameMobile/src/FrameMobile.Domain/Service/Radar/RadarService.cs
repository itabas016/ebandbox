using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.News;
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain.Service
{
    public class RadarService : CommonDbContextService, IRadarService
    {
        public IList<RadarCategory> GetRadarCategoryList()
        {
            var radarlist = dbContextService.All<RadarCategory>().ToList();
            return radarlist;
        }

        public void UpdateServerVersion<T>() where T : MySQLModelBase
        {
            try
            {
                var type = typeof(T);
                var flag = (type == typeof(RadarCategory) || type == typeof(RadarElement));
                var configName = flag ? Const.NEWS_RADAR_CONFIG_TABLE_NAME : typeof(T).Name.ToLower();

                var config = dbContextService.Single<NewsConfig>(x => x.NameLowCase == configName);
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
    }
}
