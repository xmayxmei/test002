using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace Weixin.Apps.Entity
{
    /// <summary>
    /// token实体
    /// </summary>
    public class WeixinToken
    {
        public static TokenVO tokenvo = null;
        
        /// <summary>
        /// 配置文件读取tokenvo;
        /// </summary>
        public WeixinToken()
        {
            if (tokenvo == null)
            {
                tokenvo = new TokenVO();
                System.Collections.IDictionary weixinSettings = (System.Collections.IDictionary)ConfigurationManager.GetSection("weixinSettings");
                tokenvo.token = (string)weixinSettings["token"];
                tokenvo.appId = (string)weixinSettings["appId"];
                tokenvo.appSecret = (string)weixinSettings["appSecret"];
            }
        }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public static string getAccessToken()
        {
            if (WeixinConstant.EXPIRES_DATE == null)
            {//为空，拉一个
                tokenvo = pullAccessToken(tokenvo);
            }
            else if (WeixinConstant.EXPIRES_DATE <= DateTime.Now)//早于，已过期
            {
                tokenvo = pullAccessToken(tokenvo);
            }
            else
            {
                tokenvo = new TokenVO();
                tokenvo.access_token = WeixinConstant.ACCESS_TOKEN;
            }
            return tokenvo.access_token;
        }
        /// <summary>
        /// access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。
        /// 正常情况下access_token有效期为7200秒，重复获取将导致上次获取的access_token失效。
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        static TokenVO pullAccessToken(TokenVO tk)
        {
            string accessTokenUrl = WeixinConstant.ACCESS_TOKEN_URL;
            string accessToken = WeixinConstant.ACCESS_TOKEN;
            accessTokenUrl = accessTokenUrl.Replace("\\{appId\\}", tk.appId);
            accessTokenUrl = accessTokenUrl.Replace("\\{appSecret\\}", tk.appSecret);
            /**
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            string accessTokenStr = client.DownloadString(accessTokenUrl);
            **/
            string accessTokenStr = WeixinUtils.getResponseByUrl(accessTokenUrl);
            //accessTokenStr = "{\"access_token\":\"SimPVeMxVqlfu019kpEiLG1jtSMmrRTdmsO7nww3gDM8M3L0mUsMkKR3sBAIvGe4FjLfEV4BzTXexyPT36a-E3NZk9SBoYMCfdw81mzooD1Tm5NyPXlNNo92yhcuqlmdD_SEFVEZ9P65CM9yqQO9Rw\",\"expires_in\":7200}";
            TokenVO tokenvo = WeixinUtils.JsonDeserialize<TokenVO>(accessTokenStr);
            tokenvo.appId = tk.appId;
            tokenvo.appSecret = tk.appSecret;
            updateAccessToken(tokenvo);//更新TOKEN
            return tokenvo;
        }
        
        /// <summary>
        /// 更新TOKEN
        /// </summary>
        public static void updateAccessToken(TokenVO tokenvo)
        {
            DateTime date = DateTime.Now.AddSeconds(tokenvo.expires_in - 600);//预防服务器时间差，默认7200秒
            WeixinConstant.ACCESS_TOKEN = tokenvo.access_token;
            WeixinConstant.EXPIRES_DATE = date;
        }
        
    }
}