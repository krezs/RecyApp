using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RecyApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            var mapPosition = new Position(-33.436761, -70.656800);
            var mapSpan = MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(2));
            MyMap.MoveToRegion(mapSpan);
        }
    }
}