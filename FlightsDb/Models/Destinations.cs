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
        DBservices dBservices;
        public Destinations()
        {
             dBservices = new DBservices();
        }
        /// <summary>
        /// הכנסה לדטה בייס
        /// </summary>
        public void insertToDb(List<Destinations> destinations)
        {
            dBservices.insert(destinations);
        }

        internal List<Destinations> GetFromDBDestinations()
        {
           return dBservices.importData();
        }
    }
}