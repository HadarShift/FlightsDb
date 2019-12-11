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
        public string FlightId { get; set; }
        public string cityFrom { get; set; }
        public string cityTo { get; set; }
        public string dateFrom { get; set; }
        public string dateUntil { get; set; }
        public double Price { get; set; }
        public List<string> Routes { get; set; }

        internal List<Flights> getFlightList()
        {
            DBservices dBservices = new DBservices();
            flightsList= dBservices.ReturnFlightsChosen();
            return flightsList;
        }

   

        public Flights()
        {
        }

        /// <summary>
        ///  קבלת טיסות מסוננות לפי קונקשיין
        /// </summary>
        public List<Flights> getFilteredConnection(string City)
        {
            fiteredConnectionList = flightsList.Where(x => x.Routes.Contains(City)).ToList();
            return fiteredConnectionList;
        }

        //שמירת טיסה רצויה בדטה בייס
        internal void SaveFlight(Flights flights)
        {
            DBservices dBservices = new DBservices();
            dBservices.SaveFlight(flights);
        }
    }
}