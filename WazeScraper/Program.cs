using MySql.Data.MySqlClient;
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

                string cs = @"server=88.119.198.18;userid=rent_AmSlab;password=password;database=rent_AmSlab";

                using var con = new MySqlConnection(cs);
                con.Open();
                try
                {
                    apiClient.InsertPayloadCommand(alerts, con);
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
