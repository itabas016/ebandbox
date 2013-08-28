using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using SubSonic.Repository;

namespace FrameMobile.Domain.Service
{
    public class DataBaseService : IDataBaseService
    {
        public IRepository Repository { get; set; }

        public DataBaseService()
        {
            this.Repository = new SimpleRepository(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING, SimpleRepositoryOptions.None);
        }

        public void Add<T>(T model) where T : class, IMySQLModel, new()
        {
            this.Repository.Add<T>(model);
        }

        public void Delete<T>(T model) where T : class, IMySQLModel, new()
        {
            this.Repository.Delete<T>(model.Id);
        }


        public void Update<T>(T model) where T : class, IMySQLModel, new()
        {
            this.Repository.Update<T>(model);
        }

        public IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : class, IMySQLModel, new()
        {
            throw new NotImplementedException();
        }
    }
}
