using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;
using FrameMobile.Model.Theme;

namespace FrameMobile.Domain.Service
{
    public interface IWallPaperServiceHelper
    {
        IList<WallPaperView> GetLatestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount);
        IList<WallPaperView> GetHottestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount);
        IList<WallPaperRelateCategory> GetWallPaperRelateCategoryList(int categoryId);
        IList<WallPaperRelateSubCategory> GetWallPaperRelateSubCategoryList(int subcategoryId);
        IList<WallPaperRelateTopic> GetWallPaperRelateTopicList(int topicId);
        IList<WallPaperRelateMobileProperty> GetWallPaperRelateMobilePropertyList(int propertyId);
    }
}
