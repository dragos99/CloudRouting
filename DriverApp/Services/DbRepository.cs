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
            Manager manager = _db.Managers.Where(m => m.CustomerKey == key).Include(m => m.Drivers).FirstOrDefault();
            return manager.Drivers;
        }

        public IEnumerable<Order> GetUnplannedOrders()
        {
            IEnumerable<Order> orders = _db.Orders.Where(m => m.TripId == 0);
            return orders;
        }

        public Driver GetDriver(string key, string id)
        {
            return _db.Drivers.Include(d => d.Manager).Where(d => d.DriverId == id && d.Manager.CustomerKey == key).FirstOrDefault();
        }

        public bool InsertTrip(TriggerResponse response, string customerKey, string driverId)
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

				List<Order> orders = GetUnplannedOrders().ToList();
				List<Stop> stops = response.OutputPlan.Routes[0].Stops;
				int lastid = _db.Trips.Last().Id + 1;

				foreach (var stop in stops)
				{
					var order = orders.Find(o => o.Id == stop.AddressId);
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
                return false;
            }


            return true;
        }
    }
}
