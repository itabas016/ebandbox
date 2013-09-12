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
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
                }
                return _dbContextService;
            }
        }
        private IDbContextService _dbContextService;

        public void Execute(JobExecutionContext context)
        {
            FetchTouTiaoService service = new FetchTouTiaoService(dbContextService);

            service.Capture();
        }
    }
}
