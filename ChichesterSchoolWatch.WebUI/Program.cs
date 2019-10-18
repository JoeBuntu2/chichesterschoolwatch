using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting; 

namespace ChichesterSchoolWatch.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #if DEBUG
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            #endif

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
