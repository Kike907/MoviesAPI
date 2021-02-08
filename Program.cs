using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry.Extensibility;

namespace MoviesAPI
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
                    //webBuilder.UseSentry(Environment.GetEnvironmentVariable("SENTRY_DSN"));
                    webBuilder.UseSentry(o => {
                        o.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
                        o.Debug = true;
                        o.MaxRequestBodySize = RequestSize.Always;
                        o.Debug = true;
                        o.SendDefaultPii = true;
                        o.IncludeActivityData =  true;
                    
                       });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
