using SubSonic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Domain
{
    public interface IDbContext
    {
        IRepository Repository { get; set; }
    }
}
