using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core
{
    /// <summary>
    /// 集线器基类
    /// </summary>
    public abstract class BasePub
    {
        protected SocketClient.SocketClientMgr Clients { get; set; }

        /// <summary>
        /// 集线器路由
        /// </summary>
        public abstract string Route { get; protected set; }

        protected abstract void OnConnected(SocketClient.SocketClient client);

        protected abstract void OnClientClosed(SocketClient.SocketClient client);

        /// <summary>
        /// 收到消息
        /// </summary>
        protected abstract void RecvMsg();
    }
}
