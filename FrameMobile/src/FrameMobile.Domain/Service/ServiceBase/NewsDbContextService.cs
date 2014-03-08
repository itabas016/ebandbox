using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using StructureMap;
using SubSonic.Repository;

namespace FrameMobile.Domain.Service
{
    public class NewsDbContextService : INewsDbContextService
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

        private INewsDbContextService _dbContextService;
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        private ICommonDbContextService _dbCommonContextService;
        public ICommonDbContextService dbCommonContextService
        {
            get
            {
                if (_dbCommonContextService == null)
                {
                    _dbCommonContextService = ObjectFactory.GetInstance<ICommonDbContextService>();
                }
                return _dbCommonContextService;
            }
            set
            {
                _dbCommonContextService = value;
            }
        }

        public IMobileUIService MobileUIService
        {
            get
            {
                if (_mobileUIService == null)
                    _mobileUIService = ObjectFactory.GetInstance<IMobileUIService>();

                return _mobileUIService;
            }
            set
            {
                _mobileUIService = value;
            }
        }
        private IMobileUIService _mobileUIService;
    }
}
