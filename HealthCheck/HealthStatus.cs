using System.Net;

namespace HealthCheck
{
    public class HealthStatus
    {
        public string Url { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            var statusMessage = HasErrors() ? ErrorMessage : StatusCode.ToString();
            return $"{Url} | {statusMessage}";
        }

        private bool HasErrors()
        {
            return !string.IsNullOrEmpty(ErrorMessage);
        }
    }
}