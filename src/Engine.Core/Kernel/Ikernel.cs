using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core.Kernel
{
    public interface IKernel
    {
        /// <summary>
        /// 注册集线器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RegPubs<T>() where T : BasePub;

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SocketClient.SocketClient GetClient(string name);

        /// <summary>
        /// 发给 某个人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        Task SendTo(string name, string msg);

        /// <summary>
        /// 发给客户端
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendTo(SocketClient.SocketClient name, string msg);

        /// <summary>
        /// 发给所有人
        /// </summary>
        /// <param name="msg"></param>
        Task SendAll(string msg);
    }
}
