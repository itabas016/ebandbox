using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using StructureMap;
using SubSonic.Repository;

namespace FrameMobile.Domain.Service
{
    public class ThemeDbContextService : IThemeDbContextService
    {
        public IRepository Repository
        {
            get
            {
                return new SimpleRepository(ConnectionStrings.THEME_MYSQL_CONNECTSTRING, SimpleRepositoryOptions.None);
            }
            set
            {
                value = this.Repository;
            }
        }

        private IThemeDbContextService _dbContextService;
        public IThemeDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IThemeDbContextService>();
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
