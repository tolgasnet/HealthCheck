using System;
using System.Configuration;
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
                webRequest.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeoutMilliseconds"]);
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