using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.News;

namespace FrameMobile.Domain.Service
{
    public interface INewsServiceHelper
    {
        IList<NewsExtraApp> GetNewsExtraAppList();
        int GetImageURLTypeByResolution(MobileParam mobileParams);
        int GetExtraRatioByChannel(MobileParam mobileParams);
        IEnumerable<NewsContentView> GetLocalContentViewList(List<NewsExtraApp> extraAppList, int imageType);
        IEnumerable<NewsContentView> GetOldestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime endDateTime, DateTime stampTime);
        IEnumerable<NewsContentView> GetLatestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime stampTime);
        NewsExtraResult GetNewsExtraResult(MobileParam mobileParams, int extracver, out int extrasver, out int ratio);
        NewsContentResult GetNewsContentResult(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount);
    }
}
