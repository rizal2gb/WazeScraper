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
                AppDomain.CurrentDomain.ProcessExit += (o, e) => QuitEvent.Set();
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => LogException(args.ExceptionObject as Exception);
                SetupLogger();
                Logger.Info("Starting...");
                AutofacInitializer.Initialize();
                var scraper = AutofacInitializer.GetInstance<Scraper>();
                await scraper.Start();

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Application has crashed");
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

        private static void LogException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            Logger.Fatal(ex, "Unhandled exception occured");
        }
    }
}
