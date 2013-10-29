using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using StructureMap;

namespace TYDNewsService
{
    public class JobBase
    {
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
        }
        private IDbContextService _dbContextService;

    }
}
