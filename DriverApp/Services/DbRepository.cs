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

        public IEnumerable<Order> GetUnplannedOrders()
        {
            IEnumerable<Order> orders = _db.Orders.Where(m => m.TripId == 0);
            return orders;
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

				List<Order> orders = GetUnplannedOrders().ToList();
				List<Stop> stops = response.OutputPlan.Routes[0].Stops;
				var last = _db.Trips.Last();
				int lastid = (last == null) ? 1 : last.Id + 1;

				foreach (var stop in stops)
				{
                    var order = orders.Find(o => o.Id.ToString() == stop.AddressId);
					if (order == null) continue;
					order.TripId = lastid;
					order.StopSequence = stop.StopSequence;
					order.ArrivalDateTime = stop.ArrivalDateTime;
					order.DepartureDateTime = stop.DepartureDateTime;
					order.Distance = stop.Distance;
				}

				_db.Trips.Add(trip);
				_db.SaveChanges();
            }
            catch(Exception e)
            {
				_logger.LogInformation("Insert Trip exception " + e.Message);
                return 0;
            }

            return response.OutputPlan.Routes[0].Stops.Count;
        }
    }
}
