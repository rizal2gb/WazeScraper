using System;

namespace WazeScraper
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Starting...");
                AutofacInitializer.Initialize();
                var scraper = AutofacInitializer.GetInstance<Scraper>();
                scraper.Start();
                while (scraper.IsRunning) { }

            }
            catch (Exception e)
            {
                Console.WriteLine("this sucks");
                Console.WriteLine(e);
            }
        }
    }
}
