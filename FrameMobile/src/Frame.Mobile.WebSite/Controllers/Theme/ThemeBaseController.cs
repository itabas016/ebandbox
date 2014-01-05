using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame.Mobile.WebSite.Controllers
{
    public class ThemeBaseController : MvcControllerBase
    {
        public IWallPaperService WallPaperService
        {
            get
            {
                if (_wallPaperService == null)
                    _wallPaperService = ObjectFactory.GetInstance<IWallPaperService>();

                return _wallPaperService;
            }
            set
            {
                _wallPaperService = value;
            }
        }
        private IWallPaperService _wallPaperService;

    }
}
