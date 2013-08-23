using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Cache
{
    public interface ICacheManagerHelper
    {
        void Add(string key, object value);
        void Add(string key, object value, int timeoutSecs);
        void AddNullableData(string key, object value, int timeoutSecs);
        void Add(string key, object value, DateTime? absoluteExpiration);
        void Add(string key, object value, TimeSpan? slidingExpiration);
        void Remove(string key);
        bool Contains(string key);
        int Count { get; }
        long Increment(string countID);
        void Flush();
        void ClearServiceCache();
        T GetData<T>(string key);
        object GetData(string key, Type dataType);
        object GetNullableData(string key, Type dataType);
    }
}
