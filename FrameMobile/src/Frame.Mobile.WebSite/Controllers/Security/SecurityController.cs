using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Web;
using StructureMap;
using FrameMobile.Core;

namespace Frame.Mobile.WebSite.Controllers
{
    public class SecurityController : MvcControllerBase
    {
        private ISecurityService _securityService;
        public ISecurityService securityService
        {
            get
            {
                if (_securityService == null)
                {
                    _securityService = ObjectFactory.GetInstance<ISecurityService>();
                }
                return _securityService;
            }
            set
            {
                _securityService = value;
            }
        }

        public ActionResult Config(string target, string ch = "", int ver = 0, bool enc = true)
        {
            var config = securityService.GetConfig(target);
            Func<FloatingView> getfloatingconfig = () => securityService.GetFloatingConfig(config, ch, ver);

            var actionResult = BuildResult<FloatingView>(getfloatingconfig);

            var result = enc ? (actionResult.ToString()).Base64Encode() : actionResult.ToString();
            return Content(result);
        }
    }
}
