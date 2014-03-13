using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using NCore;
using FrameMobile.Domain.Service;
using FrameMobile.Common;
using FrameMobile.Core;

namespace TYDNewsService
{
    public class UpdateNewsExtraAppJob : NewsJobBase, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var _commonService = new CommonService();

            try
            {
                _commonService.UpdateNewsExtraApp();
            }
            catch (Exception ex)
            {
                NLogHelper.WriteError(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace.ToString()));
            }
        }
    }
}
