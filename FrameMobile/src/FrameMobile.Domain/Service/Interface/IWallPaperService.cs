using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public interface IWallPaperService
    {
        IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams);

        IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams);

        IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams);

        IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount);

        WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId);
    }
}
