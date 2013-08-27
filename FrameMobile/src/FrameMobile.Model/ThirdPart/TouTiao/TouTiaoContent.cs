using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TouTiaoContent
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        //同Id
        [JsonProperty(PropertyName = "group_id")]
        public long GroupId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        //新闻来源
        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; }

        //新闻来源网址
        [JsonProperty(PropertyName = "site_url")]
        public string SiteURL { get; set; }

        //新闻发表时间 时间戳
        [JsonProperty(PropertyName = "publish_time")]
        public float PublishTime { get; set; }

        //更新时间
        [JsonProperty(PropertyName = "modify_time")]
        public float ModifyTime { get; set; }

        //简介
        [JsonProperty(PropertyName = "abstract")]
        public string Abstract { get; set; }

        //新闻头条网址
        [JsonProperty(PropertyName = "toutiao_url")]
        public string TouTiaoURL { get; set; }

        //新闻头条WAP网址
        [JsonProperty(PropertyName = "toutiao_wap_url")]
        public string TouTiaoWAPURL { get; set; }

        //应用详情网址
        [JsonProperty(PropertyName = "app_open_url")]
        public string AppOpeURL { get; set; }

        //热荐状态
        //0, 无额外信息 默认
        //1 代表 热
        //10 代表 推荐
        //11 代表 推荐 + 热 ( 1 | 2)
        [JsonProperty(PropertyName = "tip")]
        public int Tip { get; set; }

        //顶的数量
        [JsonProperty(PropertyName = "digg_count")]
        public int DiggCount { get; set; }

        //踩的数量
        [JsonProperty(PropertyName = "bury_count")]
        public int BuryCount { get; set; }

        //收藏的数量
        [JsonProperty(PropertyName = "favorite_count")]
        public int FavoriteCount { get; set; }

        //评论的数量
        [JsonProperty(PropertyName = "comment_count")]
        public int CommentCount { get; set; }

        //图片集合
        [JsonProperty(PropertyName = "images")]
        public List<TouTiaoImageInfo> ImageList { get; set; }
    }
}
