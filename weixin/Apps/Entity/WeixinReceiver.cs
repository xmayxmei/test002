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
    /// 接受并解析消息
    /// </summary>
    public class WeixinReceiver
    {
        /// <summary>
        /// 
        /// 解析收到的消息。
        /// </summary>
        /// <param name="postStr"></param>
        /// <returns></returns>
        public WeixinVO parseMessager(string postStr)
        {
            WeixinVO weixinVO = null;
            try
            {
                //内容不为空
                if (postStr != null && postStr.Trim().Length > 0)
                {
                    System.Xml.XmlDocument postObj = new System.Xml.XmlDocument();
                    postObj.LoadXml(postStr);
                    weixinVO = new WeixinVO();
                    
                    var FromUserNameList = postObj.GetElementsByTagName("FromUserName");//发送方帐号（一个OpenID），对应不同的APP有不同的ID
                    var ToUserNameList = postObj.GetElementsByTagName("ToUserName");//接收人（开发者）
                    var ContentList = postObj.GetElementsByTagName("Content");//收到的信息文本
                    var CreateTimeList = postObj.GetElementsByTagName("CreateTime");//消息创建时间 （整型）
                    var MsgTypeList = postObj.GetElementsByTagName("MsgType");//消息类型：text，image,voice,video,location,link,event
                    var EventList = postObj.GetElementsByTagName("Event");//事件类型，subscribe(订阅)、unsubscribe(取消订阅)
                    var PicUrlList = postObj.GetElementsByTagName("PicUrl");//图片链接
                    var MediaIdList = postObj.GetElementsByTagName("MediaId");//图片、语音等消息媒体id，可以调用多媒体文件下载接口拉取数据。
                    var MsgIdList = postObj.GetElementsByTagName("MsgId");//消息id，64位整型
                    var FormatList = postObj.GetElementsByTagName("Format");//语音格式，如amr，speex等
                    var ThumbMediaIdList = postObj.GetElementsByTagName("ThumbMediaId");//视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
                    var Location_XList = postObj.GetElementsByTagName("Location_X");//地理位置维度
                    var Location_YList = postObj.GetElementsByTagName("Location_Y");//地理位置精度
                    var ScaleList = postObj.GetElementsByTagName("Scale");//地图缩放大小
                    var LabelList = postObj.GetElementsByTagName("Label");//地理位置信息
                    var TitleList = postObj.GetElementsByTagName("Title");//消息标题
                    var DescriptionList = postObj.GetElementsByTagName("Description");//消息描述
                    var UrlList = postObj.GetElementsByTagName("Url");//消息链接
                   
                    if (FromUserNameList != null && FromUserNameList.Count > 0)
                    {
                        weixinVO.FromUserName = FromUserNameList[0].ChildNodes[0].Value;
                    }
                    if (ToUserNameList != null && ToUserNameList.Count > 0)
                    {
                        weixinVO.ToUserName = ToUserNameList[0].ChildNodes[0].Value;
                    }
                    if (ContentList != null && ContentList.Count > 0)
                    {
                        weixinVO.Content = ContentList[0].ChildNodes[0].Value;
                    }
                    if (MsgTypeList != null && MsgTypeList.Count > 0)
                    {
                        weixinVO.MsgType = MsgTypeList[0].ChildNodes[0].Value;
                    }
                    if (PicUrlList != null && PicUrlList.Count > 0)
                    {
                        weixinVO.PicUrl = PicUrlList[0].ChildNodes[0].Value;
                    }
                    if (CreateTimeList != null && CreateTimeList.Count > 0)
                    {
                        weixinVO.CreateTime = CreateTimeList[0].ChildNodes[0].Value;
                    }
                    if (EventList != null && EventList.Count > 0)
                    {
                        weixinVO.Event = EventList[0].ChildNodes[0].Value;
                    }
                    if (MediaIdList != null && MediaIdList.Count > 0)
                    {
                        weixinVO.MediaId = MediaIdList[0].ChildNodes[0].Value;
                    }
                    if (MsgIdList != null && MsgIdList.Count > 0)
                    {
                        weixinVO.MsgId = MsgIdList[0].ChildNodes[0].Value;
                    }
                    if (FormatList != null && FormatList.Count > 0)
                    {
                        weixinVO.Format = FormatList[0].ChildNodes[0].Value;
                    }
                    if (ThumbMediaIdList != null && ThumbMediaIdList.Count > 0)
                    {
                        weixinVO.ThumbMediaId = ThumbMediaIdList[0].ChildNodes[0].Value;
                    }
                    if (Location_XList != null && Location_XList.Count > 0)
                    {
                        weixinVO.Location_X = Location_XList[0].ChildNodes[0].Value;
                    }
                    if (Location_YList != null && Location_YList.Count > 0)
                    {
                        weixinVO.Location_Y = Location_YList[0].ChildNodes[0].Value;
                    }
                    if (ScaleList != null && ScaleList.Count > 0)
                    {
                        weixinVO.Scale = ScaleList[0].ChildNodes[0].Value;
                    }
                    if (LabelList != null && LabelList.Count > 0)
                    {
                        weixinVO.Label = LabelList[0].ChildNodes[0].Value;
                    }
                    if (TitleList != null && TitleList.Count > 0)
                    {
                        weixinVO.Title = TitleList[0].ChildNodes[0].Value;
                    }
                    if (DescriptionList != null && DescriptionList.Count > 0)
                    {
                        weixinVO.Description = DescriptionList[0].ChildNodes[0].Value;
                    }
                    if (PicUrlList != null && PicUrlList.Count > 0)
                    {
                        weixinVO.Url = PicUrlList[0].ChildNodes[0].Value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return weixinVO;
        }
        /// <summary>
        /// 解析特定前缀的消息
        /// </summary>
        public string[] parseContent(string fromUserName,string content,string msgType,string msgEvent){
            int clauseKind = -1;//查询的业务类型
            string machedstr = null;
            if (WeixinConstant.TYPE_TEXT.Equals(msgType))//文本消息
            {
                //商品编码，名称（数字10位）
                //经营单位有效期（）
                //报关进程状态查询（数字报关单9-18位（前4位关区代码，再4位年，第九位1进口，0出口，后9位序列号））
                if (content == null || content.Trim().Length == 0)
                {
                    return null;
                }
                content = content.ToUpper();//用户输入字符转换为大写后处理
                string[][] clauseKey = new string[11][];//

                clauseKey[WeixinConstant.CLAUSE_BIANMA] = new string[6] { "商品", "编码", "商品编码", "申报要素", "HS", "BM" };
                clauseKey[WeixinConstant.CLAUSE_YOUXIAOQI] = new string[4] { "有效期", "经营单位", "YXQ", "DW" };
                clauseKey[WeixinConstant.CLAUSE_BAOGUANDAN] = new string[5] { "进程", "状态", "报关单", "提单", "装船单" };
                clauseKey[WeixinConstant.CLAUSE_COUNTDAILY] = new string[6] { "小结", "总结", "小计", "总计", "统计", "ZJ" };
                clauseKey[WeixinConstant.CLAUSE_BIND] = new string[5] { "绑定", "账户", "账号", "BD", "ZH" };
                clauseKey[WeixinConstant.CLAUSE_ORG_DAILY] = new string[2] { "日报", "RB" };
                clauseKey[WeixinConstant.CLAUSE_ORG_WEEK] = new string[2] { "周报", "ZB" };
                clauseKey[WeixinConstant.CLAUSE_ORG_MONTH] = new string[2] { "月报", "YB" };
                clauseKey[WeixinConstant.CLAUSE_YUNCLASS] = new string[6] { "归类", "考试", "答题", "每日一练", "报关员", "归类师" };
                clauseKey[WeixinConstant.CLAUSE_INTRO] = new string[2] { "云报关", "联系电话" };
                clauseKey[WeixinConstant.CLAUSE_HELLO] = new string[18] {"亲", "你好", "您好", "请问", ",", "，", "?", "？", ".", "。", ":", "：", "/", "\\", "<", ">", "!", "！" };//"\\s", "\\d{1-2}", "",
                //关键字前缀匹配
                for (int i = 0; i < clauseKey.Length; i++)
                {
                    for (int j = 0; j < clauseKey[i].Length; j++)
                    {
                        if (content.IndexOf(clauseKey[i][j]) >= 0)
                        {
                            clauseKind = i;
                            content = content.Substring(clauseKey[i][j].Length);
                            break;
                        }
                    }
                    if (clauseKind != -1)
                    {
                        break;
                    }
                }
                //如果关键字没匹配到
                if (clauseKind == -1)
                {
                    string regstr = "\\d{10}";
                    Regex reg = new Regex(regstr, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match mc = reg.Match(content);
                    machedstr = mc.Groups[0].ToString();
                    if (machedstr != null && !"".Equals(machedstr))//若可以匹配到10位
                    {
                        clauseKind = WeixinConstant.CLAUSE_BIANMA;//查编码
                    }
                    else
                    {//如果不能匹配到10位，再匹配8位的
                        regstr = "\\d{8}";
                        reg = new Regex(regstr, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        mc = reg.Match(content);
                        machedstr = mc.Groups[0].ToString();
                        if (machedstr != null && !"".Equals(machedstr))
                        {
                            clauseKind = WeixinConstant.CLAUSE_BIANMA;//查编码
                            if (machedstr.Length == 8)
                            {
                                machedstr = machedstr + "00";
                            }
                        }
                        else
                        {
                            clauseKind = WeixinConstant.CLAUSE_YOUXIAOQI;//有效期
                            machedstr = content;
                        }
                    }
                }
            }
            return new string[] { clauseKind + "", machedstr, fromUserName, msgType, msgEvent };
        }
    }
}