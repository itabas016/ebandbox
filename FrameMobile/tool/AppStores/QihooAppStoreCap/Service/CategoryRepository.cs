using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;
using QihooAppStoreCap.Model;

namespace QihooAppStoreCap.Service
{
    public class CategoryRepository : SingletonBase<CategoryRepository>
    {
        private GetCategorys _category;
        private DataConvertService _service;

        protected internal Dictionary<string, string> CategoryIds = new Dictionary<string, string>();

        public CategoryRepository()
        {
            _category = new GetCategorys();
            _service = new DataConvertService();

            var categoryList = GetAllCategory();

            if (categoryList != null && categoryList.Count > 0)
            {
                foreach (var item in categoryList)
                {
                    CategoryIds.Add(item.CategoryId, item.CategoryName);
                }
            }
        }

        public List<QihooAppStoreCategory> GetAllCategory()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var data = _category.GetData(parameters, true);

            var catlist = _service.DeserializeCategory(data);

            return catlist;

        }

        public string GetCategoryNameById(string categoryId)
        {
            if (CategoryIds.ContainsKey(categoryId))
            {
                return CategoryIds[categoryId];
            }
            return string.Empty;
        }
    }
}
