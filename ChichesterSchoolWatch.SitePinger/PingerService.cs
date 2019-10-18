using System;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading;
using NLog;

namespace ChichesterSchoolWatch.SitePinger
{
    public class PingerService : ServiceBase
    {
        private readonly ILogger Logger;
        private bool IsStopped { get; set; }
        private Timer Timer;


        public PingerService(ILogger logger)
        {
            Logger = logger;
            Timer = new Timer(OnTimerFired, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void ManualStart()
        {
            Logger.Info("Manual Start...");
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Logger.Info("Starting...");
            IsStopped = false;
            new Thread(KeepThreadAlive){IsBackground = false}.Start();

            Timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(2));
        }

        public void KeepThreadAlive(object state)
        {
            while (!IsStopped)
            {
                Thread.Sleep(5000);
            }
        }

        private void OnTimerFired(object state)
        {
            try
            {
                Logger.Info("Executing...");
                var uri = new Uri("https://www.chichesterschoolwatch.com/api/keepalive");
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(45);
                var result = httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead).Result;
                Logger.Info("Result Status Code: " +result.StatusCode);
            }
            catch (Exception e)
            {  
                Logger.Error(e, "Erorr occurred while pinging site.");
            }

        }

        protected override void OnStop()
        {
            Logger.Info("Stopping...");
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Timer.Dispose();
            IsStopped = true;
        }
    }
}
