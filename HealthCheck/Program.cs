using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net.Config;
using System.Configuration;
using System.IO;

namespace HealthCheck
{
    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var urls = ConfigurationManager.AppSettings["serverUrls"]
                .Split(';')
                .Select(url=>url.Replace(Environment.NewLine,"").Trim())
                .ToList();

            var healthStatusList = new List<HealthStatus>();

            foreach (var url in urls)
            {
                var healthStatus = new RestClient().GetHealthStatus(url);

                healthStatusList.Add(healthStatus);

                Console.WriteLine(healthStatus.ToString());
            }

            if (healthStatusList.Any(status => status.StatusCode != HttpStatusCode.OK))
            {
                var htmlNewLine = "<br />";
                var report = htmlNewLine + string.Join(htmlNewLine, healthStatusList.Select(s => s.ToString()));
                var currentlyDownServers = healthStatusList.Where(status => status.StatusCode != HttpStatusCode.OK).Select(s => s.Url).ToList();

                var currentlydownLogFile = Path.Combine(Environment.CurrentDirectory, "CurrentlyDown.log");
                if (args.Length == 0 || args[0] != "test")
                {
                    var previouslyDownServers = File.Exists(currentlydownLogFile) ? File.ReadAllLines(currentlydownLogFile) : new string[0];
                    if (currentlyDownServers.Any(item => !previouslyDownServers.Contains(item)))
                    {
                        new EmailClient().Send(report);
                    }
                }
                File.WriteAllLines(currentlydownLogFile, currentlyDownServers);

                Logger.Error(report.Replace(htmlNewLine, Environment.NewLine));
            }
            else
            {
                Logger.Info("All OK");
            }
        }
    }
}