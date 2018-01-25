using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core.SocketServer;
using Microsoft.AspNetCore.Builder;
using Engine.Common;
using Microsoft.AspNetCore.Http;

namespace Engine.Core.Kernel
{
    public static class KernelExt
    {

        public static void UseWebSocketKernel(this IApplicationBuilder app)
        {
            Kernel.CreateKernel(app);
        }


    }

    public class Kernel : IKernel
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
            _server.RegPub((BasePub)instance);
        }
    }
}
