using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QihooAppStoreCap.Invocation;
using QihooAppStoreCap.Model;
using QihooAppStoreCap.Service;

namespace QihooAppStoreCap
{
    public class AppItemCap
    {
        private GetApps _app;
        private DataConvertService _service;

        public AppItemCap()
        {
            _app = new GetApps();

            _service = new DataConvertService();
        }

        public void Request()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var startTime = DateTime.Now.AddDays(-1).UnixStamp().ToString();

            parameters["startTime"] = startTime;

            int total = 0;

            var applist = GetTotalAppItem(parameters, out total);

            var apppagelist = GetAllAppItem(parameters,total);

            applist = applist.Union(apppagelist).ToList();

        }

        public List<QihooAppStoreApp> GetTotalAppItem(Dictionary<string, string> parameters, out int total)
        {
            var result = new List<QihooAppStoreApp>();
            total = 0;

            var data = _app.GetData(parameters);

            var appResult = _service.DeserializeBase(data);

            if (appResult != null)
            {
                result = appResult.QihooApplist;
                total = appResult.Total;
            }

            return result;
        }

        public List<QihooAppStoreApp> GetAllAppItem(Dictionary<string, string> parameters, int total)
        {
            var result = new List<QihooAppStoreApp>();

            var page = total / 100;
            if (page > 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    parameters["start"] = (i * 100 + 1).ToString();

                    var data = _app.GetData(parameters);

                    var applist = _service.DeserializeAppItem(data);

                    if (applist != null && applist.Count > 0)
                    {
                        result = result.Union(applist).ToList();
                    }
                }
            }
            return result;
        }
    }
}
