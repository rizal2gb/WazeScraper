using System;
using System.Threading;

namespace WazeScraper
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main()
        {
            try
            {
                Console.WriteLine("Starting...");
                AutofacInitializer.Initialize();
                var scraper = AutofacInitializer.GetInstance<Scraper>();
                scraper.Start();

                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    _quitEvent.Set();
                    eArgs.Cancel = true;
                };

                // kick off asynchronous stuff 

                _quitEvent.WaitOne();

                // cleanup/shutdown and quit


            }
            catch (Exception e)
            {
                Console.WriteLine("this sucks");
                Console.WriteLine(e);
            }
        }
    }
}
