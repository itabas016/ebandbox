using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FrameMobile.Common;
using NCore;

namespace FrameMobile.Core
{
    public class ImageHelper
    {
        #region Const

        public const int BIG_IMAGE_WIDTH = 665;
        public const int BIG_IMAGE_HEIGHT = 558;

        public const int MEDIUM_IMAGE_WIDTH = 410;
        public const int MEDIUM_IMAGE_HEIGHT = 560;

        public const int SMALL_IMAGE_WIDTH = 358;
        public const int SMALL_IMAGE_HEIGHT = 305;

        public const int HD_IMAGE_WIDTH = 720;
        public const int NORMAL_IMAGE_WIDTH = 480;

        public static string NEWS_IMAGE_FILE_URL = ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue();

        #endregion

        #region Resized By Diff Size

        public static string ResizedNormal(string oriFileName, string destFilePath, long newsId)
        {
            return ResizedByWidth(oriFileName, destFilePath, NORMAL_IMAGE_WIDTH, newsId);
        }

        public static string ResizedHD(string oriFileName, string destFilePath, long newsId)
        {
            return ResizedByWidth(oriFileName, destFilePath, HD_IMAGE_WIDTH, newsId);
        }

        public static string ResizedToBig(string oriFileName, string destFilePath)
        {
            return ResizedBySize(oriFileName, destFilePath, BIG_IMAGE_WIDTH, BIG_IMAGE_HEIGHT);
        }

        public static string ResizedToMedium(string oriFileName, string destFilePath)
        {
            return ResizedBySize(oriFileName, destFilePath, MEDIUM_IMAGE_WIDTH, MEDIUM_IMAGE_HEIGHT);
        }

        public static string ResizedToSmall(string oriFileName, string destFilePath)
        {
            return ResizedBySize(oriFileName, destFilePath, SMALL_IMAGE_WIDTH, SMALL_IMAGE_HEIGHT);
        }

        public static string ResizedNormal(HttpPostedFileBase imageFile, string destFilePath, string destFileName)
        {
            return ResizedByWidth(imageFile, destFilePath, destFileName, NORMAL_IMAGE_WIDTH);
        }

        public static string ResizedHD(HttpPostedFileBase imageFile, string destFilePath, string destFileName)
        {
            return ResizedByWidth(imageFile, destFilePath, destFileName, HD_IMAGE_WIDTH);
        }

        public static string Resized(string originalFilePath, string destFilePath, int width, int height)
        {
            FileInfo fileInfo = new FileInfo(originalFilePath);
            var bitmap = new Bitmap(originalFilePath);
            if (bitmap != null)
            {
                if (bitmap.Width > width)
                {
                    var w = width;
                    var h = (w * bitmap.Height) / bitmap.Width;
                    var size = new Size(w, h);

                    var destBitMap = ResizeImage(bitmap, size);
                    if (destBitMap != null)
                    {
                        var destFileName = string.Format("{0}{1}x{2}{3}", destFilePath, width, height, fileInfo.Name);
                        destBitMap.Save(destFileName);
                    }
                }
            }
            return string.Empty;

        }

        #endregion

        #region Helper

        private static string ResizedByWidth(string oriFileNamePath, string destFilePath, int width, long newsId)
        {
            FileInfo fileInfo = new FileInfo(oriFileNamePath);
            var bitmap = new Bitmap(oriFileNamePath);
            if (bitmap.Width > width)
            {
                var height = (width * bitmap.Height) / bitmap.Width;
                var size = new Size(width, height);

                if (bitmap != null)
                {
                    var destBitMap = ResizeImage(bitmap, size);
                    if (destBitMap != null)
                    {
                        var destFileName = string.Format("{0}{1}{2}", destFilePath, newsId, fileInfo.Name);
                        destBitMap.Save(destFileName);
                        var cdnFileURL = string.Format("{0}/{1}/{2}{3}", NEWS_IMAGE_FILE_URL, width, newsId, fileInfo.Name);
                        return cdnFileURL;
                    }
                }
            }
            else
            {
                var destFileName = string.Format("{0}{1}{2}", destFilePath, newsId, fileInfo.Name);
                fileInfo.CopyTo(destFileName, true);
                var cdnFileURL = string.Format("{0}/{1}/{2}{3}", NEWS_IMAGE_FILE_URL, width, newsId, fileInfo.Name);
                return cdnFileURL;
            }

            return string.Empty;
        }

        private static string ResizedBySize(string oriFileNamePath, string destFilePath, int width, int height)
        {
            FileInfo fileInfo = new FileInfo(oriFileNamePath);

            var bitmap = new Bitmap(oriFileNamePath);
            var size = new Size(width, height);

            if (bitmap != null)
            {
                var destBitMap = ResizeImage(bitmap, size);
                if (destBitMap != null)
                {
                    var destFileName = string.Format("{0}{1}x{2}{3}", destFilePath, width, height, fileInfo.Name);
                    destBitMap.Save(destFileName);
                    return destFileName;
                }
            }
            return string.Empty;
        }

        private static string ResizedByWidth(HttpPostedFileBase imageFile, string destFilePath, string destFileName, int width)
        {
            var bitmap = new Bitmap(imageFile.InputStream);
            if (bitmap.Width > width)
            {
                var height = (width * bitmap.Height) / bitmap.Width;
                var size = new Size(width, height);

                if (bitmap != null)
                {
                    var destBitMap = ResizeImage(bitmap, size);
                    if (destBitMap != null)
                    {
                        var destFullFileName = string.Format("{0}{1}", destFilePath, destFileName);
                        destBitMap.Save(destFullFileName);
                        var cdnFileURL = string.Format("{0}/{1}/{2}", NEWS_IMAGE_FILE_URL, width, destFileName);
                        return cdnFileURL;
                    }
                }
            }
            else
            {
                var destFullFileName = string.Format("{0}{1}", destFilePath, destFileName);
                imageFile.SaveAs(destFullFileName);
                var cdnFileURL = string.Format("{0}/{1}/{2}", NEWS_IMAGE_FILE_URL, width, destFileName);
                return cdnFileURL;
            }

            return string.Empty;
        }

        private static Image ResizeImage(Bitmap mg, Size newSize)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Image bp;

            if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
            Convert.ToDouble(newSize.Height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
            bp = new Bitmap(newSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width) / 2;
            y = (newSize.Height - thumbSize.Height);
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.Low;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

            return bp;

        }

        #endregion
    }
}
