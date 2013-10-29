using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using FrameMobile.Model.Account;
using FrameMobile.Model.News;

namespace FrameMobile.Domain.Service
{
    public class CleanService : NewsServiceBase
    {
        public void CleanNewsContent(int days)
        {
            var endDate = DateTime.Now.AddDays(-days);

            var contentlist = dbContextService.Find<NewsContent>(x => x.PublishTime <= endDate);

            if (contentlist != null && contentlist.Count > 0)
            {
                var ret = dbContextService.Delete<NewsContent>(contentlist);

                NLogHelper.WriteTrace(string.Format("Delete NewsContent {0} rows end time {1}.", contentlist.Count, endDate));
            }
            else
            {
                NLogHelper.WriteTrace("No rows to clean!");
            }
        }

        public void CleanAccountInvitationCode()
        {
            var ret = dbContextService.Delete<InvitationCode>(x => x.Status == 0);
            if (ret > 0)
            {
                NLogHelper.WriteTrace(string.Format("Delete Account InvitationCode for {0} rows", ret));

            }
        }
    }
}
