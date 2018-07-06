using System;
using System.Collections.Generic;
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

        private Dictionary<string, BasePub> Pubs = new Dictionary<string, BasePub>();

        //构造函数
        public WebSocketServer()
        {

        }

        public async Task AddClient(string ip, int port, string sessionId, WebSocket webSocket, HttpContext context)
        {
            var route = context.Request.Path.ToString().ToLower();
            var client = new SocketClient.SocketClient()
            {
                Ip = ip,
                Port = port,
                SessionId = sessionId,
                Socket = webSocket,
                Context = context,
                Route = route
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
            SocketClientMgr.Instance.Remove(e.Arg1, e.Arg2);
            foreach (var item in Pubs.Values)
            {
                item.RaiseClose(e.Arg2);
            }
        }

        private void Client_OnConnect(object sender, DataEventArgs<string, SocketClient.SocketClient> e)
        {
            foreach (var item in Pubs.Values)
            {
                item.RaiseConnect(e.Arg2);
            }
            Console.WriteLine(e.Arg1);
        }

        private void Client_OnReceive(object sender, DataEventArgs<string, SocketClient.SocketClient> e)
        {
            var path = e.Arg2.Context.Request.Path;
            var pubkey = (path.HasValue ? path.Value : "").Replace("/", "");
            if (!string.IsNullOrWhiteSpace(pubkey))
            {
                var pub = Pubs[pubkey];
                if (pub != null)
                {
                    pub.RaiseReveive(e.Arg2, e.Arg1);
                }
                Console.WriteLine(e.Arg1);
                return;
            }

            foreach (var item in Pubs.Values)
            {
                item.RaiseReveive(e.Arg2, e.Arg1);
            }
            Console.WriteLine(e.Arg1);
        }

        /// <summary>
        /// 初始化集线器
        /// </summary>
        internal void InitPubs()
        {
            Pubs = new Dictionary<string, BasePub>();
        }

        /// <summary>
        /// 注册集线器
        /// </summary>
        /// <param name="pub"></param>
        internal void RegPub(BasePub pub)
        {
            if (!Pubs.ContainsKey(pub.Route.ToLower()))
            {
                Pubs.Add(pub.Route.ToLower(), pub);
            }
            else
            {
                throw new Exception($"the pub {pub.Route} had added, please check your code! ");
            }
        }
    }
}
