using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace TencentAppStoreCap
{
    public class UrlRequest
    {
        // Methods
        public void DownloadFile(string fileUrl, string path, bool force = false)
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
            if (retryTimes > 0)
            {
                //LogHelper.WriteInfo("Can not download the image + " + fileUrl, ConsoleColor.DarkMagenta);
            }
        }

        public TResult TryGet<TResult>(Func<string> getUrl, Func<string, TResult> getResult, TResult defaultResult)
        {
            int retryTimes = 0;
            var ret = defaultResult;
            var url = getUrl();
        Label_0002:
            try
            {
                if (retryTimes > 0)
                {
                    LogHelper.WriteInfo("Get info failed : " + url, ConsoleColor.Magenta);
                }

                ret = getResult(url);

                retryTimes = 0;
            }
            catch
            {
                Thread.Sleep(500);
                LogHelper.WriteInfo("Get info failed : " + url, ConsoleColor.Red);
                retryTimes++;
                if (retryTimes < 3)
                {
                    goto Label_0002;
                }
            }
            if (retryTimes > 0)
            {
                LogHelper.WriteInfo("Can not get info from " + url, ConsoleColor.DarkMagenta);
            }
            return ret;
        }


        public string GetContent(string url)
        {
            string content = string.Empty;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.UserAgent = "Dalvik/1.4.0 (Linux; U; Android 2.3.5; MI-ONE Plus Build/GINGERBREAD)";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Encoding encoding = Encoding.GetEncoding("gb2312");
                StreamReader sr = new StreamReader(webResponse.GetResponseStream(), encoding);
                content = sr.ReadToEnd();
                webResponse.Close();
                sr.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo(ex.Message, ConsoleColor.Red);
            }
            return content;
        }

    }

}
