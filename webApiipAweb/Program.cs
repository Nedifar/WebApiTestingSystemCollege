using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseWebRoot("files");
                    webBuilder.UseSentry(o =>
                    {
                        o.Dsn = "https://e8a98c7bcab44af38977740d7a87788a@o4504633413599232.ingest.sentry.io/4504806730039296";
                        o.Debug = true;
                        o.TracesSampleRate = 1.0;
                    });
                });
    }
}
