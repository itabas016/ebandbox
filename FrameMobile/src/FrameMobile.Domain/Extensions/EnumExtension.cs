using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FrameMobile.Core;
using FrameMobile.Model;
using FrameMobile.Model.News;

namespace FrameMobile.Domain
{
    public static class EnumExtension
    {
        public static SelectList GetSelectList<T>(this IList<T> list) where T : MySQLModel
        {
            List<SelectItem> items = GetSelectItem<T>(list);

            return new SelectList(items, "Value", "Text");
        }

        public static SelectList GetSelectList<T>(this IList<T> list, object selectedValue) where T : MySQLModel
        {
            List<SelectItem> items = GetSelectItem<T>(list);

            return new SelectList(items, "Value", "Text", selectedValue);
        }

        private static List<SelectItem> GetSelectItem<T>(IList<T> list) where T : MySQLModel
        {
            List<SelectItem> items = new List<SelectItem>(list.Count);

            foreach (var item in list)
            {
                items.Add(new SelectItem
                {
                    Text = item.Name,
                    Value = ((int)item.Id).ToString()
                });
            }
            return items;
        }
    }
}
