# AspNetCoreWebSocketEngine -- 基于asp.net core 的websocket引擎 
Asp.Net Core WebSocket Engine
基于这个架构快速创建可以运行于linux服务器的 。net core websocket 服务器，实现即时通信

## 初次使用只需要2行代码，启动你的服务，注册全局的kernel可以方便你随时随地通过kernel获取用户和客户端信息：
            //注册websocket服务
            knl = app.UseWebSocketKernel();
            Global.Kernel = knl;

            knl.RegPubs<FriendsPub>();
            knl.RegPubs<ChatPub>();

            可以类似这样登录：

            public IActionResult Index()
            {
                var User = UserMgr.GetUserBySId(Request.HttpContext.Session.Id);
                if (User == null)
                {
                    return RedirectToAction("login");
                }
                return View(User);
            }

            [HttpPost]
            public IActionResult Login(string name, string password)
            {

                Global.Kernel.AddUser(new LoginUser
                {
                    Name = name,
                    Password = password,
                    SessionId = Request.HttpContext.Session.Id

                });
                return Json(new { ok = true });
            }


## 你可以专注于编写你的pub，类似于signalR的pub，继承于BasePub，去重写相应的方法，用来获取信息或者注册用户：
        
        
        
        
        /// <summary>
        /// 发给某人
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        protected async void SendTo(string name, string msg)
        {
            
        }

        protected async void SendAll(string msg)
        {
            
        }

        /// <summary>
        /// 集线器路由
        /// </summary>
        public abstract string Route { get; }

        protected virtual void OnLoad(IKernel knl)
        {

        }

        /// <summary>
        /// 客户端 连接
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnConnected(SocketClient.SocketClient client)
        {

        }

        /// <summary>
        /// 客户端关闭
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientClosed(SocketClient.SocketClient client)
        {

        }

        /// <summary>
        /// 收到消息
        /// </summary>
        protected virtual void RecvMsg(SocketClient.SocketClient client, string msg)
        {

        }


        提供一个内核
        protected IKernel Knl { get; private set; }

        内核可以获取用户，获取pub

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