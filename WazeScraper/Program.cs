using MySql.Data.MySqlClient;
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

            string cs = @"server=88.119.198.18;userid=rent_AmSlab;password=pass;database=rent_AmSlab";

            using var con = new MySqlConnection(cs);
            con.Open();
            Console.WriteLine("nice");
        }
    }
}
