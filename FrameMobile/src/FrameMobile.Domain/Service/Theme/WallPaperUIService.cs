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
    }
}
