using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;

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
        public long PubMillis { get; }

        [JsonConstructor]
        public DbAlert(string id, Location location, string country, string city, int reportRating,
                       int confidence, int reliability, string subtype, long pubMillis)
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

        /// <summary>
        /// NOTE: this might get deprecated due to the usage of ApiClient methods
        /// Takes in DbAlert Object and converts it MySqlCommand used to insert to database
        /// </summary>
        /// <returns>return insert sql command for the object</returns>
        public MySqlCommand InsertToDatabaseCommandCreate()
        {
            MySqlCommand command = new MySqlCommand(Constants.InsertQuery);

            command.Parameters.AddWithValue("@id", Id);
            command.Parameters.AddWithValue("@X", Location.X);
            command.Parameters.AddWithValue("@Y", Location.Y);
            command.Parameters.AddWithValue("@Country", Country);
            command.Parameters.AddWithValue("@City", City);
            command.Parameters.AddWithValue("@ReportRating", ReportRating);
            command.Parameters.AddWithValue("@Confidence", Confidence);
            command.Parameters.AddWithValue("@Reliability", Reliability);
            command.Parameters.AddWithValue("@Subtype", Subtype);
            command.Parameters.AddWithValue("@published", DateTimeOffset.FromUnixTimeMilliseconds(PubMillis).DateTime);

            return command;
        }

    }
}
