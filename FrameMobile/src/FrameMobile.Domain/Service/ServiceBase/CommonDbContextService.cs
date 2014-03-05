using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using StructureMap;
using SubSonic.Repository;

namespace FrameMobile.Domain.Service
{
    public class CommonDbContextService : ICommonDbContextService
    {
        public IRepository Repository
        {
            get
            {
                return new SimpleRepository(ConnectionStrings.COMMON_MYSQL_CONNECTSTRING, SimpleRepositoryOptions.None);
            }
            set
            {
                value = this.Repository;
            }
        }

        private ICommonDbContextService _dbContextService;
        public ICommonDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<ICommonDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }
    }
}
