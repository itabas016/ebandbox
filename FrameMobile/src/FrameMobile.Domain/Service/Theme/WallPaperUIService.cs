using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;
using FrameMobile.Model.Theme;

namespace FrameMobile.Domain.Service
{
    public class WallPaperUIService : ThemeDbContextService, IWallPaperUIService
    {
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

        public MobileProperty GetMobileProperty(MobileParam mobileParams)
        {
            throw new NotImplementedException();
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
    }
}
