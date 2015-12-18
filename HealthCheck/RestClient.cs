using System.Net;

namespace HealthCheck
{
    public class RestClient
    {
        public HealthStatus GetHealthStatus(string url)
        {
            try
            {
                var webRequest = (HttpWebRequest) WebRequest.Create(url);
                webRequest.AllowAutoRedirect = true;
                webRequest.Timeout = 10000;
                using (var response = (HttpWebResponse) webRequest.GetResponse())
                {
                    return new HealthStatus { Url = url, StatusCode = response.StatusCode };
                }
            }
            catch (WebException exception)
            {
                return new HealthStatus { Url = url, ErrorMessage = exception.Message };
            }
        }
    }
}