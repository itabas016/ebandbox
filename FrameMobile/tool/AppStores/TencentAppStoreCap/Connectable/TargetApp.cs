using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;
using TYD.Mobile.Core;
using TYD.Mobile.Infrastructure.AppStore.Models;

namespace TencentAppStoreCap.Connectable
{
    [Serializable]
    public class TargetApp : App
    {
        [QueryOrSortField]
        public string NameLowCase { get; set; }
    }
}
