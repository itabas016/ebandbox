﻿using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Domain;
using System.IO;
using FrameMobile.Model;
using FrameMobile.Model.Theme;

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

        public IWallPaperUIService WallPaperUIService
        {
            get
            {
                if (_wallPaperUIService == null)
                    _wallPaperUIService = ObjectFactory.GetInstance<IWallPaperUIService>();

                return _wallPaperUIService;
            }
            set
            {
                _wallPaperUIService = value;
            }
        }
        private IWallPaperUIService _wallPaperUIService;

        private IThemeDbContextService _dbContextService;
        public IThemeDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IThemeDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        protected string GetThemeLogoFilePath<T>(T model, HttpPostedFileBase logoFile) where T : MySQLModel
        {
            if (logoFile != null && !string.IsNullOrWhiteSpace(logoFile.FileName))
            {
                var logoFilePath = SaveResourceFile(Const.THEME_LOGOS_FOLDER_NAME, ResourcesFilePathHelper.ThemeLogoPath, logoFile, string.Format("{0}_{1}_{2}", model.Name, Guid.NewGuid().ToString(), Path.GetExtension(logoFile.FileName)).NormalzieFileName());

                return logoFilePath;
            }
            return string.Empty;
        }

        protected string GetThemeThumbnailFilePath(WallPaper model, HttpPostedFileBase thumbnailFile)
        {
            if (thumbnailFile != null && !string.IsNullOrWhiteSpace(thumbnailFile.FileName))
            {
                var thumbnailFilePath = SaveResourceFile(Const.THEME_THUMBNAILS_FOLDER_NAME, ResourcesFilePathHelper.ThemeLogoPath, thumbnailFile, string.Format("{0}_{1}_{2}", model.Titile, Guid.NewGuid().ToString(), Path.GetExtension(thumbnailFile.FileName)).NormalzieFileName());

                return thumbnailFilePath;
            }
            return string.Empty;
        }

        protected string GetThemeOriginalFilePath(WallPaper model, HttpPostedFileBase originailFile)
        {
            if (originailFile != null && !string.IsNullOrWhiteSpace(originailFile.FileName))
            {
                var originailFilePath = SaveResourceFile(Const.THEME_ORIGINALS_FOLDER_NAME, ResourcesFilePathHelper.ThemeLogoPath, originailFile, string.Format("{0}_{1}_{2}", model.Titile, Guid.NewGuid().ToString(), Path.GetExtension(originailFile.FileName)).NormalzieFileName());

                return originailFilePath;
            }
            return string.Empty;
        }

    }
}
