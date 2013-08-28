using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using StructureMap.Configuration.DSL;

namespace FrameMobile.Domain
{
    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            For<INewsService>().Use<NewsService>();
            For<IDataBaseService>().Use<DataBaseService>();
        }
    }
}
