using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AppItemCap cap = new AppItemCap();

                cap.AppItemCompleteCap();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
            }
            
        }
    }
}
