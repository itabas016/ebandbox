using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    public class MySQLModelBase : IMySQLModel
    {
        [SubSonicPrimaryKey]
        public int Id { get; set; }

        public DateTime CreateDateTime
        {
            get { return createTime; }
            set { createTime = value; }
        } private DateTime createTime = DateTime.Now;

        public int Status { get; set; }
    }
}
