using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Common;
using Snap;
using Snap.StructureMap;
using StructureMap;
using NCore;

namespace FrameMobile.Domain
{
    public class Bootstrapper
    {
        public static bool _hasBeenIntialized = false;

        public static void Start()
        {
            if (!_hasBeenIntialized)
            {
                EntityMapping.Config();

                DBModelstrapper.Initialize();

                ConfigueInjection();

                _hasBeenIntialized = true;
            }
        }

        public static void ConfigueInjection()
        {
            DependencyResolver.SetResolver(
                t =>
                {
                    try { return ObjectFactory.GetInstance(t); }
                    catch { return null; }
                },
                t => ObjectFactory.GetAllInstances<object>().Where(s => s.GetType() == t));

            if (ConfigKeys.ENABLE_SNAP.ConfigValue().ToBoolean())
            {
                SnapConfiguration.For<StructureMapAspectContainer>(c =>
                {
                    c.IncludeNamespace("FrameMobile.Domain.Service*");
                    c.Bind<ServiceCacheInterceptor>().To<ServiceCacheAttribute>();
                });
            }

            ObjectFactory.Configure(x => x.AddRegistry(new ControllerRegistry()));
        }
    }
}
