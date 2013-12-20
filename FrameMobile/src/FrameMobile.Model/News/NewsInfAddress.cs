using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsinfaddress")]
    public class NewsInfAddress : MySQLModel
    {
        public int SourceId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int IsStamp { get; set; }

        [SubSonicStringLength(256)]
        public string InfAddress { get; set; }
    }
}
