using Newtonsoft.Json;
using System;

namespace WazeScraper.Models
{
    public class WazeAlert
    {
        public string ReportBy { get; }
        public string Country { get; }
        public bool Inscale { get; }
        public bool IsJamUnifiedAlert { get; }
        public int ReportRating { get; set; }
        public int Confidence { get; set; }
        public int Reliability { get; set; }
        public int NImages { get; }
        public string Type { get; }
        public Guid Uuid { get; }
        public string NearBy { get; }
        public int Speed { get; }
        public int ReportMood { get; }
        public int RoadType { get; }
        public int Magvar { get; }
        public bool ShowFacebookPic { get; }
        public string Subtype { get; }
        public string Street { get; }
        public string AdditionalInfo { get; }
        public string WazeData { get; }
        public string ReportDescription { get; }
        public string ID { get; }
        public Location Location { get; }
        public ulong PubMillis { get; }

        [JsonConstructor]
        public WazeAlert(string reportBy, string country, bool inscale, bool isJamUnifiedAlert, int reportRating, int confidence, int reliability, int nImages, string type,
                         Guid uuid, string nearBy, int speed, int reportMood, int roadType, int magvar, bool showFacebookPic, string subtype, string street,
                         string additionalInfo, string wazeData, string reportDescription, string id, Location location, ulong pubMillis)
        {
            ReportBy = reportBy;
            Country = country;
            Inscale = inscale;
            IsJamUnifiedAlert = isJamUnifiedAlert;
            ReportRating = reportRating;
            Confidence = confidence;
            Reliability = reliability;
            NImages = nImages;
            Type = type;
            Uuid = uuid;
            NearBy = nearBy;
            Speed = speed;
            ReportMood = reportMood;
            RoadType = roadType;
            Magvar = magvar;
            ShowFacebookPic = showFacebookPic;
            Subtype = subtype;
            Street = street;
            AdditionalInfo = additionalInfo;
            WazeData = wazeData;
            ReportDescription = reportDescription;
            ID = id;
            Location = location;
            PubMillis = pubMillis;
        }
    }
}
