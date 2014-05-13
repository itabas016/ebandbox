using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Security;

namespace FrameMobile.Domain.Service
{
    public class SecurityService : SecurityDbContextService, ISecurityService
    {
        public SecurityConfig GetConfig(string target)
        {
            var config = dbContextService.Single<SecurityConfig>(x => x.Type == target);
            return config;
        }

        public FloatingView GetFloatingConfig(SecurityConfig config, string channel, int version)
        {
            if (config != null)
            {
                if (version >= config.LatestVersion)
                {
                    return null;
                }
                else
                {
                    var floatingconfig = dbContextService.Single<FloatingModel>(x => x.Version == config.LatestVersion && x.Status == 1);
                    var floatingView = new FloatingView()
                    {
                        JsonResult = floatingconfig.JsonResult,
                        Version = config.LatestVersion,
                        Rate = config.Rate
                    };
                    return floatingView;
                }
            }
            return null;
        }
    }
}
