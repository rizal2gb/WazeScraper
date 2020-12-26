using Newtonsoft.Json;
using System;

namespace WazeScraper.Models
{
    public class WazeAlert : DbAlert
    {
        public string ReportBy { get; }
        public bool Inscale { get; }
        public bool IsJamUnifiedAlert { get; }
        public int NImages { get; }
        public string Type { get; }
        public Guid Uuid { get; }
        public string NearBy { get; }
        public int Speed { get; }
        public int ReportMood { get; }
        public int RoadType { get; }
        public int Magvar { get; }
        public bool ShowFacebookPic { get; }
        public string Street { get; }
        public string AdditionalInfo { get; }
        public string WazeData { get; }
        public string ReportDescription { get; }

        [JsonConstructor]
        public WazeAlert(string reportBy, string country, bool inscale, bool isJamUnifiedAlert, int reportRating, int confidence, int reliability, int nImages, string type,
                         Guid uuid, string nearBy, int speed, int reportMood, int roadType, int magvar, bool showFacebookPic, string subtype, string street,
                         string additionalInfo, string wazeData, string reportDescription, string id, Location location, long pubMillis, string city) :
                        base(id, location, country, city, reportRating, confidence, reliability, subtype, pubMillis)
        {
            ReportBy = reportBy;
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
            Street = street;
            AdditionalInfo = additionalInfo;
            WazeData = wazeData;
            ReportDescription = reportDescription;
        }
    }
}
