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
    public class UserService
    {
        private Models.FeelingEntities mdb = new Models.FeelingEntities();
        
        /// <summary>
        /// 抓取关注用户信息。
        /// </summary>
        /// <param name="postStr"></param>
        public void grapUser(string openid) {
            WeixinVO weixinVO = new WeixinReceiver().parseMessager(openid);
            new OrderService().findByOpenid(weixinVO.FromUserName);
            
        }
        /// <summary>
        /// 更新关注用户信息
        /// 
        /// </summary>
        /// <param name="openid"></param>
        public void updateUser(string openid) { 
           
        }
       

       
       

        
    }
}