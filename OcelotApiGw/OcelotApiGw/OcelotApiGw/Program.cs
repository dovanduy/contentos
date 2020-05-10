using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile("ocelot.json", false, false)
                       .AddEnvironmentVariables();
               })
               .ConfigureServices(s =>
               {
                   s.AddOcelot().AddEureka().AddCacheManager(x => x.WithDictionaryHandle());
               }
               )
               .Configure(a =>
               {
                   a.UseOcelot().Wait();
               }
               );
    }
}
