using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlightsDb.Models;

namespace FlightsDb.Controllers
{
    public class FlightController : ApiController
    {
        Flights flightObj = new Flights();

        // GET api/values
        public List<Flights> Get()
        {
            return flightObj.getFlightList();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route ("api/flight/stop/{city}")]
        public List<Flights> getFilteredRoutes(string city)
        {
            return flightObj.getFilteredConnection(city);//שולח עיר לסינון
            
        }

        // POST api/values
        public void Post([FromBody]Flights flight)
        {
            //Flights.flightsList.Add(flight);
            flightObj.SaveFlight(flight);
        }
        /// <summary>
        /// שמירת יעדים בדטה בייס
        /// </summary>
        [HttpPost]
        [Route("api/flight/destination")]
        public void postDestinations([FromBody]List<Destinations> destinations)
        {
            Destinations d = new Destinations();
            d.insertToDb(destinations);
        }

        //מקבל מהדטה בייס את הלוקיישנים
        [HttpGet]
        [Route("api/flight/Getdestination")]
        public List<Destinations> GetFromDBDestinations()//
        {
            Destinations destinations = new Destinations();
            List<Destinations> destinationsList = new List<Destinations>();
            destinationsList= destinations.GetFromDBDestinations();
            return destinationsList;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
