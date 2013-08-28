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

        public void add(FrameMobile.Model.News.TouTiaoModel model)
        {
            this.Repository.Add<FrameMobile.Model.News.TouTiaoModel>(model);
        }

        public void Add<T>(T model) where T : IMySQLModel
        {

        }

        public void Delete<T>(T model) where T : IMySQLModel
        {

        }


        public void Update<T>(T model) where T : IMySQLModel
        {

        }

        public IList<T> Find<T>(Expression<Func<T, bool>> expression) where T : IMySQLModel
        {
            throw new NotImplementedException();
        }
    }
}
