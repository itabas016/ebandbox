using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;
using StructureMap;

namespace FrameMobile.Domain
{
    public class StructureMapDependencyResolver : StructureMapScope, IDependencyResolver
    {
        private IContainer _container;
        private StructureMapScope _scope;
        public StructureMapDependencyResolver(IContainer container)
            : base(container)
        {
            _container = container;
            _scope = new StructureMapScope(_container);
        }

        public IDependencyScope BeginScope()
        {
            if (_container == null) _container = ObjectFactory.Container;

            if (_scope == null) _scope = new StructureMapScope(_container);

            // check the conainer if is null
            if (_scope.Container == null) _scope.Container = _container;

            return _scope;
        }
    }
}
