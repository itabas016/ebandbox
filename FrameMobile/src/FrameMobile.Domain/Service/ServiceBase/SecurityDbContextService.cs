using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using StructureMap;
using SubSonic.Repository;

namespace FrameMobile.Domain.Service
{
    public class SecurityDbContextService : ISecurityDbContextService
    {
        public IRepository Repository
        {
            get
            {
                return new SimpleRepository(ConnectionStrings.SECURITY_MYSQL_CONNECTSTRING, SimpleRepositoryOptions.None);
            }
            set
            {
                value = this.Repository;
            }
        }

        private ISecurityDbContextService _dbContextService;
        public ISecurityDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<ISecurityDbContextService>();
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
