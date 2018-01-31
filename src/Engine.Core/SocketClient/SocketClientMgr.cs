using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core.SocketClient
{
    public class SocketClientMgr
    {
        private static object o = new object();

        public static SocketClientMgr Instance
        {
            get
            {
                lock (o)
                {
                    if (_instance == null)
                    {
                        _instance = new SocketClientMgr();
                    }
                    return _instance;
                }
            }
        }

        private static SocketClientMgr _instance = null;

        private Dictionary<string, List<SocketClient>> _sessionclients;

        private SocketClientMgr()
        {
            _sessionclients = new Dictionary<string, List<SocketClient>>();
        }

        public List<SocketClient> GetClient(string sessionid)
        {
            return _sessionclients.ContainsKey(sessionid) ? _sessionclients[sessionid] : null;
        }

        public void AddClient(SocketClient client)
        {
            if (!_sessionclients.ContainsKey(client.SessionId))
            {
                _sessionclients.Add(client.SessionId, new List<SocketClient>());
            }
            _sessionclients[client.SessionId].Add(client);
        }

        public async Task SendAll(string msg)
        {
            foreach (var clients in _sessionclients.Values)
            {
                foreach (var c in clients)
                {
                    await c.SendMsg(msg);

                }
            }
        }

        public async Task SendTo(string sessionid, string msg)
        {
            var clients = _sessionclients[sessionid];
            foreach (var c in clients)
            {
                await c.SendMsg(msg);
            }
        }

        public void Remove(string eArg1, SocketClient c)
        {
            var clients = _sessionclients[eArg1];
            for (int i = clients.Count - 1; i >= 0; i--)
            {
                if (c.Ip == clients[i].Ip && c.Port == clients[i].Port)
                {
                    clients.RemoveAt(i);
                }
            }
            if (clients.Count == 0)
            {
                _sessionclients.Remove(eArg1);
            }

        }

        public async Task CloseAsync(string sessionId)
        {
            var clients = _sessionclients[sessionId];
            _sessionclients.Remove(sessionId);
            foreach (var client in clients)
            {
                await client.Close();
            }

        }

        internal List<List<SocketClient>> GetAllClients()
        {
            return _sessionclients.Values.ToList();
        }
    }
}
