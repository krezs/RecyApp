using Plugin.Geolocator;
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
        List<RecyclingPoint> RecyclingPoints;
        public Map()
        {
            InitializeComponent();

            service = new PuntosLimpiosService();
            RecyclingPoints = new List<RecyclingPoint>();
            InitMap();
            StartListening();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void InitMap()
        {
            try
            {
                //current geolocalitation
                //Location currentLocation = new Location(-33.444302, -70.653415); // fake location
                Location currentLocation = await GetLocationAsync(); // get real smartphone location

                //Android.Locations.LocationManager locationManager = (Android.Locations.LocationManager)GetSystemService(Context.LocationService);

                //setting map location to current localitation
                var mapPosition = new Position(currentLocation.Latitude, currentLocation.Longitude);
                var mapSpan = MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMeters(500));
                RecyMap.MoveToRegion(mapSpan);

                BuildNearbyPoint(currentLocation.Latitude, currentLocation.Longitude, 2);

               
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void BuildNearbyPoint(double lat, double lng, double distance)
        {
            try
            {
                List<RecyclingPoint> recyclingPoints = await service.GetNearbyPoints(lat, lng, distance);
                //if (RecyclingPoints.Count == 0) RecyclingPoints = recyclingPoints;
                //List<RecyclingPoint> pointsToAdd = new List<RecyclingPoint>();

                foreach (var item in recyclingPoints)
                {
                    var existingPoint = RecyclingPoints.Where(x => x.lat == item.lat && x.lng == item.lng).FirstOrDefault();
                    Pin pin = new Pin
                    {
                        Label = item.manager,
                        Address = item.address_name,
                        Type = PinType.Place,
                        Position = new Position(item.lat, item.lng)
                    };
                    if (existingPoint == null)
                    {
                        Console.WriteLine("Adding " + item.address_name);
                        pin.MarkerClicked += Pin_MarkerClicked;
                        RecyclingPoints.Add(item);
                        RecyMap.Pins.Add(pin);
                    }
                }


                var itemsToRemove = RecyclingPoints.Except(recyclingPoints);

                foreach (var item in itemsToRemove)
                {
                    var existingPoint = RecyclingPoints.Where(x => x.lat == item.lat && x.lng == item.lng).FirstOrDefault();
                    if (existingPoint != null)
                    {
                        Console.WriteLine("Removing " + item.address_name);
                        Pin pin = new Pin
                        {
                            Label = item.manager,
                            Address = item.address_name,
                            Type = PinType.Place,
                            Position = new Position(item.lat, item.lng)
                        };
                        RecyclingPoints.Remove(item);
                        RecyMap.Pins.Remove(pin);
                    }
                }


                //foreach (RecyclingPoint item in recyclingPoints)
                //{
                //    Pin pin = new Pin
                //    {
                //        Label = item.manager,
                //        Address = item.address_name,
                //        Type = PinType.Place,
                //        Position = new Position(item.lat, item.lng)
                //    };
                //    pin.MarkerClicked += async (s, args) =>
                //    {
                //        args.HideInfoWindow = true;
                //        string pinName = ((Pin)s).Label;
                //        await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
                //    };

                //    RecyMap.Pins.Add(pin);
                //}
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Pin_MarkerClicked(object sender, PinClickedEventArgs e)
        {
            e.HideInfoWindow = true;
            string pinName = ((Pin)sender).Label;
            await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        private async Task<System.Collections.Generic.List<Position>> NavigateToPoint(Position position)
        {
            try
            {
                var location = new Location(position.Latitude, position.Longitude);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Walking };

                var googleDirection = await GoogleApiService.ServiceClientInstance.GetDirections(-33.444302, -70.653415, position.Latitude, position.Longitude);
                if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                {
                    var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                    return positions;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Add your payment method inside the Google Maps console.", "Ok");
                    return null;

                }
                // await RecyMap.OpenAsync(location, options);
                //await Map.OpenAsync(location, options);
            }
            catch (Exception)
            {

                throw;
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

        async Task StartListening()
        {
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });

            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var test = e.Position;
            Console.WriteLine("Full: Lat: " + test.Latitude.ToString() + " Long: " + test.Longitude.ToString());
            Console.WriteLine($"Time: {test.Timestamp.ToString()}");
            Console.WriteLine($"Heading: {test.Heading.ToString()}");
            Console.WriteLine($"Speed: {test.Speed.ToString()}");
            Console.WriteLine($"Accuracy: {test.Accuracy.ToString()}");
            Console.WriteLine($"Altitude: {test.Altitude.ToString()}");
            Console.WriteLine($"AltitudeAccuracy: {test.AltitudeAccuracy.ToString()}");

            BuildNearbyPoint(test.Latitude, test.Longitude, 2);

        }
    }
}