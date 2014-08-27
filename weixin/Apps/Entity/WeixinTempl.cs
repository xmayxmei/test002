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
    /// 微信回复的模板定义与转换为内容
    /// </summary>
    public class WeixinTempl
    {
        static string BIANMA_TMPL = "HS编码：{code_ts}\n商品名称：{g_name}\n备注：{note_s}\n海关监管条件：{control} \n申报要素：{tax_type}\n法一单位：{unit_no}\n法二单位：{sunit_no}\n进口最惠国税率：{low_rate}\n普通税率：{high_rate}\n增值税率：{tax_rate}";

        static string YOUXIAOQI_TMPL = "公司名称：{name}\n海关编码：{code}\n报关类别：{com_type}\n等级：{com_Level}\n有效日期：{limit_dt}\n通关期限：{exp_dt}";

        static string CUST_TOTAL_TMPL = "公司名：{cust_name}，您{apply_dt}在{decl_name}总计{count_total}份报关单。其中：" +
                "\n清关出口{count_exp}份，清关进口{count_imp}份，转关进口{count_trans_imp}份，转关出口{count_trans_exp}份，属地岸放{count_locat}份。" +
                "\n当日已放行{count_pass}份，海关查验{count_hgcheck}份，商检查验{count_sjcheck}份，截止{now_time}累计未放行{count_unpass}份。" +
                "\n查验的柜号如下：{list_check}，未放行的柜号如下：{list_unpass}。";

        static string ORG_DAILY_TMPL = "公司名：{decl_name}，您{apply_dt}日总计{count_total}份报关单。其中：" +
                "\n清关出口{count_exp}份，清关进口{count_imp}份，转关进口{count_trans_imp}份，转关出口{count_trans_exp}份，属地岸放{count_locat}份。" +
                "\n当日已放行{count_pass}份，海关查验{count_hgcheck}份，商检查验{count_sjcheck}份，截止{now_time}累计未放行{count_unpass}份。" +
                "\n查验的柜号如下：{list_check}，未放行的柜号如下：{list_unpass}。";

        static string ORG_WEEK_TMPL = "公司名：{decl_name}，您{apply_dt}日到{apply_dt2}日总计{count_total}份报关单。其中：" +
                "\n清关出口{count_exp}份，清关进口{count_imp}份，转关进口{count_trans_imp}份，转关出口{count_trans_exp}份，属地岸放{count_locat}份。" +
                "\n海关查验{count_hgcheck}份，商检查验{count_sjcheck}份。";

        static string ORG_MONTH_TMPL = "公司名：{decl_name}，您{apply_dt}日到{apply_dt2}日总计{count_total}份报关单。其中：" +
                "\n清关出口{count_exp}份，清关进口{count_imp}份，转关进口{count_trans_imp}份，转关出口{count_trans_exp}份，属地岸放{count_locat}份。" +
                "\n海关查验{count_hgcheck}份，商检查验{count_sjcheck}份。";

        /// <summary>
        /// 实例化消息，对模板进行转换为发送到腾讯微信服务器的XML文本
        /// </summary>
        /// <param name="weixinVO"></param>
        /// <returns></returns>
        public static string makeMessage(WeixinVO weixinVO)
        {
            //获取消息模板
            string messageTmpl = getMessageTmpl(weixinVO);
            //消息模板实例化
            messageTmpl = Regex.Replace(messageTmpl, "\\{toUser\\}", weixinVO.ToUserName == null ? "" : weixinVO.ToUserName);
            messageTmpl = Regex.Replace(messageTmpl, "\\{fromUser\\}", weixinVO.FromUserName == null ? "" : weixinVO.FromUserName);
            messageTmpl = Regex.Replace(messageTmpl, "\\{createTime\\}", weixinVO.CreateTime == null ? "" : weixinVO.CreateTime);
            messageTmpl = Regex.Replace(messageTmpl, "\\{content\\}", weixinVO.Content == null ? "" : weixinVO.Content);
            messageTmpl = Regex.Replace(messageTmpl, "\\{msgType\\}", weixinVO.MsgType == null ? "" : weixinVO.MsgType);
            messageTmpl = Regex.Replace(messageTmpl, "\\{picUrl\\}", weixinVO.PicUrl == null ? "" : weixinVO.PicUrl);
            messageTmpl = Regex.Replace(messageTmpl, "\\{event\\}", weixinVO.Event == null ? "" : weixinVO.Event);
            messageTmpl = Regex.Replace(messageTmpl, "\\{media_id\\}", weixinVO.MediaId == null ? "" : weixinVO.MediaId);
            messageTmpl = Regex.Replace(messageTmpl, "\\{msgId\\}", weixinVO.MsgId == null ? "" : weixinVO.MsgId);
            messageTmpl = Regex.Replace(messageTmpl, "\\{format\\}", weixinVO.Format == null ? "" : weixinVO.Format);
            messageTmpl = Regex.Replace(messageTmpl, "\\{thumbMediaId\\}", weixinVO.ThumbMediaId == null ? "" : weixinVO.ThumbMediaId);
            messageTmpl = Regex.Replace(messageTmpl, "\\{location_X\\}", weixinVO.Location_X == null ? "" : weixinVO.Location_X);
            messageTmpl = Regex.Replace(messageTmpl, "\\{location_Y\\}", weixinVO.Location_Y == null ? "" : weixinVO.Location_Y);
            messageTmpl = Regex.Replace(messageTmpl, "\\{scale\\}", weixinVO.Scale == null ? "" : weixinVO.Scale);
            messageTmpl = Regex.Replace(messageTmpl, "\\{label\\}", weixinVO.Label == null ? "" : weixinVO.Label);
            messageTmpl = Regex.Replace(messageTmpl, "\\{description\\}", weixinVO.Description == null ? "" : weixinVO.Description);
            messageTmpl = Regex.Replace(messageTmpl, "\\{url\\}", weixinVO.Url == null ? "" : weixinVO.Url);
            messageTmpl = Regex.Replace(messageTmpl, "\\{musicUrl\\}", weixinVO.MusicUrl == null ? "" : weixinVO.MusicUrl);
            messageTmpl = Regex.Replace(messageTmpl, "\\{hqMusicUrl\\}", weixinVO.HQMusicUrl == null ? "" : weixinVO.HQMusicUrl);
            /*****/
            if (weixinVO.NewsList != null && weixinVO.NewsList.Count>0)
            {
                messageTmpl = Regex.Replace(messageTmpl, "\\{articleCount\\}", weixinVO.NewsList.Count + "");
                string regstr = "<item>.*</item>";
                Regex reg = new Regex(regstr, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match mc = reg.Match(messageTmpl);
                string machedstr = mc.Groups[0].ToString();
                StringBuilder replacedstr = new StringBuilder();
                /**news需要循环替换，产生多个**/
                foreach (WeixinVO.News news in weixinVO.NewsList)
                {
                    String item = new String(machedstr.ToCharArray());
                    item = Regex.Replace(item, "\\{newsTitle\\}", news.title == null ? "" : news.title);
                    item = Regex.Replace(item, "\\{newsDescription\\}", news.description == null ? "" : news.description);
                    item = Regex.Replace(item, "\\{newsPicUrl\\}", news.picUrl == null ? "" : news.picUrl);
                    item = Regex.Replace(item, "\\{newsUrl\\}", news.url == null ? "" : news.url);
                    replacedstr.Append(item);
                }
                messageTmpl = messageTmpl.Replace(machedstr, replacedstr.ToString());
            }
            else{
                messageTmpl = Regex.Replace(messageTmpl, "\\{articleCount\\}", "0");
            }
            return messageTmpl;
        }
        /// <summary>
        /// 实例化推送消息，对模板进行转换为发送到腾讯微信服务器的XML文本
        /// </summary>
        /// <param name="weixinVO"></param>
        /// <returns></returns>
        public static string makePushMessage(WeixinVO weixinVO)
        {
            //获取消息模板
            string messageTmpl = getPushMessageTmpl(weixinVO);
            //消息模板实例化
            messageTmpl = Regex.Replace(messageTmpl, "\\{toUser\\}", weixinVO.ToUserName == null ? "" : weixinVO.ToUserName);
            messageTmpl = Regex.Replace(messageTmpl, "\\{content\\}", weixinVO.Content == null ? "" : weixinVO.Content);
            messageTmpl = Regex.Replace(messageTmpl, "\\{msgType\\}", weixinVO.MsgType == null ? "" : weixinVO.MsgType);
            messageTmpl = Regex.Replace(messageTmpl, "\\{media_id\\}", weixinVO.MediaId == null ? "" : weixinVO.MediaId);
            messageTmpl = Regex.Replace(messageTmpl, "\\{msgId\\}", weixinVO.MsgId == null ? "" : weixinVO.MsgId);
            messageTmpl = Regex.Replace(messageTmpl, "\\{description\\}", weixinVO.Description == null ? "" : weixinVO.Description);
            return messageTmpl;
        }
        /// <summary>
        /// 获取 （回复消息的模板）
        /// </summary>
        /// <param name="weixinVO"></param>
        /// <returns></returns>
        public static string getMessageTmpl(WeixinVO weixinVO)
        {
            string messageTmpl = null;

            if (WeixinConstant.TYPE_TEXT.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{content}]]></Content></xml>";
            }
            if (WeixinConstant.TYPE_IMAGE.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[{mediaId}]]></MediaId></Image></xml>";
            }
            if (WeixinConstant.TYPE_VOICE.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[voice]]></MsgType><Voice><MediaId><![CDATA[{mediaId}]]></MediaId></Voice></xml>";
            }
            if (WeixinConstant.TYPE_VIDEO.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[video]]></MsgType><Video><MediaId><![CDATA[{mediaId}]]></MediaId><Title><![CDATA[{title}]]></Title><Description><![CDATA[{description}]]></Description></Video></xml>";
            }
            if (WeixinConstant.TYPE_MUSIC.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[music]]></MsgType><Music><Title><![CDATA[{title}]]></Title><Description><![CDATA[{description}]]></Description><MusicUrl><![CDATA[{musicUrl}]]></MusicUrl><HQMusicUrl><![CDATA[{hqMusicUrl}]]></HQMusicUrl><ThumbMediaId><![CDATA[{mediaId}]]></ThumbMediaId></Music></xml>";
            }
            if (WeixinConstant.TYPE_NEWS.Equals(weixinVO.MsgType))
            {
                messageTmpl = "<xml><ToUserName><![CDATA[{toUser}]]></ToUserName><FromUserName><![CDATA[{fromUser}]]></FromUserName><CreateTime>{createTime}</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>{articleCount}</ArticleCount><Articles><item><Title><![CDATA[{newsTitle}]]></Title><Description><![CDATA[{newsDescription}]]></Description><PicUrl><![CDATA[{newsPicUrl}]]></PicUrl><Url><![CDATA[{newsUrl}]]></Url></item></Articles></xml> ";
            }
            if (WeixinConstant.TYPE_LOCATION.Equals(weixinVO.MsgType))
            {
                messageTmpl = "";
            }
            if (WeixinConstant.TYPE_LINK.Equals(weixinVO.MsgType))
            {
                messageTmpl = "";
            }
            if (WeixinConstant.TYPE_EVENT.Equals(weixinVO.MsgType))
            {
                messageTmpl = "";
            }
            return messageTmpl;
        }
        /// <summary>
        /// 获取消息推送模板
        /// </summary>
        /// <param name="weixinVO"></param>
        /// <returns></returns>
        public static string getPushMessageTmpl(WeixinVO weixinVO)
        {
            string messageTmpl = null;
            //文本消息
            if (WeixinConstant.TYPE_TEXT.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"text\",\"text\":{\"content\":\"{content}\"}}";
            }
            //图片消息
            if (WeixinConstant.TYPE_IMAGE.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"{mediaId}\"}}";
            }
            //语音消息
            if (WeixinConstant.TYPE_VOICE.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"{mediaId}\"}}";
            }
            //视频消息
            if (WeixinConstant.TYPE_VIDEO.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"{mediaId}\",\"title\":\"{title}\",\"description\":\"{description}\"}}";
            }
            //音乐消息
            if (WeixinConstant.TYPE_MUSIC.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"music\",\"music\":{\"title\":\"MUSIC_TITLE\",\"description\":\"MUSIC_DESCRIPTION\",\"musicurl\":\"{musicUrl}\",\"hqmusicurl\":\"{hqMusicUrl}\",\"thumb_media_id\":\"{mediaId}\"}}";
            }
            //图文消息
            if (WeixinConstant.TYPE_NEWS.Equals(weixinVO.MsgType))
            {
                messageTmpl = "{\"touser\":\"{toUser}\",\"msgtype\":\"news\",\"news\":{\"articles\": [{\"title\":\"{newsTitle}\",\"description\":\"{newsDescription}\",\"url\":\"{newsUrl}\",\"picurl\":\"{newsPicUrl}\"}]}";
            }
            return messageTmpl;
        }
        /// <summary>
        /// HS编码模板转换为内容
        /// </summary>
        /// <param name="stcode"></param>
        /// <returns></returns>
        public static string covtStcode2PostStr(Stcodes stcode)
        {
            string messageTmpl = BIANMA_TMPL; 
            messageTmpl = Regex.Replace(messageTmpl, "\\{code_ts\\}", stcode.code_ts==null?"":stcode.code_ts);
            messageTmpl = Regex.Replace(messageTmpl,"\\{g_name\\}", stcode.g_name==null?"":stcode.g_name);
            messageTmpl = Regex.Replace(messageTmpl,"\\{control\\}", stcode.control==null?"":stcode.control);
            messageTmpl = Regex.Replace(messageTmpl, "\\{tax_type\\}", stcode.tax_type == null ? "" : stcode.tax_type);
            messageTmpl = Regex.Replace(messageTmpl,"\\{unit_no\\}", stcode.unit_no==null?"":stcode.unit_no);
            messageTmpl = Regex.Replace(messageTmpl,"\\{sunit_no\\}", stcode.sunit_no==null?"":stcode.sunit_no);
            messageTmpl = Regex.Replace(messageTmpl,"\\{note_s\\}", stcode.note_s==null?"":stcode.note_s.Trim());
            messageTmpl = Regex.Replace(messageTmpl,"\\{low_rate\\}", stcode.low_rate==null?"":(Double.Parse(stcode.low_rate)>0?(Double.Parse(stcode.low_rate)*100).ToString()+"%":"0"));
            messageTmpl = Regex.Replace(messageTmpl, "\\{high_rate\\}", stcode.high_rate == null ? "" : (Double.Parse(stcode.high_rate) > 0 ? (Double.Parse(stcode.high_rate) * 100).ToString() + "%" : "0"));
            messageTmpl = Regex.Replace(messageTmpl, "\\{tax_rate\\}", stcode.tax_rate == null ? "" :(Double.Parse(stcode.tax_rate)>0?(Double.Parse(stcode.tax_rate)*100).ToString()+"%":"0"));
            return messageTmpl;
        }
        /// <summary>
        /// 有效期模板转换为内容
        /// //华为技术有限公司|4403950010|自理报关|0|2040-4-9|2014-4-11
        /// </summary>
        /// <param name="companie"></param>
        /// <returns></returns>
        public static string covtYouxq2PostStr(Companies companie)
        {
            string messageTmpl = YOUXIAOQI_TMPL;
            messageTmpl = Regex.Replace(messageTmpl, "\\{name\\}", companie.znamec == null ? "" : companie.znamec);
            messageTmpl = Regex.Replace(messageTmpl, "\\{code\\}", companie.zucode == null ? "" : companie.zucode);
            messageTmpl = Regex.Replace(messageTmpl, "\\{com_type\\}", companie.com_type == null ? "" : companie.com_type);
            messageTmpl = Regex.Replace(messageTmpl, "\\{com_Level\\}", companie.com_Level == null ? "" : WeixinUtils.converLevel(companie.com_Level));
            messageTmpl = Regex.Replace(messageTmpl, "\\{limit_dt\\}", companie.limit_dt == null ? "" : ((DateTime)companie.limit_dt).ToString("yyyy-MM-dd"));
            messageTmpl = Regex.Replace(messageTmpl, "\\{exp_dt\\}", companie.exp_dt == null ? "" : ((DateTime)companie.exp_dt).ToString("yyyy-MM-dd"));
            return messageTmpl;
        }

        /// <summary>
        /// 客户每日小结模板转换为内容
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string covtDailyCounter2PostStr(int conditionKind, object[] result,string apply_dt, string apply_dt2)
        {
            string messageTmpl = CUST_TOTAL_TMPL;
            if(conditionKind == WeixinConstant.CLAUSE_ORG_DAILY){
                messageTmpl = ORG_DAILY_TMPL;
            }
            else if (conditionKind == WeixinConstant.CLAUSE_ORG_DAILY)
            {
                messageTmpl = ORG_DAILY_TMPL;
            }
            else if (conditionKind == WeixinConstant.CLAUSE_ORG_WEEK)
            {
                messageTmpl = ORG_WEEK_TMPL;
            }
            else if (conditionKind == WeixinConstant.CLAUSE_ORG_MONTH)
            {
                messageTmpl = ORG_MONTH_TMPL;
            }
            string now_time = DateTime.Now.ToString("yyyy-MM-dd日HH:mm分");
            messageTmpl = Regex.Replace(messageTmpl, "\\{cust_name\\}", (string)result[1]);
            messageTmpl = Regex.Replace(messageTmpl, "\\{decl_name\\}", (string)result[2]); 
            messageTmpl = Regex.Replace(messageTmpl, "\\{apply_dt\\}", apply_dt);
            messageTmpl = Regex.Replace(messageTmpl, "\\{apply_dt2\\}", apply_dt2);
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_total\\}", (int)result[3]+"");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_exp\\}", (int)result[4] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_imp\\}", (int)result[5] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_trans_imp\\}", (int)result[6] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_trans_exp\\}", (int)result[7] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_locat\\}", (int)result[8] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_pass\\}", (int)result[9] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_unpass\\}", (int)result[10] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_hgcheck\\}", (int)result[11] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{count_sjcheck\\}", (int)result[12] + "");
            messageTmpl = Regex.Replace(messageTmpl, "\\{now_time\\}", now_time);
            messageTmpl = Regex.Replace(messageTmpl, "\\{list_check\\}", "".Equals((string)result[13]) ? "无" : (string)result[13]);
            messageTmpl = Regex.Replace(messageTmpl, "\\{list_unpass\\}", "".Equals((string)result[14]) ? "无" : (string)result[14]);
            return messageTmpl;
        }
        /// <summary>
        /// 报关单状态模板转换为内容
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string covtBaoguandan2PostStr(string result)
        {
            //string[] dan = result.Split('|');
            //string messageTmpl = "报关单号：{applyno}\n";
            //messageTmpl.Replace("\\{applyno\\}", dan[0]);
            return result;
        }
    }
}