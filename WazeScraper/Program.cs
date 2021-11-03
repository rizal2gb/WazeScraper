using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WazeScraper
{
    class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        static async Task Main()
        {
            try
            {
                SetupLogger();
                Logger.Info("Starting...");
                AutofacInitializer.Initialize();
                var scraper = AutofacInitializer.GetInstance<Scraper>();
                await scraper.Start();

                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    QuitEvent.Set();
                    eArgs.Cancel = true;
                };

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Logger.Info($"Application has crashed, Exception: {e.Message}\n StackTrace: \n {e.StackTrace}");
            }
        }

        private static void SetupLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new FileTarget
            {
                FileName = "Logs-${shortdate}.log",
                Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}"
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;
        }
    }
}
