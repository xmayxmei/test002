using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weixin.Models;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using Weixin.Controllers;
using Weixin.Apps.Service;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// 回复用户消息
    /// </summary>
    public class WeixinSender
    {
        string[] errMsg = { 
                              "抱歉，没有找到该编码，请确认编码格式是否正确。"
                              ,"抱歉，没有找到该经营单位，请确认经营单位名称是否正确。" 
                          };
        string[] resultMsg = {
                                "亲，怎么才关注【云报关】呀！您都错过好多内容啦！海关商品编码微信在线查询系统现已上线！亲可以回复8位或10位的编码查询监管条件或申报要素，回复经营单位简称可查询经营单位有效期等信息。"
                                ,"亲，你真的要离开么？"
                                ,"您还未绑定您的云报关账号。"
                                ,"请点击进入并登录您的云报关账号，以便绑定到您的微信账户。"
                                ,"亲，您查询的内容我们暂时无法回答，请加QQ报关群144257300讨论。"
                                ,"亲，报关员、归类师考试和培训请加QQ群144257300咨询。"
                                ,"亲，关于云报关软件产品介绍或咨询请联系电话慧通软件公司0755-86217997。或加入云报关QQ讨论群144257300。"
                             };
        string[,] newsMsg = {
                               {"【云报关】微信操作指引","","WebContent/Images/weixinbanner.jpg",""}
                               ,{"【商品编码查询】请直接回复8位或10位的商品编码，查看规范申报、监管条件等信息，如：0101210010","","WebContent/Images/01.png",""}
                               ,{"【经营单位有效期查询】请直接回复经营单位名称或简称，可查询有效期，如：华为","","WebContent/Images/02.png",""}
                              // ,{"【每日报关小结】请回复总结，查询您公司当天的报关情况小结，如总结","","WebContent/Images/03.png",""}
                              // ,{"【绑定账户】请回复绑定或点击进入绑定您的账户，我们将为您提供每日报关情况汇总消息推送服务，有效期提醒等服务，如果您已经绑定账户无需重复绑定。","","WebContent/Images/04.png",""}
                               //,{"","","",""}
                              // ,{"","","",""}
                               //,{"","","",""}
                             };
        /// <summary>
        /// 应答消息
        /// </summary>
        /// <param name="upperStr"></param>
        /// <returns></returns>
        public string replyResponse(string upperStr) {
            WeixinReceiver weixinReceiver = new WeixinReceiver();
            WeixinVO upperWeixin = weixinReceiver.parseMessager(upperStr);//解析微信服务器发送过来的数据
            string[] parsedContent = weixinReceiver.parseContent(upperWeixin.FromUserName, upperWeixin.Content, upperWeixin.MsgType, upperWeixin.Event);//分解用户发送的内容
            
            WeixinVO lowerWeixin= searchResult(parsedContent);//抓取数据返回给用户
            WeixinVO rtnWeixin = getLowerWeixin(lowerWeixin, upperWeixin);//根据上游消息，对下游消息处理
            
            string rtnStr = WeixinTempl.makeMessage(rtnWeixin);
            return rtnStr;
        }
        
        /// <summary>
        /// 结果查询
        /// </summary>
        /// <param name="parsedContent"></param>
        /// <returns></returns>
        public WeixinVO searchResult(string[] parsedContent) {
            if (parsedContent == null) {
                return null;
            }
            DateTime nowTime = DateTime.Now;
            int clauseKind = int.Parse(parsedContent[0]);//查询类型
            string clauseKey= parsedContent[1];//查询类容
            string fromUserName = parsedContent[2];//发件人
            string msgType = parsedContent[3];//消息类型
            string msgEvent = parsedContent[4];//事件类型
            string result = null;
            WeixinVO weixinVO = new WeixinVO();
            weixinVO.MsgType = WeixinConstant.TYPE_TEXT;

            if (WeixinConstant.TYPE_EVENT.Equals(msgType))//事件消息
            {
                if (WeixinConstant.EVENT_SCRIB.Equals(msgEvent))//订阅
                {
                    result = resultMsg[0];
                } else if (WeixinConstant.EVENT_UNSCRIB.Equals(msgEvent))//取消订阅
                {
                    result = resultMsg[1];
                }
                else if (WeixinConstant.EVENT_CLICK.Equals(msgEvent))//点击菜单事件
                {
                    result = resultMsg[1];
                }
                else if (WeixinConstant.EVENT_VIEW.Equals(msgEvent))//点击菜单URL查看
                {
                    result = resultMsg[1];
                }
            }
            else if (WeixinConstant.TYPE_VOICE.Equals(msgType))//语音消息
            {

            }
            else if (WeixinConstant.TYPE_IMAGE.Equals(msgType))//图片消息
            {
                
            }
            else if (WeixinConstant.TYPE_VIDEO.Equals(msgType))//视频消息
            {

            }
            else if (WeixinConstant.TYPE_LOCATION.Equals(msgType))//地理位置消息
            {

            }
            else if (WeixinConstant.TYPE_LINK.Equals(msgType))//链接消息
            {

            }
            else if (WeixinConstant.TYPE_TEXT.Equals(msgType))//文本消息
            {

            }
            weixinVO.Content = result;
            return weixinVO;
        }
        /// <summary>
        /// 生成账号绑定Url
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <returns></returns>
        public string getYunBind(string fromUserName)
        {
            string result = resultMsg[3] + WeixinConstant.BIND_URL + "?openid=" + fromUserName;
            return result;
        }
        /// <summary>
        /// 操作指引
        /// </summary>
        /// <returns></returns>
        public WeixinVO getYunHelper() {
            WeixinVO weixinVO = new WeixinVO();
            weixinVO.MsgType = WeixinConstant.TYPE_NEWS;
            List<WeixinVO.News> newsList = new List<WeixinVO.News>();
            WeixinVO.News news = null;
            for (int i = 0; i < newsMsg.GetLength(0);i++ )
            {
                news = new WeixinVO.News(newsMsg[i, 0], newsMsg[i, 1], WeixinConstant.ROOT_PATH + newsMsg[i, 2], newsMsg[i,3]);
                newsList.Add(news);
            }
            weixinVO.NewsList = newsList;
            return weixinVO;
        }

        /// <summary>
        /// 根据上游vo以及抓取的数据拼装下游vo
        /// </summary>
        /// <param name="lowerWeixin"></param>
        /// <param name="upperWeixin"></param>
        /// <returns></returns>
        public WeixinVO getLowerWeixin(WeixinVO lowerWeixin,WeixinVO upperWeixin) {
            lowerWeixin.FromUserName = upperWeixin.ToUserName;
            lowerWeixin.ToUserName = upperWeixin.FromUserName;
            lowerWeixin.CreateTime = WeixinUtils.convertDateTimeInt(System.DateTime.Now) + "";
            return lowerWeixin;
        }
        /// <summary>
        /// 获取公司统计数据
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="apply_dt"></param>
        /// <param name="apply_dt2"></param>
        /// <returns></returns>
        public string countCompByEmp(int clauseKind, string fromUserName, string apply_dt, string apply_dt2)
        {
            string result = null;
            List<object[]> resultList = new BaoguanService().countCompByEmp(clauseKind, fromUserName, apply_dt, apply_dt2);
            if (resultList != null && resultList.Count > 0)
            {
                foreach (object[] r in resultList)
                {
                    result = WeixinTempl.covtDailyCounter2PostStr(clauseKind, r, apply_dt, apply_dt2);
                }
            }
            else
            {
                result = resultMsg[3] + getYunBind(fromUserName);
            }
            return result;
        }
    }
}