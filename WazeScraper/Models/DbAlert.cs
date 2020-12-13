using Newtonsoft.Json;

namespace WazeScraper.Models
{
    public class DbAlert
    {
        public string Id { get; }
        public Location Location { get; }
        public string City { get; }
        public string Country { get; }
        public int ReportRating { get; set; }
        public int Confidence { get; set; }
        public int Reliability { get; set; }
        public string Subtype { get; }
        public ulong PubMillis { get; }

        [JsonConstructor]
        public DbAlert(string id, Location location, string country, string city, int reportRating,
                       int confidence, int reliability, string subtype, ulong pubMillis)
        {
            Id = id;
            Location = location;
            City = city;
            Country = country;
            ReportRating = reportRating;
            Confidence = confidence;
            Reliability = reliability;
            Subtype = subtype;
            PubMillis = pubMillis;
        }

        public DbAlert(WazeAlert alert)
        {
            Id = alert.Id;
            Location = alert.Location;
            City = alert.City;
            Country = alert.Country;
            ReportRating = alert.ReportRating;
            Confidence = alert.Confidence;
            Reliability = alert.Reliability;
            Subtype = alert.Subtype;
            PubMillis = alert.PubMillis;
        }

    }
}
