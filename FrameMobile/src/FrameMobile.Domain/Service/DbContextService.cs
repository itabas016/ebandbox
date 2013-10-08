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
    public class DbContextService : IDbContextService
    {
        public IRepository Repository
        {
            get
            {
                 return new SimpleRepository(ConnectionStrings.NEWS_MYSQL_CONNECTSTRING, SimpleRepositoryOptions.None);
            }
            set
            {
                value = this.Repository;
            }
        }
    }
}
