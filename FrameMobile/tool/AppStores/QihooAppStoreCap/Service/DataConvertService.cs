using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using QihooAppStoreCap.Model;

namespace QihooAppStoreCap.Service
{
    public class DataConvertService
    {
        public QihooAppStoreGetAppResult DeserializeBase(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var getAppResult = JsonConvert.DeserializeObject<QihooAppStoreGetAppResult>(content);

                return getAppResult;
            }
            return null;
        }

        public List<QihooAppStoreApp> DeserializeAppItem(QihooAppStoreGetAppResult result)
        {
            if (result != null && result.QihooApplist.Count > 0)
            {
                return result.QihooApplist;
            }
            return null;
        }

        public List<QihooAppStoreApp> DeserializeAppItem(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var getAppResult = JsonConvert.DeserializeObject<QihooAppStoreGetAppResult>(content);

                if (getAppResult != null && getAppResult.QihooApplist.Count > 0)
                {
                    return getAppResult.QihooApplist;
                }
            }
            return null;
        }

        public List<QihooAppStoreCategory> DeserializeCategory(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var getAppResult = JsonConvert.DeserializeObject<QihooAppStoreGetCategoryResult>(content);

                if (getAppResult != null && getAppResult.QihooCategorylist.Count > 0)
                {
                    return getAppResult.QihooCategorylist;
                }
            }
            return null;
        }
    }
}
