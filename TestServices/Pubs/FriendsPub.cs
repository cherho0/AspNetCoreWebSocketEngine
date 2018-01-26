using Engine.Core;
using Engine.Core.SocketClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestServices.Pubs
{
    public class FriendsPub : BasePub
    {
        public override string Route
        {
            get { return "friends"; }
        }

        protected override void OnConnected(SocketClient client)
        {
            Knl.SendAll(new { cmd="updateuser",
                msg =Knl.GetAllUsers() });
        }
    }
}
