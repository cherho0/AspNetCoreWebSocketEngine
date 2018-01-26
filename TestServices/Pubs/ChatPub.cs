using Engine.Core;
using Engine.Core.SocketClient;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestServices.Pubs
{
    /// <summary>
    /// 聊天集线器
    /// </summary>
    public class ChatPub : BasePub
    {
        public override string Route => "chat";

        protected override void RecvMsg(SocketClient client, string msg)
        {
            JObject jo = JObject.Parse(msg);
            var cmd = jo["cmd"].ToString();
            var content = jo["msg"];
            switch (cmd)
            {
                case "toall":
                    Knl.SendAll(new {cmd,msg=new { from = client.User.Name, msg= content .ToString()} });
                    break;
                default:
                    break;
            }
        }
    }
}
