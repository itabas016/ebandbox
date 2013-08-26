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
    public class FetchTouTiaoService
    {
        public TouTiaoParameter GenerateParam()
        {
            var param = new TouTiaoParameter();
            return param;
        }

        public string Request(string category)
        {
            var param = GenerateParam();
            var request_url = ConfigKeys.TYD_NEWS_TOUTIAO_REQUEST_URL.ConfigValue();

            param.Category = category;

            var query_url = string.Format("nonce={0}&category={1}&timestamp={2}&signature={3}&partner={4}",
            param.Nonce, param.Category, param.Timestamp, param.Signature, param.Partner);

            var response = HttpHelper.HttpGet(request_url, query_url);

            return response;
        }

        public TouTiaoResult DeserializeTouTiao(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }
            var instance = JsonConvert.DeserializeObject<TouTiaoResult>(response);
            return instance;
        }

        public List<TouTiaoContent> Anlynaze(TouTiaoResult toutiaoResult)
        {
            if (toutiaoResult == null)
            {
                return null;
            }
            if (toutiaoResult.ret == 0 && toutiaoResult.DataByCursor != null)
            {
                var result = toutiaoResult.DataByCursor.ContentList;
                return result;
            }
            return null;
        }

        public void StorageInstance()
        {
            var categoryList = Enum.GetNames(typeof(TouTiaoCategory));
            foreach (var item in categoryList)
            {
                var response = Request(item);
                var instance = DeserializeTouTiao(response);
            }
        }
    }
}
