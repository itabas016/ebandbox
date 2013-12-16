﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Category : CategoryList
    {
        public int CategoryId { get; set; }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["id"] = this.CategoryId.ToString();
            this.NameValues["apilevel"] = "4";
            this.NameValues["pn"] = "0";
            base.AddAdditionalParams(parameters);
        }
    }
}
