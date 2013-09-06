using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Domain.Service.Fake
{
    public class NewsFakeService : INewsService
    {

        public IList<Model.NewsCategoryView> GetCategoryList(Model.MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        public IList<Model.NewsSubCategoryView> GetSubCategoryList(Model.MobileParam mobileParams)
        {
            throw new NotImplementedException();
        }

        public IList<Model.TouTiaoContentView> GetTouTiaoContentList(Model.MobileParam mobileParams, int categoryId, int startnum, int num, out int totalCount)
        {
            throw new NotImplementedException();
        }
    }
}
