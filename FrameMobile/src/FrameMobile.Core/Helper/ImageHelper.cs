﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
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

        #endregion

        #region Resized By Diff Size

        public static string ResizedNormal(string oriFileName, string destFilePath)
        {
            return ResizedByWidth(oriFileName, destFilePath, NORMAL_IMAGE_WIDTH);
        }

        public static string ResizedHD(string oriFileName, string destFilePath)
        {
            return ResizedByWidth(oriFileName, destFilePath, HD_IMAGE_WIDTH);
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

        #endregion

        #region Helper

        private static string ResizedByWidth(string oriFileNamePath, string destFilePath, int width)
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
                        var destFileName = string.Format("{0}{1}x{2}{3}", destFilePath, width, height, fileInfo.Name);
                        destBitMap.Save(destFileName);
                        return destFileName;
                    }
                }
            }
            else
            {
                var destFileName = string.Format("{0}{1}x{2}{3}", destFilePath, bitmap.Width, bitmap.Height, fileInfo.Name);
                fileInfo.CopyTo(destFileName, true);
                return destFileName;
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
