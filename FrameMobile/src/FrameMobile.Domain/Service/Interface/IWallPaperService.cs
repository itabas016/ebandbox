using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Theme;

namespace FrameMobile.Domain.Service
{
    public interface IWallPaperService : IThemeServiceBase
    {
        IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams,int cver, out int sver);

        IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams, int cver, out int sver);

        IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams, int cver, out int sver);

        IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int screenType, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount);

        WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId);

        IList<WallPaper> GetWallPaperListByScreenType(int screenType);

        IList<WallPaperRelateCategory> GetWallPaperRelateCategoryList(int categoryId);

        IList<WallPaperRelateSubCategory> GetWallPaperRelateSubCategoryList(int subcategoryId);

        IList<WallPaperRelateTopic> GetWallPaperRelateTopicList(int topicId);

        IList<WallPaperRelateMobileProperty> GetWallPaperRelateMobilePropertyList(int propertyId);

    }
}
