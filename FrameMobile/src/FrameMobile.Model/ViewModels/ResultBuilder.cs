using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FrameMobile.Common;
using FrameMobile.Core;

namespace FrameMobile.Model
{
    public static class ResultBuilder
    {
        private static Dictionary<Type, List<PropertyInfo>> _cachedTypeProperties = new Dictionary<Type, List<PropertyInfo>>();

        public static string BuildInstanceWithExcludedPropertyNames(object instance, List<string> excludedPropertyNames, bool withKey = true)
        {
            var toStringValue = BuildInstance(instance, true);
            var regex = new Regex(@"^\[(.*?)\](.*)", RegexOptions.IgnoreCase);
            Match headerMatch = regex.Match(toStringValue);

            List<string> headers = headerMatch.Groups[1].Captures[0].Value.Split(ASCII.COMMA_CHAR).ToList();
            List<string> values = headerMatch.Groups[2].Captures[0].Value.Split(ASCII.SEMICOLON_CHAR).ToList();

            if (headers.Count != values.Count) return string.Empty;

            List<int> excludedIndexes = new List<int>();
            for (int i = 0; i < headers.Count; i++)
            {
                if (excludedPropertyNames.Contains(headers[i]))
                {
                    excludedIndexes.Add(i);
                }
            }

            StringBuilder valueSB = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (!excludedIndexes.Contains(i))
                {
                    valueSB.AppendFormat("{0};", values[i]);
                }
            }
            if (valueSB.Length > 0)
            {
                valueSB.Remove(valueSB.Length - 1, 1);
                if (!withKey)
                {
                    return valueSB.ToString();
                }
            }

            StringBuilder headerSB = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (!excludedIndexes.Contains(i))
                {
                    headerSB.AppendFormat("{0},", headers[i]);
                }
            }
            if (headerSB.Length > 0)
            {
                headerSB.Remove(headerSB.Length - 1, 1);
            }

            if (valueSB.Length > 0 && headerSB.Length > 0)
            {
                return string.Format("[{0}]{1}", headerSB.ToString(), valueSB.ToString());
            }

            return string.Empty;
        }

        public static string BuildInstance(object instance, bool withKey = true)
        {
            if (instance != null)
            {
                var type = instance.GetType();
                var properties = new List<PropertyInfo>();

                #region PropertyInfo
                if (_cachedTypeProperties.ContainsKey(type))
                {
                    properties = _cachedTypeProperties[type];
                }
                else
                {
                    properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                    RemoveHiddenProperties(properties);

                    var declaredProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();
                    if (declaredProperties.Count > 0 && declaredProperties.Count < properties.Count)
                    {
                        properties = properties.Where(x => !declaredProperties.Exists(m => m.Name == x.Name)).ToList();
                        RemoveHiddenProperties(declaredProperties);

                        properties.AddRange(declaredProperties);
                    }
                }
                #endregion

                var sb = new StringBuilder();

                AppendKeys(properties, sb);

                var ret = AppendValues(instance, properties, sb);

                return ret;
            }

            return string.Empty;
        }

        private static void RemoveHiddenProperties(List<PropertyInfo> properties)
        {
            properties.RemoveAll(s =>
            {
                var viewFiledAttr = s.GetCustomAttributes(typeof(ViewFieldAttribute), false);
                if (viewFiledAttr.Any())
                {
                    var isDisplay = (viewFiledAttr[0] as ViewFieldAttribute).IsDisplay;
                    return isDisplay == false;
                }
                return false;
            });
        }

        private static void AppendKeys(List<PropertyInfo> properties, StringBuilder sb)
        {
            var displayNames = new List<string>();
            foreach (var property in properties)
            {
                var key = property.Name;
                var viewFieldMembers = property.GetCustomAttributes(typeof(ViewFieldAttribute), false);
                if (viewFieldMembers.Any())
                {
                    var viewField = (viewFieldMembers[0] as ViewFieldAttribute);
                    if (!string.IsNullOrEmpty(viewField.DisplayName)) key = viewField.DisplayName;
                }

                displayNames.Add(key);
            }
            sb.AppendFormat("[{0}]", string.Join(ASCII.COMMA, displayNames.ToArray()));
        }

        private static string AppendValues(object instance, List<PropertyInfo> properties, StringBuilder sb)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(instance, null);

                var valueString = default(String);
                if (property.PropertyType == typeof(DateTime)) valueString = ((DateTime)value).ToString(DateTimeFormat.DEFAULT_DATETIME_SEC_FORMAT);
                else if (property.PropertyType.IsEnum) valueString = ((int)value).ToString();
                else if (property.PropertyType.IsGenericType)
                {
                    if (property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (value != null) valueString = value.ToString();
                    }
                    else
                    {
                        foreach (var interfaceType in property.PropertyType.GetInterfaces())
                        {
                            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                                && interfaceType.GetGenericArguments().Length == 1)
                            {
                                var listValue = value as IEnumerable;
                                if (listValue == null) break;

                                var listValueArray = new List<string>();
                                foreach (var listItem in listValue) { listValueArray.Add(listItem.ToString()); }

                                break;
                            }
                        }
                    }
                }
                else valueString = value == null ? string.Empty : value.ToString();

                sb.Append(valueString);
            }

            return sb.ToString();
        }

        public static string BuildInstanceList<T>(IList<T> list) where T : IViewModel
        {
            var sb = new StringBuilder();

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var appendKey = i == 0;

                    sb.Append(BuildInstance(item, appendKey));
                }
            }

            return sb.ToString();
        }
    }
}
