using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using Quartz;
using StructureMap;

namespace TYDNewsService
{
    public class TouTiaoJob : IJob
    {
        public IDataBaseService dataBaseService
        {
            get
            {
                if (_dataBaseService == null)
                {
                    _dataBaseService = ObjectFactory.GetInstance<IDataBaseService>();
                }
                return _dataBaseService;
            }
        }
        private IDataBaseService _dataBaseService;

        public void Execute(JobExecutionContext context)
        {
            FetchTouTiaoService service = new FetchTouTiaoService(dataBaseService);

            service.Capture();
        }
    }
}
