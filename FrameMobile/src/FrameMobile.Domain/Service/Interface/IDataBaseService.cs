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
        void Add<T>(T model) where T : IMySQLModel;
        void Delete<T>(T model) where T : IMySQLModel;
        void Update<T>(T model) where T : IMySQLModel;
        IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : IMySQLModel;
    }
}
