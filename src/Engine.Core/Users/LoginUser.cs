using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core.Users
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// sessionId
        /// </summary>
        public string SessionId { get;  set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get;  set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public object Ext { get; set; }

        /// <summary>
        /// tag
        /// </summary>
        public object Tag { get; set; }
    }
}
