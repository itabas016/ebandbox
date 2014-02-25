using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Web;
using StructureMap;
using NCore;

namespace Frame.Mobile.WebSite.Controllers
{
    public class CommonBaseController : MvcControllerBase
    {
        private ICommonDbContextService _dbContextService;
        public ICommonDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<ICommonDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        public IMobileUIService MobileUIService
        {
            get
            {
                if (_mobileUIService == null)
                    _mobileUIService = ObjectFactory.GetInstance<IMobileUIService>();

                return _mobileUIService;
            }
            set
            {
                _mobileUIService = value;
            }
        }
        private IMobileUIService _mobileUIService;

        private IRadarService _radarService;
        public IRadarService RadarService
        {
            get
            {
                if (_radarService == null)
                {
                    _radarService = ObjectFactory.GetInstance<IRadarService>();
                }
                return _radarService;
            }
            set
            {
                _radarService = value;
            }
        }

        private IAccountService _accountService;
        public IAccountService AccountService
        {
            get
            {
                if (_accountService == null)
                {
                    _accountService = ObjectFactory.GetInstance<IAccountService>();
                }
                return _accountService;
            }
            set
            {
                _accountService = value;
            }
        }

        private ICookieService _cookieService;
        public ICookieService CookieService
        {
            get
            {
                if (_cookieService == null)
                {
                    _cookieService = ObjectFactory.GetInstance<ICookieService>();
                }
                return _cookieService;
            }
            set
            {
                _cookieService = value;
            }
        }

        public const int CookieTimeoutSeconds = 1209600;

        public const int pageSize = 20;

        public string NEWS_LOGOS_IMAGE_PREFIX = ConfigKeys.TYD_NEWS_LOGO_IMAGE_PREFIX.ConfigValue();

        protected string GetNewsLogoFilePath<T>(T model, HttpPostedFileBase logoFile) where T : MySQLModel
        {
            if (logoFile != null && !string.IsNullOrWhiteSpace(logoFile.FileName))
            {
                var logoFilePath = SaveNewsResourceFile(Const.NEWS_LOGOS_FOLDER_NAME, ResourcesFilePathHelper.NewsLogoPath, logoFile, string.Format("{0}_{1}", Guid.NewGuid().ToString(), Path.GetExtension(logoFile.FileName)).NormalzieFileName());

                return logoFilePath;
            }
            return string.Empty;
        }
    }
}
