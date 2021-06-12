using System;
using System.Collections.Generic;
using System.Text;

namespace RecyApp.Models
{
    public interface ILocationUpdateService
    {
        void GetUserLocation();
        event EventHandler<ILocationEventArgs> LocationChanged;
    }

    public interface ILocationEventArgs
    {
        double Latitude { get; set; }
        double Longiuted { get; set; }
    }
}
