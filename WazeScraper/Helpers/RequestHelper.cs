using System.Net;

namespace WazeScraper.Helpers
{
    public static class RequestHelper
    {
        public static HttpWebRequest CreateValidRequest(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                return null;

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
