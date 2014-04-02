using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TencentAppStoreCap.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            AppInfoCollector appInfoCollector = new AppInfoCollector();

            appInfoCollector.PerformFullAppCollect();
        }
    }
}
