using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weixin.Models;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// Util类
    /// </summary>
    public class WeixinUtils
    {
        /// <summary>
        /// 时间戳转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int convertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="codelist"></param>
        /// <returns></returns>
        public string GetSha1(System.Collections.Generic.List<string> codelist)
        {
            codelist.Sort();
            var combostr = string.Empty;
            for (int i = 0; i < codelist.Count; i++)
            {
                combostr += codelist[i];
            }
            return EncryptToSHA1(combostr);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string EncryptToSHA1(string str)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] str1 = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        /// <summary>
        /// 获取url 内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getResponseByUrl(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.GetEncoding("GBK");
            return client.DownloadString(url);
        }
        
        /// <summary>
        /// 获取url 内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getResponseByEncodeUrl(string url, string name)
        {
            WebClient client = new WebClient();
            byte[] reply = client.DownloadData(url + HttpUtility.UrlEncode(name, Encoding.GetEncoding("GBK")));
            return Encoding.UTF8.GetString(reply);
        }
        /// <summary>
        /// 获取推送的URL
        /// </summary>
        /// <returns></returns>
        public static string postMessage2UrlByToken(string token, string message)
        {
            string url = WeixinConstant.TEN_WX_POST_URL;
            url = url.Replace("\\{accessToken\\}", WeixinConstant.ACCESS_TOKEN);
            byte[] sendData = Encoding.GetEncoding("UTF-8").GetBytes(message.ToString());
            WebClient client = new WebClient();
            byte[] recData = client.UploadData(url, "POST", sendData);
            return Encoding.GetEncoding("UTF-8").GetString(recData);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="strMemo"></param>
        public static void WriteLog(string strMemo)
        {
            string logPath = "c:/log/";
            string logname = "WeiXin.txt";
            string filename = logPath + logname;
            //string logServerPath = context.Server.MapPath(logPath);
            if (!System.IO.Directory.Exists(logPath))
            {
                System.IO.Directory.CreateDirectory(logPath);
            }
            System.IO.StreamWriter sr = null;
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    sr = System.IO.File.CreateText(filename);
                }
                else
                {
                    sr = System.IO.File.AppendText(filename);
                }
                sr.WriteLine(DateTime.Now + ":" + strMemo);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

    }
}