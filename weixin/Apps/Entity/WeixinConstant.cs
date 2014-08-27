using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// 常量
    /// </summary>
    public class WeixinConstant
    {
        public static string TYPE_TEXT = "text";//文本
        public static string TYPE_IMAGE = "image";//图片
        public static string TYPE_VOICE = "voice";//语音
        public static string TYPE_VIDEO = "video";//视频
        public static string TYPE_MUSIC = "music";//音乐
        public static string TYPE_NEWS = "news";//新闻
        public static string TYPE_LOCATION = "location";//位置
        public static string TYPE_LINK = "link";//链接
        public static string TYPE_EVENT = "event";//事件
        public static int CLAUSE_BIANMA = 0;//编码查询
        public static int CLAUSE_YOUXIAOQI = 1;//有效期查询
        public static int CLAUSE_BAOGUANDAN = 2;//通关单查询
        public static int CLAUSE_COUNTDAILY = 3;//报关小结
        public static int CLAUSE_BIND = 4;//绑定账户
        public static int CLAUSE_ORG_DAILY = 5;//日报
        public static int CLAUSE_ORG_WEEK = 6;//周报
        public static int CLAUSE_ORG_MONTH = 7;//月报
        public static int CLAUSE_YUNCLASS = 8;//云课堂
        public static int CLAUSE_INTRO = 9;//公司介绍
        public static int CLAUSE_HELLO = 10;//打招呼
        public static string EVENT_SCRIB = "subscribe";//订阅事件
        public static string EVENT_UNSCRIB = "unsubscribe";//取消订阅事件
        public static string EVENT_CLICK = "click";//点击菜单事件
        public static string EVENT_VIEW = "view";//点击菜单跳转URL
        public static string ACCESS_TOKEN = "";  //公众号的全局唯一票据
        public static DateTime EXPIRES_DATE ;//失效日期
        public static string APPID = ""; //第三方用户唯一凭证
        public static string APPSECRET = "";//第三方用户唯一凭证密钥
        public static string SEPT = ",";

        public static string ROOT_PATH = "http://weixin.yunbaoguan.cn/";

        public static string SERV_PATH = "http://42.121.136.137:3001/";

        public static string BIND_URL = "http://weixin.yunbaoguan.cn/Home/Bind";

        public static string ACCESS_TOKEN_URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={appSecret}";

        public static string TEN_WX_POST_URL = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={accessToken}"; //
    }
}