using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using bgweb;
using Weixin.Apps.Entity;
using Weixin.Apps.Service;

namespace Weixin.Controllers
{
    public class TaskController : Controller
    {
        static TaskController taskCtl=null;

        TaskController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }
        //批量发送消息
        public void sendMessageRepeat() {
            List<object[]> resultList = null;
            if (resultList != null && resultList.Count > 0) {
                foreach (object[] result in resultList)
                {
                    WeixinVO lowerWeixin = new WeixinVO();
                    lowerWeixin.MsgType = WeixinConstant.TYPE_TEXT;
                    lowerWeixin.ToUserName = (string)result[0];
                    new WeixinPush().pushMessage(lowerWeixin, result);//发送消息
                }
            }
        }

        
        public static TaskController instance
        {
            get
            {
                if (taskCtl == null)
                {
                    taskCtl = new TaskController();
                }
                return taskCtl;
            }
        }
    }
}
