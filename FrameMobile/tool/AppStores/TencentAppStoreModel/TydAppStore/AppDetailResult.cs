using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TYD.Mobile.Infrastructure.Models;
using TYD.Mobile.Core;
using Newtonsoft.Json;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;

namespace TencentAppStoreModel.TydAppStore
{
    public class AppDetailResult
    {
        public int result { get; set; }
        public string desc { get; set; }
        public string host { get; set; }
        public AppView data { get; set; }
    }
    public enum PlatformType
    {
        Java = 1,
        Lua = 2,
        C = 4,
        Txt = 3,
        Android = 8
    }

    public enum SectionViewType
    {
        FirstScreen = 1,
        Recommend = 2,
        Game = 3,
        Application = 4,
        Book = 5,
        BASE_Lib = 1001,
        BASE_Management = 1002,

        /// <summary>
        /// 游戏中心--精品推荐
        /// </summary>
        Game_Recommend = 10001,
    }

    public class AppView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("Id")]
        public int Id { get; set; }

        [ViewField(DisplayName = "summary")]
        [JsonProperty("Summary")]
        public string Summary { get; set; }

        [ViewField(DisplayName = "imgcount")]
        [JsonProperty("ImageCount")]
        public int ImageCount { get; set; }

        [ViewField(DisplayName = "imglist")]
        [JsonProperty("ImageList")]
        public List<string> ImageList { get; set; }

        [JsonProperty]
        public List<string> ImageUrls { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonIgnore]
        public List<string> ScreenShots { get; set; }

        [ViewField(DisplayName = "publishtime")]
        [JsonProperty("PublishTime")]
        public DateTime PublishTime { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonIgnore]
        public string DownloadUrl { get; set; }

        [ViewField(DisplayName = "storagename")]
        [JsonProperty("StorageName")]
        public string StorageName { get; set; }

        [ViewField(DisplayName = "drive")]
        [JsonProperty("Drive")]
        public string Drive { get; set; }

        [ViewField(DisplayName = "ishide")]
        [JsonProperty("IsHide")]
        public int IsHide { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonIgnore]
        public SectionViewType Section { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("Version")]
        public string Version { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("VersionName")]
        public string VersionName { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonIgnore]
        public string LogoFileName { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("AppNo")]
        public string AppNo { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonIgnore]
        public PlatformType PlatformType { get; set; }

        [JsonProperty("activity")]
        [ViewField(IsDisplay = false)]
        public string Activity { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("MinSDKVersion")]
        public string MinSDKVersion { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("Company")]
        public string Company { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("EnglishName")]
        public string EnglishName { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("Category")]
        public string Category { get; set; }




        [JsonProperty("download")]
        [ViewField(IsDisplay = false)]
        public int DownloadTimes { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("size")]
        public int FileSize { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("ApkName")]
        public string ApkName { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("review")]
        public int ReviewCount { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("rate")]
        public string Rate { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("icon")]
        public List<AppLogo> Icon { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("MD5")]
        public string MD5 { get; set; }

        #region For the manual
        public static List<AppView> Repo;

        public static List<AppView> GameRepo;

        #endregion
    }
}
