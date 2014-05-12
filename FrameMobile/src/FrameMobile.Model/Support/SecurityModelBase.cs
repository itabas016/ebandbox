using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;
using FrameMobile.Core;

namespace FrameMobile.Model
{
    [Serializable]
    public class SecurityModelBase : ISecurityModelBase
    {
        [SubSonicLongString]
        public string JsonResult { get; set; }

        public int Version { get; set; }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }private int _status = 1;

        public DateTime CreateDateTime
        {
            get { return createTime; }
            set { createTime = value; }
        } private DateTime createTime = DateTime.Now;

        [SubSonicPrimaryKey]
        public int Id { get; set; }
    }
}
