using System;
using System.Threading;
using System.Threading.Tasks;

namespace WazeScraper
{
    class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        static async Task Main()
        {
            try
            {
                Console.WriteLine("Starting...");
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
                Console.WriteLine($"Application has crashed, Exception: {e.Message}\n StackTrace: \n {e.StackTrace}");
            }
        }
    }
}
