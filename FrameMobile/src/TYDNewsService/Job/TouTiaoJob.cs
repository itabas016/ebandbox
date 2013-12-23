using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using Quartz;

namespace TYDNewsService
{
    public class TouTiaoJob : JobBase, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            TouTiaoService service = new TouTiaoService(dbContextService);

            service.Capture();
        }
    }
}
