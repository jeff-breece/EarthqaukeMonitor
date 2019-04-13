using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Earthquake.Function.Service;
using Earthquake.Function.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace Earthquake.Function
{
    public static class TimerTriggerCSharp
    {
        // Reminder: run this command on Ubuntu: azurite -l /home/jeff/Azurite/

        // Problem with the return value binding - looking into this
        [FunctionName("EarthQuakeTimerTrigger")]
        public static async Task<String> GetQuakesBySchedule([TimerTrigger("1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Earth Quake Timer trigger function executed at: {DateTime.Now}");
            var repository = new EarthQuakeRepository();
            var quakeTask = await repository.GetAsync(CancellationToken.None);
            var quakes = Task.FromResult(quakeTask);
            return quakes.IsCompleted.ToString();
        }

        [FunctionName("EarthQuakeManualTrigger")]
        public static async Task<string> GetQuakesByManualTrigger(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var repository = new EarthQuakeRepository();
            var quakeTask = await repository.GetAsync(CancellationToken.None);
            var quakes = Task.FromResult<Properties[]>(quakeTask);
            /* foreach(Properties quake in quakes)
            {
                RecordData(quake);
            } */
            return quakes.ToString();
        }
        public static bool RecordData(Properties input)
        {
            return true;
        }
    }

    public class EarthQuakeEntity : TableEntity
    {
        public EarthQuakeEntity(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public EarthQuakeEntity() {}

            public double mag { get; set; }
            public string place { get; set; }
            public object time { get; set; }
            public object updated { get; set; }
            public int tz { get; set; }
            public string url { get; set; }
            public string detail { get; set; }
            public int? felt { get; set; }
            public double? cdi { get; set; }
            public double mmi { get; set; }
            public string alert { get; set; }
            public string status { get; set; }
            public int tsunami { get; set; }
            public int sig { get; set; }
            public string net { get; set; }
            public string code { get; set; }
            public string ids { get; set; }
            public string sources { get; set; }
            public string types { get; set; }
            public int? nst { get; set; }
            public double dmin { get; set; }
            public double rms { get; set; }
            public int gap { get; set; }
            public string magType { get; set; }
            public string type { get; set; }
            public string title { get; set; }
    }
}
