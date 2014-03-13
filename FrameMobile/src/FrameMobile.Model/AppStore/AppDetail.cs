using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.AppStore
{
    [Serializable]
    public class AppDetail
    {
        [JsonProperty(PropertyName = "apk_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hot")]
        public string Hot { get; set; }

        [JsonProperty(PropertyName = "brief_introduction")]
        public string BriefIntroduction { get; set; }

        [JsonProperty(PropertyName = "download_num")]
        public string DownloadNumber { get; set; }

        [JsonProperty(PropertyName = "version_code")]
        public string VersionCode { get; set; }

        [JsonProperty(PropertyName = "version_name")]
        public string VersionName { get; set; }

        [JsonProperty(PropertyName = "file_md5")]
        public string FileMD5 { get; set; }

        [JsonProperty(PropertyName = "file_size")]
        public string FileSize { get; set; }

        [JsonProperty(PropertyName = "package_name")]
        public string PackageName { get; set; }

        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "download_url")]
        public string DownloadURL { get; set; }

        [JsonProperty(PropertyName = "local_path")]
        public string LocalPath { get; set; }

        [JsonProperty(PropertyName = "png_icon_id")]
        public string PngIconId { get; set; }

        [JsonProperty(PropertyName = "jpg_icon_id")]
        public string IpgIconId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "mark")]
        public string Mark { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "is_search")]
        public string IsSearch { get; set; }

        [JsonProperty(PropertyName = "company_type")]
        public string CompanyType { get; set; }

        [JsonProperty(PropertyName = "create_time")]
        public string CreateTime { get; set; }

        [JsonProperty(PropertyName = "modify_time")]
        public string ModifyTime { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageURL { get; set; }
    }
}
