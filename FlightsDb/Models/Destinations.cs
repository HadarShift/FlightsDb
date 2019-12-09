using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightsDb.Models
{
    public class Destinations
    {
        public string City { get; set; }
        public string Code { get; set; }
        public double LenLat { get; set; }
        public double LenLon { get; set; }

    }
}