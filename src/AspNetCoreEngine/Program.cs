using Engine.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreEngine
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Cfg.Init();
            Task.Factory.StartNew(Call);
            var port = Cfg.GetCfg<int>("Port");
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:"+ port + "/")
                .Build();

            host.Run();
        }

        private static void Call()
        {
            var str = Console.ReadLine();
            Console.WriteLine(str);
            if (str == "exit")
            {
                Console.WriteLine("over");
            }
            else
            {
                Call();
            }
            
        }
    }
}
