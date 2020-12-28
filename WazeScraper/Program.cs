using System;
using WazeScraper.Helpers;

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
                Console.WriteLine("Running ;)");
            }
            catch (Exception e)
            {
                Console.WriteLine("this sucks");
                Console.WriteLine(e);
            }
        }
    }
}
