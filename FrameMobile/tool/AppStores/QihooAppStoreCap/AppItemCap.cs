using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QihooAppStoreCap.Invocation;

namespace QihooAppStoreCap
{
    public class AppItemCap
    {
        public void Request()
        {
            var app = new GetApps();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters["start"] = "0";

            var data = app.GetData(parameters);


        }
    }
}
