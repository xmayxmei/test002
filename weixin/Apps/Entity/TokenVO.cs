using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin.Apps.Entity
{
    /// <summary>
    /// token实体
    /// </summary>
    public class TokenVO
    {
        public string token { get; set; }
        public string appId { get; set; }
        public string appSecret{ get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}