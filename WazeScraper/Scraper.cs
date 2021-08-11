using System;
using System.Collections.Generic;
using System.Net;
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
        private static readonly TimeSpan DefaultDelayTime = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan CrashDelayTime = TimeSpan.FromSeconds(60);
        private TimeSpan _lastTaskTime = TimeSpan.Zero;

        private TimeSpan DelayTime
        {
            get
            {
                if (_lastTaskTime > DefaultDelayTime)
                {
                    return TimeSpan.Zero;
                }

                return DefaultDelayTime - _lastTaskTime;
            }
        }

        public bool IsRunning => _runLoop;

        public Scraper(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task Start()
        {
            _runLoop = true;
            Console.WriteLine("Running...");
            try
            {
                await RunLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await Restart();
            }
        }


        private async Task Restart()
        {
            Console.WriteLine($"Main Loop crashed, trying to restart in {CrashDelayTime} seconds");
            await Task.Delay(CrashDelayTime);
            await Start();
        }

        public void Stop()
        {
            _runLoop = false;
        }

        private async Task RunLoop()
        {
            while (_runLoop)
            {
                Console.WriteLine($"Delaying task for {DelayTime.TotalSeconds:F}s...");
                await Task.Delay(DelayTime);
                try
                {
                    var list = GetListOfAlertsByType(Constants.x_start, Constants.x_end, Constants.y_start, Constants.y_end, Constants.SearchedType);
                    if (list == null || list.Count == 0)
                    {
                        continue;
                    }

                    _apiClient.InsertPayloadCommand(list);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public List<WazeAlert> GetListOfAlertsByType(int x_start, int x_end, int y_start, int y_end, string type)
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
                        var alerts = GetAlerts(request);

                        if (alerts == null || alerts.Count == 0)
                        {
                            continue;
                        }

                        for (var index = 0; index < alerts.Count; index++)
                        {
                            var alert = alerts[index];
                            if (alert.Type == type && alert.Country == Constants.CountryCode)
                            {
                                wantedAlerts.Add(alert);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }
                }
            }

            _lastTaskTime = DateTime.Now - startTime;
            Console.WriteLine($"Task finished, it took: {_lastTaskTime.TotalSeconds:F}s");

            return wantedAlerts;
        }

        private List<WazeAlert> GetAlerts(HttpWebRequest request)
        {
            var response = _apiClient.Get(request);
            if (string.IsNullOrWhiteSpace(response))
            {
                return null;
            }

            var alerts = JsonHelper.DeserializeResponse(response);
            return alerts;
        }
    }
}
