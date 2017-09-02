using DriverApp.Dtos;
using DriverApp.Dtos.CloudDtos;
using DriverApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DriverApp.Services.CloudApi;

namespace DriverApp.Services
{
    public class DbRepository
    {
        private ApiContext _db;
		private ILogger _logger;

        public DbRepository(ApiContext db, ILoggerFactory fact)
        {
            _db = db;
			_logger = fact.CreateLogger("DbRepoLogger");
        }

        public Manager GetManager(string key)
        {
            return _db.Managers.Where(m => m.CustomerKey == key).FirstOrDefault();
        }

        public IEnumerable<Driver> GetDrivers(string key)
        {
            return _db.Drivers.Where(d => d.CustomerKey == key).ToList();
        }

        public IEnumerable<Order> GetUnassignedOrders()
        {
            IEnumerable<Order> orders = _db.Orders.Where(m => m.DriverId == null);
            return orders;
        }

        public IEnumerable<Order> GetDriverOrders(string customerKey, string driverId)
        {
            IEnumerable<Order> orders = _db.Orders.Where(o => o.AccountId == customerKey && o.DriverId == driverId);
            return orders;
        }

        public IEnumerable<Order> GetTripOrders(int tripid)
        {
            IEnumerable<Order> trips = _db.Orders.Where(m => m.TripId == tripid);
            return trips;
        }

        public IEnumerable<Trip> GetTrips(string customerKey)
        {
            IEnumerable<Trip> trips = _db.Trips.Where(m => m.AccountId == customerKey);
            return trips;
        }

        public IEnumerable<Trip> GetDriverTrips(string customerKey, string driverId)
        {
            IEnumerable<Trip> trips = _db.Trips.Where(t => t.AccountId == customerKey && t.DriverId == driverId);
            return trips;
        }

        public string SetOrderComplete(int orderId, bool isComplete)
        {
            _logger.LogInformation("Request sent! Order ID: " + orderId + ", isComplete: " + isComplete.ToString());
            _db.Orders.Where(d => d.Id == orderId).FirstOrDefault().Complete = isComplete;
            _db.SaveChanges();
            return null;

        }

        public Driver GetDriver(string key, string id)
        {
            return _db.Drivers.Where(d => d.DriverId == id && d.CustomerKey == key).FirstOrDefault();
        }

        public int InsertTrip(TriggerResponse response, string customerKey, string driverId)
        {
            try
            {
				var trip = new Trip {
					AccountId = customerKey,
					DriverId = driverId,
					AvailableFromTime = DateTime.Parse(response.OutputPlan.Routes[0].StartDateTime),
					AvailableTillTime = DateTime.Parse(response.OutputPlan.Routes[0].FinishDateTime),
					StartTime = DateTime.Parse(response.OutputPlan.Routes[0].StartDateTime),
					FinishTime = DateTime.Parse(response.OutputPlan.Routes[0].FinishDateTime),
					TotalDistanceInKm = response.OutputPlan.Routes[0].Distance,
					TotalDurationInSec = response.OutputPlan.Routes[0].DurationInSec,
					NOfStops = response.OutputPlan.Routes[0].NofStops
				};

				_db.Trips.Add(trip);

				if (trip.NOfStops == 0) return 0;

				List<Order> orders = GetDriverOrders(customerKey, driverId).ToList();
				List<Stop> stops = response.OutputPlan.Routes[0].Stops;
				int lastid = _db.Trips.Last().Id + 1;
                int k = 0;

				foreach (var stop in stops)
				{
                    var order = orders.Find(o => o.Id.ToString() == stop.AddressId);
					if (order == null) continue;
					order.TripId = lastid;
					order.StopSequence = stop.StopSequence;
					order.ArrivalDateTime = stop.ArrivalDateTime;
					order.DepartureDateTime = stop.DepartureDateTime;
					order.Distance = stop.Distance;
                    order.OrderNumber = (++k).ToString();
                    //order.GeoX = stop.
				}

				_db.SaveChanges();
            }
            catch(Exception e)
            {
				_logger.LogInformation("Insert Trip exception " + e.Message);
                return 0;
            }

            return response.OutputPlan.Routes[0].Stops.Count;
        }
        public List<Order> UpdateTrip(TriggerResponse response, string customerKey, string driverId, int tripId)
        {
            List<Order> orders = new List<Order>();
            try
            {
                var trip = new Trip
                {
                    AccountId = customerKey,
                    DriverId = driverId,
                    AvailableFromTime = DateTime.Parse(response.OutputPlan.Routes[0].StartDateTime),
                    AvailableTillTime = DateTime.Parse(response.OutputPlan.Routes[0].FinishDateTime),
                    StartTime = DateTime.Parse(response.OutputPlan.Routes[0].StartDateTime),
                    FinishTime = DateTime.Parse(response.OutputPlan.Routes[0].FinishDateTime),
                    TotalDistanceInKm = response.OutputPlan.Routes[0].Distance,
                    TotalDurationInSec = response.OutputPlan.Routes[0].DurationInSec,
                    NOfStops = response.OutputPlan.Routes[0].NofStops
                };

                orders = GetTripOrders(tripId).ToList();
                List<Stop> stops = response.OutputPlan.Routes[0].Stops;
                int k = 0;

                foreach (var stop in stops)
                {
                    var order = orders.Find(o => o.Id.ToString() == stop.AddressId);
                    if (order == null) continue;
                    order.StopSequence = stop.StopSequence;
                    order.ArrivalDateTime = stop.ArrivalDateTime;
                    order.DepartureDateTime = stop.DepartureDateTime;
                    order.Distance = stop.Distance;
                    order.OrderNumber = (++k).ToString();
                    //order.GeoX = stop.
                }

                _db.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Update Trip exception " + e.Message);
                return new List<Order>();
            }

            return orders;
        }
    }
}
