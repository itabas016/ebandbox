using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap;
using NCore;
using FrameMobile.Model;
using FrameMobile.Domain;
using System.IO;

namespace Frame.Mobile.WebSite.Controllers
{
    public class NewsBaseController : MvcControllerBase
    {
        public INewsService NewsService
        {
            get
            {
                if (_newsService == null)
                    _newsService = ObjectFactory.GetInstance<INewsService>();

                return _newsService;
            }
            set
            {
                _newsService = value;
            }
        }
        private INewsService _newsService;

        private INewsUIService _newsUIService;
        public INewsUIService NewsUIService
        {
            get
            {
                if (_newsUIService == null)
                {
                    _newsUIService = ObjectFactory.GetInstance<INewsUIService>();
                }
                return _newsUIService;
            }
            set
            {
                _newsUIService = value;
            }
        }

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

        private INewsDbContextService _dbContextService;
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        public const int pageSize = 20;

        public static string NEWS_RESOURCES_DIR_ROOT = ConfigKeys.TYD_NEWS_RESOURCES_DIR_ROOT.ConfigValue();

        public string NEWS_DEST_HD_IMAGE_DIR_BASE = string.Format("{0}\\Images\\720\\", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_DEST_NORMAL_IMAGE_DIR_BASE = string.Format("{0}\\Images\\480\\", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_RADAR_LOGOS_IMAGE_PREFIX = ConfigKeys.TYD_NEWS_RADAR_LOGO_IMAGE_PREFIX.ConfigValue();

        protected string GetRadarCategoryLogoFilePath<T>(T model, HttpPostedFileBase logoFile) where T : MySQLModel
        {
            if (logoFile != null && !string.IsNullOrWhiteSpace(logoFile.FileName))
            {
                var logoFilePath = SaveNewsResourceFile(Const.NEWS_RADAR_LOGOS_FOLDER_NAME, ResourcesFilePathHelper.NewsRadarLogoPath, logoFile, string.Format("{0}_{1}", Guid.NewGuid().ToString(), Path.GetExtension(logoFile.FileName)).NormalzieFileName());

                return logoFilePath;
            }
            return string.Empty;
        }

    }
}
