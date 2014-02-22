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

        public List<int> RelateCategoryIds { get; set; }

        public List<int> RelateSubCategoryIds { get; set; }

        public List<int> RelateTopicIds { get; set; }

        public List<int> RelateMobilePropertyIds { get; set; }

        public List<string> ThumbnailNames { get; set; }

        public List<string> OriginalNames { get; set; }
    }
}
