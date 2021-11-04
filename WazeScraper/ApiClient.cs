using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WazeScraper.Models;
using WazeScraper.Utils;

namespace WazeScraper
{
    [AppScope(Scope.SingleInstance)]
    public class ApiClient
    {
        private readonly EnvironmentVariableGetter _environmentVariableGetter;
        private string _connectionString;

        public ApiClient(EnvironmentVariableGetter environmentVariableGetter)
        {
            _environmentVariableGetter = environmentVariableGetter;
            GetConnectionString();
        }

        public string Get(HttpWebRequest request)
        {
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            if (stream == null)
            {
                return string.Empty;
            }
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async void InsertPayloadCommand(List<WazeAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0)
                return;

            var insertValues = new StringBuilder("");
            var size = alerts.Count;
            await using var con = new MySqlConnection(GetConnectionString());
            await using var cmd = new MySqlCommand();
            for (int i = 0; i < size; i++)
            {
                var wazeAlert = alerts[i];
                cmd.Parameters.AddWithValue($"@id{i}", wazeAlert.Id);
                cmd.Parameters.AddWithValue($"@X{i}", wazeAlert.Location.X);
                cmd.Parameters.AddWithValue($"@Y{i}", wazeAlert.Location.Y);
                cmd.Parameters.AddWithValue($"@Country{i}", wazeAlert.Country);
                cmd.Parameters.AddWithValue($"@City{i}", wazeAlert.City);
                cmd.Parameters.AddWithValue($"@ReportRating{i}", wazeAlert.ReportRating);
                cmd.Parameters.AddWithValue($"@Confidence{i}", wazeAlert.Confidence);
                cmd.Parameters.AddWithValue($"@Reliability{i}", wazeAlert.Reliability);
                cmd.Parameters.AddWithValue($"@subtype{i}", wazeAlert.Subtype);
                cmd.Parameters.AddWithValue($"@published{i}", DateTimeOffset.FromUnixTimeMilliseconds(wazeAlert.PubMillis).DateTime);

                insertValues.Append($"(@id{i}, @X{i}, @Y{i}, @Country{i}, @City{i}, @ReportRating{i}, @Confidence{i}, @Reliability{i}, @subtype{i}, @published{i})");

                if (i < size - 1)
                {
                    insertValues.Append(", ");
                }
            }

            cmd.Connection = con;
            cmd.CommandText = $"{Constants.InsertQuery}{insertValues}{Constants.OnDuplicateQuery}";
            con.Open();
            await cmd.ExecuteNonQueryAsync();
            await con.CloseAsync();
        }

        /// <summary>
        /// NOTE: we are not using this at this moment, as we don't yet care if items are in DB or not.
        /// Gets all the IDs of events from the database
        /// </summary>
        /// <returns>all IDs of events in DB</returns>
        public HashSet<string> SelectKnownIdsCommand()
        {
            HashSet<string> knownIds = new HashSet<string>();
            using var con = new MySqlConnection(GetConnectionString());
            try
            {
                con.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM `LT-police`", con);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        knownIds.Add(reader["Id"].ToString());
                    }
                }

                return knownIds;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        private string GetConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(_connectionString))
            {
                return _connectionString;
            }
            _connectionString = string.Format(Constants.ConnectionStringFormat,
                _environmentVariableGetter.GetConnectionIp(),
                _environmentVariableGetter.GetConnectionUser(),
                _environmentVariableGetter.GetConnectionPassword(),
                _environmentVariableGetter.GetConnectionDatabase());

            return _connectionString;
        }
    }
}
