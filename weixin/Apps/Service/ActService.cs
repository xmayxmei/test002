using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using bgweb;
using Weixin.Models;
using System.Text;
using Weixin.Apps.Entity;

namespace Weixin.Apps.Service
{
    /// <summary>
    /// 报关业务数据查询
    /// </summary>
    public class ActService
    {
        private Models.FeelingEntities mdb = new Models.FeelingEntities();
        
        /// <summary>
        /// 收到消息后，验证消息，然后。。
        //  1保存并解析消息，2查找他的那个对象(分2种情况)
        //（如有对象）3、消息过滤，4转发消息。
        //（如无对象）3、系统应答：男，回复查找信息，女，回复加入或管理界面、或保存图片。
        /// </summary>
        /// <param name="postStr"></param>
        public void a(string postStr) {
            //收到消息，保存原始消息。


            //解析消息，更新
            WeixinVO weixinVO = new WeixinReceiver().parseMessager(postStr);
            //保存消息
            We_Message weMessage = new We_Message();
            weMessage.fromUser = weixinVO.FromUserName;
            weMessage.toUser = weixinVO.ToUserName;
            new MessageService().saveOrUpdate(weMessage);
            //查找订单对象
            We_Order we_order = new OrderService().findByOpenid(weixinVO.FromUserName);
            //消息过滤

            //转发(生成)或系统应答

            //存储资源
        }
       

       
       

        
    }
}