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

        public static void DownloadFile(string fileUrl, string path, bool force = false)
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

                if (retryTimes > 0)
                {
                    //LogHelper.WriteInfo("Retry to download path: " + fileUrl, ConsoleColor.Magenta);
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
                //LogHelper.WriteInfo("Download file Failed: " + fileUrl, ConsoleColor.Red);
                LogHelper.WriteInfo(ex.Message, ConsoleColor.Red);
                retryTimes++;
                if (retryTimes <= 3)
                {
                    goto Label_0002;
                }
            }
        }
    }
}
