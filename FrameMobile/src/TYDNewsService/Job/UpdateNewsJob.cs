using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Domain.Service;
using Quartz;
using NCore;

namespace TYDNewsService
{
    public class UpdateNewsJob : NewsJobBase, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var _commonService = new CommonService();

            var startTime = DateTime.Now.AddHours(-ConfigKeys.UPDATE_NEWS_START_PUBLISH_TIME_HOUR.ConfigValue().ToDouble());
            var endTime = DateTime.Now;

            _commonService.UpdateNews(startTime, endTime);
        }
    }
}
