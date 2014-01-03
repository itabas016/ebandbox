using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;

namespace FrameMobile.Domain.Service
{
    public class WallPaperService : ThemeDbContextService, IWallPaperService
    {
        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        public IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount)
        {
            throw new NotImplementedException();
        }

        public WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId)
        {
            throw new NotImplementedException();
        }
    }
}
