using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using NCore;

namespace FrameMobile.Web
{
    public class CommonActionResult
    {
        internal const string FORMAT_KEY = "format";
        internal const string JSON = "json";

        public CommonActionResult(IRequestRepository requestRepo, IViewModel viewModel)
        {
            this.ViewModels = new List<IViewModel> { viewModel };
            this.RequestRepo = requestRepo;
        }

        public CommonActionResult(IRequestRepository requestRepo, IEnumerable<IViewModel> viewModels)
        {
            this.ViewModels = new List<IViewModel>(viewModels);
            this.RequestRepo = requestRepo;
        }

        #region Common Properties

        internal IRequestRepository RequestRepo { get; set; }

        public CommonResult CommonResult { get; set; }

        public IList<IViewModel> ViewModels { get; set; }

        public string Host { get; set; }

        public int Count { get; set; }

        public int? Total { get; set; }

        public List<CustomHeaderItem> CustomResultHeaders
        {
            get
            {
                if (_customResultHeaderContent == null) _customResultHeaderContent = new List<CustomHeaderItem>();

                return _customResultHeaderContent;
            }
            set
            {
                _customResultHeaderContent = value;
            }
        }
        private List<CustomHeaderItem> _customResultHeaderContent;

        public bool SpecifyCount { get; set; }

        public bool UseSingleJsonResult { get; set; }

        public Action<CommonResult> CommonResultAction { get; set; }

        #endregion

        public override string ToString()
        {
            return ToString(this.Separator());
        }

        public string ToString(string separator)
        {
            return ToString(GetData, separator);
        }

        public string ToString(Func<IList<IViewModel>, string> getDataAction, string separator = "&")
        {
            return ToString(() => getDataAction(this.ViewModels), separator);
        }

        public string ToString(Func<string> getDataAction, string separator = "&")
        {
            var format = string.Empty;

            if (this.RequestRepo != null)
                format = this.RequestRepo.GetValueFromHeadOrQueryString(FORMAT_KEY);

            if (format.EqualsOrdinalIgnoreCase(JSON))
            {
                var jsonResult = default(JsonResultBase);
                if (this.UseSingleJsonResult && this.ViewModels.Count <= 1)
                    jsonResult = new JsonSingleDataResult(this);
                else
                    jsonResult = new JsonCommonResult(this);

                var ret = jsonResult.ToString();

                return ret;
            }
            return string.Empty;
        }

        #region Virtual Methods

        protected virtual string Separator()
        {
            return ASCII.AND;
        }

        protected virtual string GetData()
        {
            var ret = string.Empty;
            if (this.ViewModels != null)
            {
                if (this.ViewModels.Count == 1) ret = this.ViewModels[0].ToViewModelString();
                else ret = this.ViewModels.ToViewModelString();
            }
            return ret;
        }

        #endregion
    }

    public class CustomHeaderItem
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public bool IsValueType { get; set; }
    }
}
