﻿using MySql.Data.MySqlClient;
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
        public string Get(HttpWebRequest request)
        {
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public async void InsertPayloadCommand(List<WazeAlert> alerts, MySqlConnection connection)
        {
            StringBuilder insertValues = new StringBuilder("");
            int size = alerts.Count;

            using (MySqlCommand cmd = new MySqlCommand())
            {
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
                        insertValues.Append(",");
                    }
                }

                cmd.Connection = connection;
                cmd.CommandText = $"{Constants.InsertQuery}{insertValues}";
                await cmd.ExecuteNonQueryAsync();
            }
        }


    }
}