using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace FrameMobile.Domain.Service
{
    public class NewsServiceBase
    {
        private IDbContextService _dbContextService;
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
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
