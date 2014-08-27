using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using bgweb.Apps.Outer.Weixin;
using System.Text.RegularExpressions;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// 消息推送器
    /// </summary>
    public class WeixinPush
    {
       /// <summary>
       /// 推送文本消息
       /// </summary>
       /// <returns></returns>
        public void pushMessage(WeixinVO lowerWeixin,Object[] result)//下行
        {
            //取要发送的主体content内容
            String message = WeixinTempl.covtDailyCounter2PostStr(0,result,"","");
            lowerWeixin.Content = message;
            //取模板匹配模板(准备要发送的数据)
            string messageTmpl = WeixinTempl.makePushMessage(lowerWeixin);
            //获取token
            string token = WeixinToken.getAccessToken();
            //推送消息
            WeixinUtils.postMessage2Url(token, messageTmpl);
        }
    }
}