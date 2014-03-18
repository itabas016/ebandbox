using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FrameMobile.Model;

namespace FrameMobile.Domain
{
    public static class CommonExtensions
    {
        public static string NormalzieFileName(this string fileName)
        {
            return fileName.Replace("*", string.Empty).Replace(@"\", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("|", string.Empty);
        }

        public static object MakeSureNotNull(this object instance)
        {
            var ret = instance == null ? new object() : instance;
            return ret;
        }

        public static string GetFileType(this HttpPostedFileBase file)
        {
            if (file != null)
            {
                string extension = Path.GetExtension(file.FileName);
                return extension;
            } return string.Empty;
        }

        public static string GetFileNamePrefix(this string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var array = fileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                var typeName = array[array.Length - 1];
                var fileNamePrefix = fileName.Substring(0, fileName.Length - (typeName.Length + 1));
                return fileNamePrefix;
            }
            return string.Empty;
        }

        public static string GetFileNamePrefix(this HttpPostedFileBase file)
        {
            if (file != null)
            {
                var fileName = file.FileName;
                var fileNamePrefix = GetFileNamePrefix(fileName);
                return fileNamePrefix;
            }
            return string.Empty;
        }

        public static string GetFilePixel(this HttpPostedFileBase file)
        {
            var pixel = string.Empty;
            if (file != null)
            {
                var image = Image.FromStream(file.InputStream, true, true);
                if (image != null)
                {
                    var width = image.Width;
                    var height = image.Height;
                    pixel = string.Format("{0}x{1}", width, height);
                }
            }
            return pixel;
        }

        public static string GetFilePixel(this string fileName)
        {
            var pixel = string.Empty;
            if (System.IO.File.Exists(fileName))
            {

                var image = Image.FromFile(fileName);
                if (image != null)
                {
                    var width = image.Width;
                    var height = image.Height;
                    pixel = string.Format("{0}x{1}", width, height);
                }
                return pixel;
            }
            return string.Empty;
        }

        public static IQueryable<T> ConvertIQueryable<T>(this IEnumerable<T> source) where T : MySQLModelBase
        {
            if (source is IQueryable<T>)
            {
                return (IQueryable<T>)source;
            }
            return new EnumerableQuery<T>(source);
        }

        public static IList<T> GetCompleteInstance<T>(this IList<T> list) where T : MySQLModel, new()
        {
            var instance = new T()
            {
                Id = 0,
                Status = 1,
                Name = "全部",
            };
            list.Insert(0, instance);
            return list;
        }

        public static List<int> InIds(this List<int> currentIds, List<int> originalIds, out List<int> outIds)
        {
            var inIds = new List<int>();
            outIds = new List<int>();
            if (currentIds != null && originalIds != null)
            {
                foreach (var currentId in currentIds)
                {
                    if (!originalIds.Contains(currentId))
                    {
                        inIds.Add(currentId);
                    }
                }
                foreach (var originalId in originalIds)
                {
                    if (!currentIds.Contains(originalId))
                    {
                        outIds.Add(originalId);
                    }
                }
            }
            else
            {
                inIds = currentIds;
            }
            return inIds;
        }
    }
}
