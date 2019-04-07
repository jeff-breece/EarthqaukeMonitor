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
            var quakes = Task.FromResult(quakeTask);
            return quakes.ToString();
        }
    }
}
