using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Earthquake.Function.Model;

namespace Earthquake.Function.Service
{
public class EarthQuakeRepository {
    HttpClient client = new HttpClient();
        public EarthQuakeRepository()
        {
             // client.BaseAddress = new Uri("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_month.geojson");
             // https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/significant_week.geojson
             client.BaseAddress = new Uri("https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/significant_week.geojson");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

// List<TheUser> friends = JsonConvert.DeserializeObject<List<TheUser>>(response);
        public async Task<Properties[]> GetAsync(CancellationToken cancellationToken)
        {
            List<Properties> earthQuakes = new List<Properties>();
            try {
                cancellationToken.ThrowIfCancellationRequested();
                HttpResponseMessage response = await client.GetAsync("", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rootObj = JsonConvert.DeserializeObject<RootObject>(stringResult);
                    
                    foreach(var row in rootObj.features)
                    {
                        var quake = row.properties;
                        earthQuakes.Add(quake);
                        Console.WriteLine($"Added quake [{quake.code}] to return object");
                    }
                }
                return earthQuakes.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw ex;
            }
        }
    }
}