using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RecyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        public Map()
        {
            InitializeComponent();
            var mapPosition = new Position(-33.436761, -70.656800); // GEOLOCALIZACION
            //con la geo. tendremos el punto inicial


            var mapSpan = MapSpan.FromCenterAndRadius(mapPosition, Distance.FromKilometers(1));
            RecyMap.MoveToRegion(mapSpan);
            
            //consumir api
            //pintar puntos a la redonda desde la primera carga


            Pin pin = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.Place,
                Position = mapPosition
            };

            Pin pin2 = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.Place,
                Position = new Position(-33.443180, -70.653783)
            };

            RecyMap.Pins.Add(pin);
            RecyMap.Pins.Add(pin2);
        }
    }
}