using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;
using FrameMobile.Model.Theme;
using StructureMap;

namespace FrameMobile.Domain.Service
{
    public class WallPaperUIService : ThemeDbContextService, IWallPaperUIService
    {
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

        public IList<WallPaper> GetWallPaperList(string searchKey)
        {
            var wallpaperlist = dbContextService.Find<WallPaper>(x=>x.Title.Contains(searchKey)).ToList();
            return wallpaperlist;
        }

        public IList<WallPaperCategory> GetWallPaperCategoryList()
        {
            var categorylist = dbContextService.All<WallPaperCategory>().ToList();
            return categorylist;
        }

        public IList<WallPaperSubCategory> GetWallPaperSubCategoryList()
        {
            var subcategorylist = dbContextService.All<WallPaperSubCategory>().ToList();
            return subcategorylist;
        }

        public IList<WallPaperTopic> GetWallPaperTopicList()
        {
            var topiclist = dbContextService.All<WallPaperTopic>().ToList();
            return topiclist;
        }

        public void UpdateServerVersion<T>() where T : Model.MySQLModelBase
        {
            try
            {
                var config = dbContextService.Single<ThemeConfig>(x => x.NameLowCase == typeof(T).Name.ToLower());
                if (config == null)
                {
                    return;
                }
                config.Version++;
                dbContextService.Update<ThemeConfig>(config);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type)
        {
            var configlist = dbContextService.Find<ThemeConfig>(x => x.Status == 1 && x.Type == type);
            return configlist.To<IList<ThemeConfigView>>();
        }

        public IList<WallPaperRelateCategory> GetWallRelateCategoryList(int wallpaperId)
        {
            var relatecategorylist = dbContextService.Find<WallPaperRelateCategory>(x => x.WallPaperId == wallpaperId && x.Status == 1).ToList();
            return relatecategorylist;
        }

        public IList<WallPaperRelateSubCategory> GetWallRelateSubCategoryList(int wallpaperId)
        {
            var relatecategorylist = dbContextService.Find<WallPaperRelateSubCategory>(x => x.WallPaperId == wallpaperId && x.Status == 1).ToList();
            return relatecategorylist;
        }

        public IList<WallPaperRelateTopic> GetWallRelateTopicList(int wallpaperId)
        {
            var relatetopiclist = dbContextService.Find<WallPaperRelateTopic>(x => x.WallPaperId == wallpaperId && x.Status == 1).ToList();
            return relatetopiclist;
        }

        public IList<WallPaperRelateMobileProperty> GetWallRelateMobilePropertyList(int wallpaperId)
        {
            var relatemobilepropertylist = dbContextService.Find<WallPaperRelateMobileProperty>(x => x.WallPaperId == wallpaperId && x.Status == 1).ToList();
            return relatemobilepropertylist;
        }

        public IList<int> GetRelateCategoryIds(int wallpaperId)
        {
            var categoryIds = new List<int>();
            var relatecategorylist = GetWallRelateCategoryList(wallpaperId);
            if (relatecategorylist != null && relatecategorylist.Count > 0)
            {
                foreach (var item in relatecategorylist)
                {
                    categoryIds.Add(item.CategoryId);
                }
            }
            return categoryIds;
        }

        public IList<int> GetRelateSubCategoryIds(int wallpaperId)
        {
            var subcategoryIds = new List<int>();
            var relatecategorylist = GetWallRelateSubCategoryList(wallpaperId);
            if (relatecategorylist != null && relatecategorylist.Count > 0)
            {
                foreach (var item in relatecategorylist)
                {
                    subcategoryIds.Add(item.SubCategoryId);
                }
            }
            return subcategoryIds;
        }

        public IList<int> GetRelateTopicIds(int wallpaperId)
        {
            var topicIds = new List<int>();
            var relatetopiclist = GetWallRelateTopicList(wallpaperId);
            if (relatetopiclist != null && relatetopiclist.Count > 0)
            {
                foreach (var item in relatetopiclist)
                {
                    topicIds.Add(item.TopicId);
                }
            }
            return topicIds;
        }

        public IList<int> GetRelateMobilePropertyIds(int wallpaperId)
        {
            var propertyIds = new List<int>();
            var relatepropertylist = GetWallRelateMobilePropertyList(wallpaperId);
            if (relatepropertylist != null && relatepropertylist.Count > 0)
            {
                foreach (var item in relatepropertylist)
                {
                    propertyIds.Add(item.MobilePropertyId);
                }
            }
            return propertyIds;
        }

        public IList<string> GetImageNameListByMobileProperty(string type, WallPaper wallpaper, List<int> mobilepropertyIds)
        {
            var thumbnailName = wallpaper.ThumbnailName;
            var originalName = wallpaper.OriginalName;
            var thumbnailNameList = new List<string>();

            var resolutionlist = MobileUIService.GetMobileResolutionList(mobilepropertyIds);
            if (resolutionlist != null && resolutionlist.Count > 0)
            {
                switch (type)
                {
                    case Const.WALLPAPER_THUMBNAIL:
                        foreach (var item in resolutionlist)
                        {
                            thumbnailNameList.Add(string.Format("{0}_{1}", item.Value, thumbnailName));
                        }
                        break;
                    case Const.WALLPAPER_ORIGINAL:
                        foreach (var item in resolutionlist)
                        {
                            thumbnailNameList.Add(string.Format("{0}_{1}", item.Value, originalName));
                        }
                        break;
                    default:
                        break;
                }
            }
            return thumbnailNameList.ToList();
        }

        public void WallPaperConfig(WallPaperConfigView model, List<int> categoryIds, List<int> subcategoryIds, List<int> topicIds, List<int> propertyIds, string resourceFilePath)
        {
            var wallpaper = model.WallPaper.MakeSureNotNull() as WallPaper;

            var outcategoryIds = new List<int>();
            var outsubcategoryIds = new List<int>();
            var outtopicIds = new List<int>();
            var outpropertyIds = new List<int>();

            var incategoryIds = categoryIds.InIds(model.RelateCategoryIds, out outcategoryIds);
            var insubcategoryIds = subcategoryIds.InIds(model.RelateSubCategoryIds, out outsubcategoryIds);
            var intopicIds = topicIds.InIds(model.RelateTopicIds, out outtopicIds);
            var inpropertyIds = propertyIds.InIds(model.RelateMobilePropertyIds, out outpropertyIds);

            AddRelateCategory(incategoryIds, wallpaper.Id);
            AddRelateTopic(intopicIds, wallpaper.Id);
            AddRelateMobileProperty(inpropertyIds, wallpaper.Id);

            UpdateRelateCategory(outcategoryIds, wallpaper.Id);
            UpdateRelateSubCategory(outsubcategoryIds, wallpaper.Id);
            UpdateRelateTopic(outtopicIds, wallpaper.Id);
            UpdateRelateMobileProperty(outpropertyIds, wallpaper.Id);

            try
            {
                Upload(wallpaper, inpropertyIds, resourceFilePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        #region Helper

        private void AddRelateCategory(List<int> categoryIds, int wallpaperId)
        {
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (var item in categoryIds)
                {
                    var categoryId = item;

                    var flag = UpdateRelateCategory(categoryId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateCategory();
                        model.WallPaperId = wallpaperId;
                        model.CategoryId = categoryId;
                        dbContextService.Add<WallPaperRelateCategory>(model);
                    }
                }
            }
        }

        private void AddRelateSubCategory(List<int> subcategoryIds, int wallpaperId)
        {
            if (subcategoryIds != null && subcategoryIds.Count > 0)
            {
                foreach (var item_sub in subcategoryIds)
                {
                    var subcategoryId = item_sub;

                    var flag = UpdateRelateSubCategory(subcategoryId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateSubCategory();
                        model.WallPaperId = wallpaperId;
                        model.SubCategoryId = subcategoryId;
                        dbContextService.Add<WallPaperRelateSubCategory>(model);
                    }
                }
            }
        }

        private void AddRelateTopic(List<int> topicIds, int wallpaperId)
        {
            if (topicIds != null && topicIds.Count > 0)
            {
                foreach (var item in topicIds)
                {
                    var topicId = item;
                    var flag = UpdateRelateTopic(topicId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateTopic();
                        model.WallPaperId = wallpaperId;
                        model.TopicId = topicId;
                        dbContextService.Add<WallPaperRelateTopic>(model);
                    }
                }
            }
        }

        private void AddRelateMobileProperty(List<int> propertyIds, int wallpaperId)
        {
            if (propertyIds != null && propertyIds.Count > 0)
            {
                foreach (var item in propertyIds)
                {
                    var propertyId = item;
                    var flag = UpdateRelateMobileProperty(propertyId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateMobileProperty();
                        model.WallPaperId = wallpaperId;
                        model.MobilePropertyId = propertyId;
                        dbContextService.Add<WallPaperRelateMobileProperty>(model);
                    }
                }
            }
        }

        private void UpdateRelateCategory(List<int> categoryIds, int wallpaperId)
        {
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (var item in categoryIds)
                {
                    var categoryId = item;
                    var flag = UpdateRelateCategory(categoryId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateSubCategory(List<int> subcategoryIds, int wallpaperId)
        {
            if (subcategoryIds != null && subcategoryIds.Count > 0)
            {
                foreach (var item in subcategoryIds)
                {
                    var subcategoryId = item;
                    var flag = UpdateRelateSubCategory(subcategoryId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateTopic(List<int> topicIds, int wallpaperId)
        {
            if (topicIds != null && topicIds.Count > 0)
            {
                foreach (var item in topicIds)
                {
                    var topicId = item;
                    var flag = UpdateRelateTopic(topicId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateMobileProperty(List<int> propertyIds, int wallpaperId)
        {
            if (propertyIds != null && propertyIds.Count > 0)
            {
                foreach (var item in propertyIds)
                {
                    var propertyId = item;
                    var flag = UpdateRelateMobileProperty(propertyId, wallpaperId, 0);
                }
            }
        }

        private bool UpdateRelateCategory(int categoryId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateCategory>(x => x.CategoryId == categoryId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateCategory>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateSubCategory(int subcategoryId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateSubCategory>(x => x.SubCategoryId == subcategoryId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateSubCategory>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateTopic(int topicId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateTopic>(x => x.TopicId == topicId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateTopic>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateMobileProperty(int propertyId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == propertyId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateMobileProperty>(instance);
                flag = true;
            }
            return flag;
        }

        #endregion

        public void Upload(WallPaper wallpaper, List<int> propertyIds, string resourceFilePath)
        {
            if (propertyIds != null && propertyIds.Count > 0 && !string.IsNullOrEmpty(wallpaper.ThumbnailName) && !string.IsNullOrEmpty(wallpaper.OriginalName))
            {
                var resolutionlist = MobileUIService.GetMobileResolutionList(propertyIds);

                var thumbnailfilePathPrefix = string.Format("{0}{1}\\", resourceFilePath, Const.THEME_THUMBNAILS_FOLDER_NAME);
                var originalfilePathPrefix = string.Format("{0}{1}\\", resourceFilePath, Const.THEME_ORIGINALS_FOLDER_NAME);

                var thumbnailFilePath = string.Format("{0}{1}", thumbnailfilePathPrefix, wallpaper.ThumbnailName);
                var originalFilePath = string.Format("{0}{1}", originalfilePathPrefix, wallpaper.OriginalName);

                foreach (var item in resolutionlist)
                {
                    UploadSignal(thumbnailFilePath, thumbnailfilePathPrefix, item, false);
                    UploadSignal(originalFilePath, originalfilePathPrefix, item, true);
                }
            }
        }

        private string UploadSignal(string filePath, string destFilePathPrefix, MobileResolution resolution, bool isOrignal)
        {
            var destFile = string.Empty;
            if (!string.IsNullOrEmpty(filePath))
            {
                var width = resolution.Value.GetResolutionWidth();
                var height = resolution.Value.GetResolutionHeight();

                var imagePixel = string.Format("{0}x{1}", width, height);
                if (!isOrignal)
                {
                    var thumbnailPixel = imagePixel.GetThumbnailPixelByOriginal();
                    width = thumbnailPixel.GetResolutionWidth();
                    height = thumbnailPixel.GetResolutionHeight();

                    destFile = ImageHelper.Resized(filePath, destFilePathPrefix, width, height, imagePixel);
                }
                else
                {
                    destFile = ImageHelper.Resized(filePath, destFilePathPrefix, width, height, string.Empty);
                }
            }
            return destFile;
        }
    }
}
