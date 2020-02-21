using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Grinderofl.GenericSearch.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            EntityFrameworkProfiler.Initialize();
#endif
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
