using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Weixin.Controllers;
using System.Timers;

namespace Weixin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //定时任务
            TaskSchedule();
        }

        void TaskSchedule()
        {
            //定义定时器 
            //1000表示1秒的意思 
            System.Timers.Timer myTimer = new System.Timers.Timer(1000*60*10);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(sendMessageRepeatTimer);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
            
        }
        void sendMessageRepeatTimer(object source, ElapsedEventArgs e)
        {
            DateTime nowTime = DateTime.Now;
            int nowHour = nowTime.Hour;
            int nowMinute = nowTime.Minute;
            //18.55-19.05
            if ((nowHour == 18 && nowMinute > 55) || (nowHour == 19 && nowMinute < 5))
            {
                TaskController.instance.sendMessageRepeat();
            }
        }
    }
}