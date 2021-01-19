using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WazeScraper.Helpers;
using WazeScraper.Models;
using WazeScraper.Utils;

namespace WazeScraper
{
    [AppScope(Scope.SingleInstance)]
    public class Scraper
    {
        private readonly ApiClient _apiClient;
        private bool _runLoop;

        public bool IsRunning => _runLoop;

        public Scraper(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void Start()
        {
            _runLoop = true;
            Console.WriteLine("Running...");
            RunLoop();
        }

        public void Stop()
        {
            _runLoop = false;
        }

        private async void RunLoop()
        {
            while (_runLoop)
            {
                Console.WriteLine("Delaying task for 15 seconds...");
                await Task.Delay(15000); // waits 15 seconds
                try
                {
                    var list = await GetListOfAlertsByType(Constants.x_start, Constants.x_end, Constants.y_start, Constants.y_end, Constants.SearchedType);
                    if (list == null || list.Count == 0)
                        continue;

                    var start_payload = DateTime.Now;
                    _apiClient.InsertPayloadCommand(list);
                    var end_payload = DateTime.Now;

                    Console.WriteLine($"[LOG] _apiClient.InsertPayloadCommand took: {end_payload - start_payload}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public async Task<List<WazeAlert>> GetListOfAlertsByType(int x_start, int x_end, int y_start, int y_end, string type)
        {
            Console.WriteLine("Starting Task...");
            var startTime = DateTime.Now;
            List<WazeAlert> wantedAlerts = new List<WazeAlert>();

            for (int x = x_start; x <= x_end; x++)
            {
                for (int y = y_start; y <= y_end; y++)
                {
                    var request = RequestHelper.CreateValidRequest(x, y, y + 1, x + 1);
                    try
                    {
                        var start_time_try = DateTime.Now;
                        var response = await _apiClient.GetAsync(request);
                        var end_time_response = DateTime.Now;
                        var alerts = JsonHelper.DeserializeResponse(response);
                        var end_time_alerts = DateTime.Now;

                        Console.WriteLine($"[LOG] _apiclient.Get took:  {end_time_response - start_time_try}");
                        Console.WriteLine($"[LOG] Json desializer took: {end_time_alerts - end_time_response}");

                        if (alerts == null || alerts.Count == 0)
                            continue;

                        foreach (var alert in alerts)
                        {
                            if (alert.Type == type && alert.Country == "LH")
                                wantedAlerts.Add(alert);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }
                }
            }

            var endTime = DateTime.Now;
            Console.WriteLine($"Task finished, it took: {endTime - startTime}");

            return wantedAlerts;
        }
    }
}
