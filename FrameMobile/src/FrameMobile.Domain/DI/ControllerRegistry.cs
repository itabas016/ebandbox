using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            For<IRequestRepository>().Use<RequestRepository>();
            For<IDataBaseService>().Use<DataBaseService>();

            //For<INewsService>().Use<NewsService>();
            For<INewsService>().Use<NewsFakeService>();

        }
    }
}
