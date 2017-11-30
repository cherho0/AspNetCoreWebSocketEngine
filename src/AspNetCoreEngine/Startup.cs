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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDistributedMemoryCache();
            services.AddSession(x =>
            {
                x.CookieName = "wsapp";
                x.IdleTimeout = TimeSpan.FromMinutes(20);
            });
            services.AddMvc();
            services.AddMemoryCache();

            //瞬时
            services.AddTransient<ICommonService, CommonService>();


            //作用域
            //services.AddScoped<ICommonService, CommonService>();

            //单例
            //services.AddSingleton<ICommonService, CommonService>();
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
            LogService.LogInfo("sys init");
            LogService.LogInfo("sys init1");
            LogService.LogInfo("sys init2");
            LogService.LogInfo("sys init3");

            //启用session
            app.UseSession();

            //获取缓存服务
            var cacheHelper = app.ApplicationServices.GetService<IMemoryCache>();
            //获取session服务
            var sessionHelper = app.ApplicationServices.GetService<ISession>();

           



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

            //使用服务
            var commonService = app.ApplicationServices.GetService<ICommonService>();
            var str = commonService.GetWorld();
            Console.WriteLine(str);


            //注册websocket服务
            app.UseWebSocketKernel();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}

