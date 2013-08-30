using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using System.Runtime.InteropServices;
using System.Linq.Expressions;

namespace FrameMobile.Domain.Service
{
    public interface IDataBaseService
    {
        void Add<T>(T model) where T : class, IMySQLModel, new();
        void Delete<T>(T model) where T : class, IMySQLModel, new();
        void Update<T>(T model) where T : class, IMySQLModel, new();
        bool Exists<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
        IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
        T Single<T>(object key) where T : class, IMySQLModel, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
    }
}
