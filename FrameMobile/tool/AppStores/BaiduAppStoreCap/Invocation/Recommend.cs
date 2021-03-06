﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;

namespace BaiduAppStoreCap
{
    public class Recommend : InvocationBase
    {
        public int AppId { get; set; }

        public string PackageName { get; set; }

        protected override string MethodName
        {
            get { return "rec"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["apilevel"] = "4";
            base.AddAdditionalParams(parameters);
        }

        private void NecessaryParams()
        {
            if (this.AppId <= 0)
            {
                this.NameValues["package"] = this.PackageName.MakeSureNotNull();
            }
            else
            {
                this.NameValues["docid"] = this.AppId.ToString();
            }
        }
    }
}
