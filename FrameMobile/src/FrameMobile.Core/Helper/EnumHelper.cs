using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FrameMobile.Core
{
    public class EnumHelper
    {
        public static SelectList GetSelectListFromEnumType(Type enumType)
        {
            List<SelectItem> items = GetSelectItemsFromEnumType(enumType);

            return new SelectList(items, "Value", "Text");
        }

        public static SelectList GetSelectListFromEnumType(Type enumType, object selectedValue)
        {
            List<SelectItem> items = GetSelectItemsFromEnumType(enumType);

            return new SelectList(items, "Value", "Text", selectedValue);
        }

        private static List<SelectItem> GetSelectItemsFromEnumType(Type enumType)
        {
            Array values = Enum.GetValues(enumType);
            List<SelectItem> items = new List<SelectItem>(values.Length);

            foreach (var i in values)
            {
                items.Add(new SelectItem
                {
                    Text = Enum.GetName(enumType, i),
                    Value = ((int)i).ToString()
                });
            }
            return items;
        }
    }
}
