using Newtonsoft.Json;
using RecyApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecyApp.Services
{
    public class PuntosLimpiosService //: IRestService
    {
        HttpClient client;
        HttpClientHandler clientHandler = new HttpClientHandler();

        public PuntosLimpiosService()
        {
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="distance"></param>
        public async Task<List<RecyclingPoint>> GetNearbyPoints(double latitude, double longitude, int distance)
        {
            try
            {
                //var x = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                var url = $"https://puntoslimpios.mma.gob.cl/api/points/geo?lat={latitude}&lng={longitude}&distance=10";
                //puntoslimpios.mma.gob.cl/api/points/geo?lat=-33.4405632&lng=-70.6614779&distance=20
                Uri uri = new Uri(string.Format(url, string.Empty));
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    // List<RecyclingPoint> recyclingPoints = JsonSerializer.d<List<RecyclingPoint>>(content, serializerOptions);
                    List<RecyclingPoint> recyclingPoints = JsonConvert.DeserializeObject<List<RecyclingPoint>>(content);
                    return recyclingPoints;
                }else
                {
                    return new List<RecyclingPoint>();
                }
            }
            catch (Exception ex)
            {

                return new List<RecyclingPoint>();
            }
        }
    }
}
