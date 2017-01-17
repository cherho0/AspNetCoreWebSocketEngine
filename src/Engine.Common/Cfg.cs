using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Engine.Common
{
    public static class Cfg
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static T GetCfg<T>(string key)
        {
            return (T)Convert.ChangeType(Configuration[key], typeof(T));
        }

    }
}
