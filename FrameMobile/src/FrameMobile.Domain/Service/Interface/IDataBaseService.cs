using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using StructureMap;

namespace FrameMobile.Domain.Service
{
    public interface IDataBaseService
    {
        object Add<T>(T model) where T : class, IMySQLModel, new();
        int Delete<T>(T model) where T : class, IMySQLModel, new();
        int Update<T>(T model) where T : class, IMySQLModel, new();
        bool Exists<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
        IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
        T Single<T>(object key) where T : class, IMySQLModel, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new();
    }
}
