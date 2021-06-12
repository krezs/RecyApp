using Newtonsoft.Json;
using RecyApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecyApp.Services
{
    public class GoogleApiService
    {
        private string GoogleApiKey = "AIzaSyDYlE9XILNx0uR1NlJ8LdIREzP4RVplOEA";
        private JsonSerializer _serializer = new JsonSerializer();
        private static GoogleApiService _ServiceClientInstance;
        public static GoogleApiService ServiceClientInstance
        {
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new GoogleApiService();
                return _ServiceClientInstance;
            }
        }

        private HttpClient client;
        public GoogleApiService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originLatitude"></param>
        /// <param name="originLongitude"></param>
        /// <param name="destinationLatitude"></param>
        /// <param name="destinationLongitude"></param>
        /// <returns></returns>
        public async Task<GoogleDirection> GetDirections(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            GoogleDirection googleDirection = new GoogleDirection();
            var x = $"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={originLatitude},{originLongitude}&destination={destinationLatitude},{destinationLongitude}&key={GoogleApiKey}";
            Console.WriteLine(x);
            var response = await client.GetAsync($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={originLatitude},{originLongitude}&destination={destinationLatitude},{destinationLongitude}&key={GoogleApiKey}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    googleDirection = await Task.Run(() =>
                       JsonConvert.DeserializeObject<GoogleDirection>(json)
                    ).ConfigureAwait(false);

                }

            }

            return googleDirection;
        }
    }
}
