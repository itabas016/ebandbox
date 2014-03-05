using FrameMobile.Model;
using FrameMobile.Model.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public interface IWallPaperUIService : IThemeServiceBase
    {
        IList<WallPaperCategory> GetWallPaperCategoryList();
        IList<WallPaperSubCategory> GetWallPaperSubCategoryList();
        IList<WallPaperTopic> GetWallPaperTopicList();
        void UpdateServerVersion<T>() where T : MySQLModelBase;
        IList<WallPaperRelateCategory> GetWallRelateCategoryList(int wallpaperId);
        IList<WallPaperRelateSubCategory> GetWallRelateSubCategoryList(int wallpaperId);
        IList<WallPaperRelateTopic> GetWallRelateTopicList(int wallpaperId);
        IList<WallPaperRelateMobileProperty> GetWallRelateMobilePropertyList(int wallpaperId);
        IList<int> GetRelateCategoryIds(int wallpaperId);
        IList<int> GetRelateSubCategoryIds(int wallpaperId);
        IList<int> GetRelateTopicIds(int wallpaperId);
        IList<int> GetRelateMobilePropertyIds(int wallpaperId);
        IList<string> GetImageNameListByMobileProperty(string imageType, WallPaper wallpaper, List<int> mobilepropertyIds);
        void WallPaperConfig(WallPaperConfigView model, List<int> categoryIds, List<int> subcategoryIds, List<int> topicIds, List<int> propertyIds);
    }
}
