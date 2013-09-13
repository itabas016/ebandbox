using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;
using FrameMobile.Core;
using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap.Configuration.DSL;

namespace FrameMobile.Domain
{
    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            RegistyCommonService();

            RegistyNewsService();
        }

        private void RegistyCommonService()
        {
            For<IRequestRepository>().Use<RequestRepository>();
            For<IDbContextService>().Use<DbContextService>();
            For<ICacheManagerHelper>().Use<RedisCacheHelper>();
        }

        private void RegistyNewsService()
        {
#if DEBUG
            For<INewsService>().Use<NewsFakeService>();
#else
            For<INewsService>().Use<NewsService>();
#endif
        }
    }
}
