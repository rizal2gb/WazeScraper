using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WazeScraper.Models;

namespace WazeScraper.Helpers
{
    public static class JsonHelper
    {
        public static List<WazeAlert> DeserializeResponse(string response)
        {
            JObject rawJson = JObject.Parse(response);
            var alerts = rawJson["alerts"];
            if (alerts == null)
                return null;

            var deserializedAlerts = new List<WazeAlert>();
            foreach (var alert in alerts)
            {
                deserializedAlerts.Add(alert.ToObject<WazeAlert>());
            }

            return deserializedAlerts;
        }
    }
}
