using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core.SocketServer;
using Microsoft.AspNetCore.Builder;

namespace Engine.Core.Kernel
{
    public class Kernel : IKernel
    {
        private WebSocketServer _server;
        private IApplicationBuilder _app;

        public static IKernel CreateKernel(IApplicationBuilder app)
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
        }
    }
}
