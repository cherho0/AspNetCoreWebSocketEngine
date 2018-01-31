using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core.Users;

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
        List<SocketClient.SocketClient> GetClients(string name);

        /// <summary>
        /// 发给 某个人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        Task SendTo(string name, string msg);

        Task SendTo(string name, object msg);

        /// <summary>
        /// 添加登录人
        /// </summary>
        /// <param name="loginUser"></param>
        void AddUser(LoginUser loginUser);

        /// <summary>
        /// 发给客户端
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task SendTo(SocketClient.SocketClient client, string msg);

        Task SendTo(SocketClient.SocketClient client, object msg);

        /// <summary>
        /// 发给所有人
        /// </summary>
        /// <param name="msg"></param>
        Task SendAll(string msg);

        Task SendAll(object msg);

        /// <summary>
        /// 获取所有客户端
        /// </summary>
        /// <returns></returns>
        List<List<SocketClient.SocketClient>> GetAllClients();

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<Users.LoginUser> GetAllUsers();
    }
}
