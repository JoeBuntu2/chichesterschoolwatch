using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using NLog;

namespace ChichesterSchoolWatch.SitePinger
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var logger = LogManager.GetCurrentClassLogger(typeof(PingerService));

            if(Environment.UserInteractive)
                new PingerService(logger).ManualStart();
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new PingerService(logger)
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
