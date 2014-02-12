using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.News;
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain.Service
{
    public class RadarService : NewsDbContextService, IRadarService
    {
        public IList<Radar> GetRadarList()
        {
            var radarlist = dbContextService.All<Radar>().ToList();
            return radarlist;
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
    }
}
