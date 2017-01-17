using Engine.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace AspNetCoreEngine
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Cfg.Init();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://10.101.48.25:5000/")
                .Build();

            host.Run();
            Call();

        }

        private static void Call()
        {
            var str = Console.ReadLine();
            Console.WriteLine(str);
            Call();
        }
    }
}
