﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            AppItemCap cap = new AppItemCap();

            NewAppItemCap newcap = new NewAppItemCap();

            //cap.AppItemCompleteCap();
            newcap.NewAppItemCompleteCap();
        }
    }
}
