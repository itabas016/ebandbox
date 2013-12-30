using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model.ThirdPart;
using NCore;
using Newtonsoft.Json;

namespace FrameMobile.Domain.Service
{
    public class TencentService : NewsDbContextService
    {
        #region Prop

        public string NEWS_REQUEST_URL = ConfigKeys.TYD_NEWS_TENCENT_REQUEST_URL.ConfigValue();

        public int NEWS_REQUEST_COUNT = ConfigKeys.TYD_NEWS_TENCENT_REQUEST_COUNT.ConfigValue().ToInt32();

        public string NEWS_TENCENT_APP_KEY = ConfigKeys.TYD_NEWS_TENCENT_APP_KEY.ConfigValue();

        public const string NEWS_SOURCES_NAME = "腾讯新闻";

        public const string NEWS_SOURCES_NAME_LOW_CASE = "tencent";

        public const string NEWS_SOURCES_PKG_NAME = "com.tencent.news";

        #endregion

        #region Ctor

        public TencentService(INewsDbContextService dbContextService)
        {
            this.dbContextService = dbContextService;
        }

        #endregion

        #region Method

        public void Capture()
        {
            try
            {
                var categoryList = Enum.GetNames(typeof(TencentCategory));
                foreach (var item_category in categoryList)
                {
                }
            }
            catch (Exception ex)
            {
                NLogHelper.WriteError(ex.Message);
                NLogHelper.WriteError(ex.StackTrace);
            }
        }

        public string Request(string category)
        {
            var url = string.Format("{0}?child={1}&refer=openapi_for_tianyida&appkey={2}&n={3}", NEWS_REQUEST_URL, category, NEWS_TENCENT_APP_KEY, NEWS_REQUEST_COUNT);

            var response = HttpHelper.GetData(url);
            return response;
        }

        public List<TencentContent> GetContentlist(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            var instance = JsonConvert.DeserializeObject<TencentResult>(response);
            if (instance!= null)
            {
                var result = instance.NewsList;

                return result;
            }
            return null;
        }



        #endregion

    }
}
