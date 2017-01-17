using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Engine.Common;
using Engine.Core.SocketClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Engine.Core.SocketServer
{
    public class WebSocketServer
    {
        private IApplicationBuilder _app;

        //构造函数
        public WebSocketServer()
        {

        }

        public async Task AddClient(string ip, int port, string sessionId, WebSocket webSocket, HttpContext context)
        {
            var client = new SocketClient.SocketClient()
            {
                Ip = ip,
                Port = port,
                SessionId = sessionId,
                Socket = webSocket,
                Context = context
            };

            SocketClientMgr.Instance.AddClient(client);
            client.OnReceive += Client_OnReceive;
            client.OnConnect += Client_OnConnect;
            client.OnClose += Client_OnClose;
            await client.StartReceive();
        }

        private void Client_OnClose(object sender, DataEventArgs<string, SocketClient.SocketClient> e)
        {
            Console.WriteLine(e.Arg1);
            SocketClientMgr.Instance.Remove(e.Arg1);
        }

        private void Client_OnConnect(object sender, DataEventArgs<string, SocketClient.SocketClient> e)
        {
            Task.Factory.StartNew(async () =>
            {
                await SocketClientMgr.Instance.SendAll("test");
            });
            Console.WriteLine(e.Arg1);
        }

        private void Client_OnReceive(object sender, DataEventArgs<string, SocketClient.SocketClient> e)
        {
            Task.Factory.StartNew(async () =>
            {
                await e.Arg2.SendMsg(e.Arg1);
            });
            Console.WriteLine(e.Arg1);
        }
    }
}
