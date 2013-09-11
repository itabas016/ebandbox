using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    [SubSonicTableNameOverride("newsloadmode")]
    public class NewsLoadModeView : NewsSourceView
    {
    }
}
