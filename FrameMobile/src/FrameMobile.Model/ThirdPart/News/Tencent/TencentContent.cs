using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TencentContent
    {
        //新闻Id
        [JsonProperty(PropertyName = "id")]
        public string NewsId { get; set; }

        //新闻来源
        [JsonProperty(PropertyName = "from")]
        public string Source { get; set; }

        //请求地址
        [JsonProperty(PropertyName = "qqnews_download_url")]
        public string RequestUrl { get; set; }

        //新闻类型
        [JsonProperty(PropertyName = "articletype")]
        public int ArticleType { get; set; }

        //标题
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        //
        [JsonProperty(PropertyName = "src")]
        public string Src { get; set; }

        //新闻缩略图
        [JsonProperty(PropertyName = "thumbnails_qqnews")]
        public TencentThumbnails ThumbnailList { get; set; }

        //时间戳
        [JsonProperty(PropertyName = "timestamp")]
        public long Stamp { get; set; }

        //站点地址
        [JsonProperty(PropertyName = "url")]
        public string SiteUrl { get; set; }

        //摘要
        [JsonProperty(PropertyName = "abstract")]
        public string Summary { get; set; }

        //新闻段
        [JsonProperty(PropertyName = "content")]
        public List<TencentContentSection> SectionList { get; set; }
    }
}
