using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Common;
using Engine.Core.Users;
using Microsoft.AspNetCore.Http;

namespace Engine.Core.SocketClient
{
    public class SocketClient
    {
        /// <summary>
        /// ip
        /// </summary>
        public string Ip { get; set; }

        public int Port { get; set; }

        public string SessionId { get; set; }

        public WebSocket Socket { get; set; }
        public HttpContext Context { get; set; }

        public LoginUser User
        {
            get
            {
                return UserMgr.GetUserBySId(SessionId);
            }
        }

        /// <summary>
        /// 当收到消息
        /// </summary>
        public event EventHandler<DataEventArgs<string, SocketClient>> OnReceive;

        public event EventHandler<DataEventArgs<string, SocketClient>> OnClose;

        public event EventHandler<DataEventArgs<string, SocketClient>> OnConnect;

        public async Task SendMsg(string msg)
        {
            var buffer = Encoding.UTF8.GetBytes(msg);
            await Socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task StartReceive()
        {
            var pkgsize = Cfg.GetCfg<int>("PackageSize");
            var buffer = new byte[1024 * pkgsize];
            OnOnConnect(new DataEventArgs<string, SocketClient>(SessionId, this));
            var result = await Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    OnOnReceive(new DataEventArgs<string, SocketClient>(msg, this));
                }
                result = await Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await Socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            OnOnClose(new DataEventArgs<string, SocketClient>(SessionId, this));
        }



        protected virtual void OnOnReceive(DataEventArgs<string, SocketClient> e)
        {
            OnReceive?.Invoke(this, e);
        }

        protected virtual void OnOnClose(DataEventArgs<string, SocketClient> e)
        {
            OnClose?.Invoke(this, e);
        }

        protected virtual void OnOnConnect(DataEventArgs<string, SocketClient> e)
        {
            OnConnect?.Invoke(this, e);
        }

        public async Task Close()
        {
            await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, Socket.CloseStatusDescription, CancellationToken.None);
            OnOnClose(new DataEventArgs<string, SocketClient>(SessionId, this));
        }
    }
}
