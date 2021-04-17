using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WazeScraper.Helpers
{
    public static class RequestHelper
    {
        private static readonly HttpClient Client = new HttpClient();
        // Not using this at this moment, but HttpWebRequests should be retired and HttpClient used.
        public static void SetupDefaultClient()
        {
            Client.DefaultRequestHeaders.Host = Constants.WazeHost;
            Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Constants.DefaultUserAgent));
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.DefaultHeaderAccept));
            Client.DefaultRequestHeaders.Add("X-Requested-With", Constants.DefaultXRequestedWith);
            Client.DefaultRequestHeaders.Referrer = new Uri(Constants.DefaultReferer);
        }

        public static HttpWebRequest CreateValidRequest(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                return null;
            }

            request.Host = Constants.WazeHost;
            request.UserAgent = Constants.DefaultUserAgent;
            request.Accept = Constants.DefaultHeaderAccept;
            request.Headers["X-Requested-With"] = Constants.DefaultXRequestedWith;
            request.Referer = Constants.DefaultReferer;

            return request;
        }

        public static HttpWebRequest CreateValidRequest(int bottom, int left, int right, int top)
        {
            var formattedUrl = FormatUrl(bottom, left, right, top);
            return CreateValidRequest(formattedUrl);
        }

        public static string FormatUrl(int bottom, int left, int right, int top)
        {
            return $"{Constants.URL_Start}bottom={bottom}&left={left}&ma={Constants.MaxAlerts}&right={right}&top={top}{Constants.URL_Type}";
        }
    }
}
