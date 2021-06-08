using RecyApp.Models;
using RecyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RecyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {

        PuntosLimpiosService service;
        public Map()
        {
            InitializeComponent();

            service = new PuntosLimpiosService();
            InitMap();

        }

        /// <summary>
        /// 
        /// </summary>
        private async void InitMap()
        {
            try
            {
                //current geolocalitation
                Location currentLocation = await GetLocationAsync();
                //setting map location to current localitation
                var mapPosition = new Position(currentLocation.Latitude, currentLocation.Longitude);
                var mapSpan = MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMeters(500));
                RecyMap.MoveToRegion(mapSpan);
                
                //consumir api
                //pintar puntos a la redonda desde la primera carga


                Pin pin = new Pin
                {
                    Label = "Posicion de carga",
                    Address = "test",
                    Type = PinType.Place,
                    Position = mapPosition
                };


                BuildNearbyPoint(currentLocation.Latitude, currentLocation.Longitude, 2);

                


                //Pin pin2 = new Pin
                //{
                //    Label = "Santa Cruz",
                //    Address = "The city with a boardwalk",
                //    Type = PinType.Place,
                //    Position = new Position(-33.443180, -70.653783)
                //};

                RecyMap.Pins.Add(pin);
                //RecyMap.Pins.Add(pin2);
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void BuildNearbyPoint(double lat, double lng, int distance)
        {
            try
            {
                List<RecyclingPoint> recyclingPoints = await service.GetNearbyPoints(lat, lng, distance);
                foreach (RecyclingPoint item in recyclingPoints)
                {
                    Pin pin = new Pin
                    {
                        Label = item.manager,
                        Address = item.address_name,
                        Type = PinType.Place,
                        Position = new Position(item.lat, item.lng)
                    };
                    RecyMap.Pins.Add(pin);
                }
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<Location> GetLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;
        }
    }
}