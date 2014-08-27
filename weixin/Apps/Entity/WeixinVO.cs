using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// 微信实体对象VO
    /// </summary>
    public class WeixinVO
    {
       public string FromUserName { get; set; }
       public string ToUserName { get; set; }
       public string Content { get; set; }
       public string MsgType { get; set; }
       public string PicUrl { get; set; }
       public string CreateTime { get; set; }
       public string Event { get; set; }
       public string MediaId { get; set; }
       public string MsgId { get; set; }
       public string Format { get; set; }
       public string ThumbMediaId { get; set; }
       public string Location_X { get; set; }
       public string Location_Y { get; set; }
       public string Scale { get; set; }
       public string Label { get; set; }
       public string Title { get; set; }
       public string Description { get; set; }
       public string Url { get; set; }
       public string MusicUrl{ get; set; }
       public string HQMusicUrl { get; set; }
       public string EventKey { get; set; }
       public string Ticket { get; set; }
       public string Latitude { get; set; }//地理位置纬度
       public string Longitude { get; set; }//地理位置经度
       public string Precision { get; set; }//地理位置精度
       public string Recognition { get; set; }//语音识别结果，UTF8编码
       public List<News> NewsList { get; set; }
       //新闻
       public class News {
           public News(string title, string description, string picUrl, string url)
           {
               this.title = title;
               this.description = description;
               this.picUrl = picUrl;
               this.url = url;
           }
           public News()
           {
           }
           public string title;
           public string description;
           public string picUrl;
           public string url;
       }
       //
       public class A { 
       
       }
        
    }
}