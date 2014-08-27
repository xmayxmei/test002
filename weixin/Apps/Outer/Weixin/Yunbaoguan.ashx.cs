
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weixin.Apps.Entity;

namespace bgweb.Apps.Outer.Weixin
{
    /// <summary>
    /// 云报关baoguanyunclass对微信接口
    /// </summary>
    public class Yunbaoguan : IHttpHandler
    {
        HttpContext context = null;
        string postStr = "";
        public static string TOKEN = "loveyou"; //"fuckyou";
        public static string APPID = "wx6f2970709fa1a097";        //"wx6b07bfbd730a9028";
        public static string APPSECRET = "4277df640169c96f714d0af6d5e1e4c9";    //"27d614e83a42042b31e24faeaa680963";
        
        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="param_context"></param>
        public void ProcessRequest(HttpContext param_context)
        {
            context = param_context;
           // valid(context);//用于验证
            /*** * ***/
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                System.IO.Stream s = context.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = System.Text.Encoding.UTF8.GetString(b);//xml格式
                WeixinUtils.WriteLog("INN: " + postStr);
                //收到消息后返回信息
                if (!string.IsNullOrEmpty(postStr))
                {
                    string responseMsg = new WeixinSender().replyResponse(postStr);
                    WeixinUtils.WriteLog("OUT: " + responseMsg);
                    context.Response.Write(responseMsg);
                    context.Response.End();
                }
            }
            
        }
        /// <summary>
        /// 开通时第一次校验
        /// </summary>
        public void valid(HttpContext context)
        {
            string echostr = context.Request["echoStr"];
            string signature = context.Request["signature"];
            string timestamp = context.Request["timestamp"];
            string nonce = context.Request["nonce"];
            string reqlog = DateTime.Now + "\r\nechostr:" + echostr + "\r\nsignature:" + timestamp + "\r\ntimestamp:" + echostr + "\r\nnonce:" + nonce + "\r\n----";
            
            //context.Response.Output.WriteLine(reqlog);
            WeixinUtils.WriteLog("INN: " + reqlog);
            context.Response.Output.WriteLine(echostr);
            if (checkSignature(signature, timestamp, nonce) && !string.IsNullOrEmpty(echostr))
            {
                context.Response.ClearContent();
                context.Response.ContentType = "text/plain";
                context.Response.Write(echostr);
                context.Response.End();//推送...不然微信平台无法验证token
            }
        }
        
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool checkSignature(string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { TOKEN, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}