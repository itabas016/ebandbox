using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Cronus.Framework.Utilities;

namespace FrameMobile.Core
{
    public class HttpHelper
    {
        public static string HttpGet(string Url, string queryUrl)
        {
            var retString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (queryUrl == "" ? "" : "?") + queryUrl);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                Cronus.Framework.Utilities.LogHelper.Error(ex.Message);
            }
            return retString;
        }

        public static string HttpPost(string Url, string postDataStr)
        {
            var retString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                Cronus.Framework.Utilities.LogHelper.Error(ex.Message);
            }
            return retString;
        }

        public static void DownloadFile(string fileUrl, string path, bool force)
        {
            int retryTimes = 0;
        Label_0002:
            try
            {
                if (File.Exists(path) && (new FileInfo(path)).Length > 0 && !force)
                {
                    LogHelper.WriteInfo("Downloaded already: " + path, ConsoleColor.DarkMagenta);

                    return;
                }

                using (WebClient webClient = new WebClient())
                {
                    Console.WriteLine(fileUrl);
                    webClient.DownloadFile(fileUrl, path);
                }
                LogHelper.WriteInfo("Downloaded file: " + path, ConsoleColor.DarkGreen);
                retryTimes = 0;
            }
            catch (Exception ex)
            {
                Thread.Sleep(500);
                LogHelper.WriteInfo(ex.Message, ConsoleColor.Red);
                retryTimes++;
                if (retryTimes <= 3)
                {
                    goto Label_0002;
                }
            }
        }

        public static string DownloadFile(string fileUrl, string path)
        {
            int retryTimes = 0;
        Label_0002:
            try
            {
                if (File.Exists(path) && (new FileInfo(path)).Length > 0)
                {
                    return Path.GetDirectoryName(path);
                }

                WebRequest request = WebRequest.Create(fileUrl);
                var fileName = string.Empty;
                using (WebResponse response = request.GetResponse())
                using (WebClient webClient = new WebClient())
                {
                    string contentType = response.ContentType;
                    fileName = string.Format("{0}{1}", path, GetExtensionType(contentType));
                    webClient.DownloadFile(fileUrl, fileName);
                }
                retryTimes = 0;
                return fileName;
            }
            catch (Exception)
            {
                Thread.Sleep(500);
                retryTimes++;
                if (retryTimes <= 3)
                {
                    goto Label_0002;
                }
            }
        return string.Empty;
        }

        private static string GetExtensionType(string contentType)
        {
            var type = string.Empty;
            switch (contentType)
            {
                case "image/jpeg":
                case "image/pjpeg":
                    type = ".jpg";
                    break;
                case "image/gif":
                    type = ".gif";
                    break;
                case "image/png":
                case "image/x-png":
                    type = ".png";
                    break;
                case "image/x-ms-bmp":
                    type = ".bmp";
                    break;
                case "text/plain":
                case "text/richtext":
                case "text/html":
                    type = ".txt";
                    break;
                case "application/zip":
                case "application/x-zip-compressed":
                    type = ".zip";
                    break;
                case "application/x-rar-compressed":
                    type = ".rar";
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}
