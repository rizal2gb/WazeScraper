using System.IO;
using System.Net;
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
    }
}
