using FrameMobile.Model;
using FrameMobile.Model.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Domain.Service
{
    public interface IWallPaperUIService : IThemeServiceBase
    {
        IList<WallPaperCategory> GetWallPaperCategoryList();
        IList<WallPaperSubCategory> GetWallPaperSubCategoryList();
        IList<WallPaperTopic> GetWallPaperTopicList();
        void UpdateServerVersion<T>() where T : MySQLModelBase;
    }
}
