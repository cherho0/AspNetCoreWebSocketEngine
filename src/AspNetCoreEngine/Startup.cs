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
            services.AddSession();
            services.AddMvc();

        }

        private WebSocketServer _server;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            LogService.Init(loggerFactory.CreateLogger("Echo"));
            Kernel.CreateKernel(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /*   //测试代码
        //private async Task MsgReceive(HttpContext context, WebSocket webSocket, ILogger logger)
        //{
        //    var buffer = new byte[1024 * 4];
        //    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //    LogFrame(logger, result, buffer);
        //    while (!result.CloseStatus.HasValue)
        //    {
        //        // If the client send "ServerClose", then they want a server-originated close to occur
        //        string content = "<<binary>>";
        //        if (result.MessageType == WebSocketMessageType.Text)
        //        {
        //            content = Encoding.UTF8.GetString(buffer, 0, result.Count);
        //            if (content.Equals("ServerClose"))
        //            {
        //                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing from Server", CancellationToken.None);
        //                logger.LogDebug($"Sent Frame Close: {WebSocketCloseStatus.NormalClosure} Closing from Server");
        //                return;
        //            }
        //            else if (content.Equals("ServerAbort"))
        //            {
        //                context.Abort();
        //            }
        //        }
               
        //            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                
        //        logger.LogDebug($"Sent Frame {result.MessageType}: Len={result.Count}, Fin={result.EndOfMessage}: {content}");

        //        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        LogFrame(logger, result, buffer);
        //    }
        //    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //}

        //private void LogFrame(ILogger logger, WebSocketReceiveResult frame, byte[] buffer)
        //{
        //    var close = frame.CloseStatus != null;
        //    string message;
        //    if (close)
        //    {
        //        message = $"Close: {frame.CloseStatus.Value} {frame.CloseStatusDescription}";
        //    }
        //    else
        //    {
        //        string content = "<<binary>>";
        //        if (frame.MessageType == WebSocketMessageType.Text)
        //        {
        //            content = Encoding.UTF8.GetString(buffer, 0, frame.Count);
        //        }
        //        message = $"{frame.MessageType}: Len={frame.Count}, Fin={frame.EndOfMessage}: {content}";
        //    }
        //    logger.LogDebug("Received Frame " + message);
        //}
        */
    }
}

