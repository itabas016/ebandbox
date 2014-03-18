using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Mobile
{
    [Serializable]
    [SubSonicTableNameOverride("mobileresolution")]
    public class MobileResolution : MobilePropertyBase
    {
        public decimal SimilarRatio
        {
            get
            {
                var width = StringHelper.GetWidth(this.Value);
                var height = StringHelper.GetHeight(this.Value);
                return width / height;
            }
        }
    }
}
