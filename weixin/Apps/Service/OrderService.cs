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
    /// 订单
    /// </summary>
    public class OrderService
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

        public void saveOrUpdate(We_Order order) {
            if (order != null)
            {
                if (order.order_id == 0)
                {
                    order.order_time = DateTime.Now;
                    mdb.We_Order.AddObject(order);
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