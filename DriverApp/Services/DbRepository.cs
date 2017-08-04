using DriverApp.Dtos;
using DriverApp.Dtos.CloudDtos;
using DriverApp.Models;
using Microsoft.EntityFrameworkCore;
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

        public DbRepository(ApiContext db)
        {
            _db = db;
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
					AccountId = Int32.Parse(customerKey),
					DriverId = Int32.Parse(driverId),
					AvailableFromTime = response.OutputPlan.Routes[0].StartDateTime,
					AvailableTillTime = response.OutputPlan.Routes[0].FinishDateTime,
					StartTime = response.OutputPlan.Routes[0].StartDateTime,
					FinishTime = response.OutputPlan.Routes[0].FinishDateTime,
					TotalDistanceInKm = response.OutputPlan.Routes[0].Distance,
					TotalDurationInSec = response.OutputPlan.Routes[0].DurationInSec
				};

                _db.Trips.Add(trip);

                IEnumerable<Order> orders = GetUnplannedOrders();
                foreach(var order in orders)
				{ 
                    order.TripId = _db.Trips.Max(t => t.Id) + 1;
                }

                _db.SaveChanges();
            }
            catch(Exception e)
            {
				Console.WriteLine("Insert Trip exception " + e.Message);
                return false;
            }

            return true;
        }
    }
}
