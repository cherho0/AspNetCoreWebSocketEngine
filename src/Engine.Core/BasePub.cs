using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core.Kernel;
using Engine.Core.SocketClient;
using Engine.Core.Users;

namespace Engine.Core
{
    /// <summary>
    /// 集线器基类
    /// </summary>
    public abstract class BasePub
    {

        protected IKernel Knl { get; private set; }

        internal void SetKnl(IKernel knl)
        {
            Knl = knl;
        }

        /// <summary>
        /// 发给某人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        protected async void SendTo(string name, string msg)
        {
            await Knl.SendTo(name, msg);
        }

        protected async void SendAll(string msg)
        {
            await Knl.SendAll(msg);
        }

        /// <summary>
        /// 集线器路由
        /// </summary>
        public abstract string Route { get; protected set; }

        protected virtual void OnLoad(IKernel knl)
        {

        }

        /// <summary>
        /// 客户端 连接
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnConnected(SocketClient.SocketClient client)
        {

        }

        /// <summary>
        /// 客户端关闭
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientClosed(SocketClient.SocketClient client)
        {

        }

        /// <summary>
        /// 收到消息
        /// </summary>
        protected virtual void RecvMsg(SocketClient.SocketClient client, string msg)
        {

        }

        internal void RaiseClose(SocketClient.SocketClient client)
        {
            OnClientClosed(client);
        }

        internal void RaiseConnect(SocketClient.SocketClient client)
        {
            OnConnected(client);
        }

        internal void RaiseReveive(SocketClient.SocketClient client, string msg)
        {
            RecvMsg(client, msg);
        }
    }
}
