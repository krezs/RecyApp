using System;
using System.Collections.Generic;
using System.Text;

namespace RecyApp.Models
{
    public class RecyclingPoint
    {
        public double distance { get; set; }
        public double lat { get; set; }
        public string status { get; set; }
        public double lng { get; set; }
        public string owner { get; set; }
        public string type { get; set; }
        public string address_type { get; set; }
        public string manager { get; set; }
        public string address_number { get; set; }
        public string address_name { get; set; }
        public Region region { get; set; }
        public Commune commune { get; set; }
        public List<string> materials { get; set; }
    }
}
