using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core.SocketServer;
using Microsoft.AspNetCore.Builder;
using Engine.Common;
using Microsoft.AspNetCore.Http;
using Engine.Core.SocketClient;
using Engine.Core.Users;
using Newtonsoft.Json;

namespace Engine.Core.Kernel
{
    public static class KernelExt
    {

        /// <summary>
        /// 使用websocket
        /// </summary>
        /// <param name="app"></param>
        public static IKernel UseWebSocketKernel(this IApplicationBuilder app)
        {
            return Kernel.CreateKernel(app);
        }


    }

    internal class Kernel : IKernel
    {
        private WebSocketServer _server;
        private IApplicationBuilder _app;

        internal static IKernel CreateKernel(IApplicationBuilder app)
        {
            IKernel knl = new Kernel(app);
            return knl;
        }
        private Kernel(IApplicationBuilder app)
        {
            _server = new WebSocketServer();
            //初始化参数
            //var size
            _app = app;
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                context.Session.SetString("1", "1");
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var sessionId = context.Session.Id;
                    var ip = context.Connection.RemoteIpAddress.ToString();
                    var port = context.Connection.RemotePort;
                    await _server.AddClient(ip, port, sessionId, webSocket, context);
                }
                else
                {
                    await next();
                }
            });
            BuildBroadCast();
            //
            LogService.LogInfo("Start Up WebSocket Engine");
        }

        /// <summary>
        /// 待创建集线器广播服务  --  诚邀勇士加入
        /// </summary>
        private void BuildBroadCast()
        {
            //注册集线器
            _server.InitPubs();
        }

        public void RegPubs<T>() where T : BasePub
        {
            var instance = System.Activator.CreateInstance<T>();
            instance.SetKnl(this);
            _server.RegPub((BasePub)instance);
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SocketClient.SocketClient> GetClients(string name)
        {
            var user = UserMgr.GetUserByName(name);
            var client = SocketClientMgr.Instance.GetClient(user.SessionId);
            return client;
        }

        /// <summary>
        /// 发送给某人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public async Task SendTo(string name, string msg)
        {
            var cs = GetClients(name);
            if (cs != null && cs.Count > 0)
            {
                foreach (var c in cs)
                {
                    await c.SendMsg(msg);
                }

            }
        }

        /// <summary>
        /// 发送给某人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public async Task SendTo(SocketClient.SocketClient client, string msg)
        {
            await client.SendMsg(msg);
        }

        /// <summary>
        /// 发给所有人
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendAll(string msg)
        {
            await SocketClientMgr.Instance.SendAll(msg);
        }

        public List<List<SocketClient.SocketClient>> GetAllClients()
        {
            return SocketClientMgr.Instance.GetAllClients();
        }

        /// <summary>
        /// 获取所有登录用户
        /// </summary>
        /// <returns></returns>
        public List<LoginUser> GetAllUsers()
        {
            return UserMgr.GetAllUsers();
        }

        /// <summary>
        /// 添加登录人
        /// </summary>
        /// <param name="loginUser"></param>
        public void AddUser(LoginUser loginUser)
        {
            UserMgr.AddUser(loginUser);
        }

        public async Task SendTo(string name, object msg)
        {
            var msgstr = JsonConvert.SerializeObject(msg);
            await SendTo(name, msgstr);
        }

        public async Task SendTo(SocketClient.SocketClient client, object msg)
        {
            var msgstr = JsonConvert.SerializeObject(msg);
            await SendTo(client, msgstr);
        }

        public async Task SendAll(object msg)
        {
            var msgstr = JsonConvert.SerializeObject(msg);
            await SendAll(msgstr);
        }
    }
}
