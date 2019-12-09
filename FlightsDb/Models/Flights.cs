using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightsDb.Models
{
    public class Flights
    {
        public static List<Flights> flightsList = new List<Flights>();
        public static List<Flights> fiteredConnectionList = new List<Flights>();
        public string cityFrom { get; set; }
        public string cityTo { get; set; }
        public string dateFrom { get; set; }
        public string dateUntil { get; set; }
        public double Price { get; set; }
        public List<string> Routes { get; set; }

        internal List<Flights> getFlightList()
        {
            return flightsList;
        }

   

        public Flights()
        {
        }

        public List<Flights> getFilteredConnection(string City)
        {
            fiteredConnectionList = flightsList.Where(x => x.Routes.Contains(City)).ToList();
            return fiteredConnectionList;
        }
      
    }
}