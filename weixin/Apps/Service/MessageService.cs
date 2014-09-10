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
    /// 消息
    /// </summary>
    public class MessageService
    {
        private Models.FeelingEntities mdb = new Models.FeelingEntities();

        /// <summary>
        /// 根据用户查找订单
        /// </summary>
        /// <param name="openId"></param>
        public We_Order findByOpenid(String openid) {
            var query = from o in mdb.We_Order where o.customer == openid select o;
            List<We_Order> orderList = query.ToList();
            if (orderList != null && orderList.Count > 0)
            {
                return orderList.First();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 保存或更新
        /// </summary>
        /// <param name="message"></param>
        public void saveOrUpdate(We_Message message)
        {
            if (message != null)
            {
                if (message.message_id == 0)
                {
                    message.create_time = DateTime.Now;
                    mdb.We_Message.AddObject(message);
                }
                else {
                  //  mdb.We_Order.
                }
                mdb.SaveChanges();
            }
        }

        public void update(We_Order order)
        {
            if (order != null)
            {
                order.order_time = DateTime.Now;
                mdb.We_Order.AddObject(order);
                mdb.SaveChanges();
            }
        }
        
    }
}