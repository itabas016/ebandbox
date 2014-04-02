using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;

namespace TencentAppStoreModel.TydAppStore
{
    public class AppListResult
    {
        public int result { get; set; }
        public string desc { get; set; }
        public int count { get; set; }
        public string host { get; set; }
        public int total { get; set; }
        public List<ApplistItemView> data { get; set; }
    }
}
