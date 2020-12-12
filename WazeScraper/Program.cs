using System;
using System.Linq;

namespace WazeScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiClient api = new ApiClient();
            Console.WriteLine("Initial commit");
            var request = RequestHelper.CreateValidRequest(54, 24, 25, 55);
            var response = api.Get(request);
            var alerts = JsonHelper.DeserializeResponse(response);
            var cops = alerts.Where(x => x.Type == "POLICE");
            Console.WriteLine("nice");
        }
    }
}
