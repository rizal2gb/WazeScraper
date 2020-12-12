using Newtonsoft.Json;

namespace WazeScraper.Models
{
    public class Location
    {
        public float X { get; }
        public float Y { get; }

        [JsonConstructor]
        public Location(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
