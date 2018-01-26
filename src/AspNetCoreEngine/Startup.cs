using Engine.Core.SocketServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Common;
using Engine.Core.Kernel;
using TestServices;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Caching.Memory;
using AspNetCoreEngine.Filter;
using TestServices.Pubs;

namespace AspNetCoreEngine
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceCollection Services;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            // Add framework services.
            services.AddDistributedMemoryCache();
            services.AddSession(x =>
            {
                x.CookieName = "wsappid";
                x.IdleTimeout = TimeSpan.FromHours(9999);
            });
            services.AddMvc();
            services.AddMemoryCache();
            
        }

        private WebSocketServer _server;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 配置入口
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            //Echo
            LogService.Init(loggerFactory.CreateLogger(typeof(Startup)));

            //启用session
            app.UseSession();
            
            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            IKernel knl = null;

            //注册websocket服务
            knl = app.UseWebSocketKernel();
            Global.Kernel = knl;

            knl.RegPubs<FriendsPub>();
            knl.RegPubs<ChatPub>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Chat}/{action=Index}/{id?}");
            });
        }


    }
}

