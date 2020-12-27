using System;
using System.Linq;

namespace WazeScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting...");
                AutofacInitializer.Initialize();
                var apiClient = AutofacInitializer.GetInstance<ApiClient>();
                var request = RequestHelper.CreateValidRequest(54, 24, 25, 55);
                var response = apiClient.Get(request);
                var alerts = JsonHelper.DeserializeResponse(response);
                var cops = alerts.Where(x => x.Type == "POLICE");
                var test = apiClient.SelectKnownIdsCommand();
                try
                {
                    apiClient.InsertPayloadCommand(alerts);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("Works as expected");
            }
            catch (Exception e)
            {
                Console.WriteLine("this sucks");
                Console.WriteLine(e);
            }

        }
    }
}
