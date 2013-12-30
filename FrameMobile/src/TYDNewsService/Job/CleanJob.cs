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
    public class CleanJob : NewsJobBase, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            CleanService service = new CleanService();

            var days = ConfigKeys.CLEANUP_NEWS_CONTENT_DAYS_AGO_VALUE.ConfigValue().ToInt32();

            service.CleanNewsContent(days);
            service.CleanAccountInvitationCode();
        }
    }
}
