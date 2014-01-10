using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;
using FrameMobile.Model.Theme;

namespace FrameMobile.Model
{
    public class WallPaperConfigView : ViewModelBase
    {
        public WallPaper WallPaper { get; set; }

        public List<WallPaperCategory> RelateCategorylist { get; set; }

        public List<WallPaperSubCategory> RelateSubCategorylist { get; set; }

        public List<WallPaperTopic> RelateTopiclist { get; set; }

        public List<MobileProperty> RelateMobilePropertylist { get; set; }
    }
}
