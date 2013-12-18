using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Board : BoardList
    {
        public int BoardId { get; set; }

        public int PageNum { get; set; }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["id"] = this.BoardId.ToString();
            this.NameValues["apilevel"] = "4";
            this.NameValues["rn"] = "50";
            this.NameValues["pn"] = PageNum.ToString();
            base.AddAdditionalParams(parameters);
        }
    }
}
