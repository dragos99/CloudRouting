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

        public IEnumerable<Order> GetAvailableOrders()
        {
            IEnumerable<Order> orders = _db.Orders.Where(m => m.Complete == false && m.TripId == 0);
            return orders;
        }

        public Driver GetDriver(string key, string id)
        {
            return _db.Drivers.Include(d => d.Manager).Where(d => d.DriverId == id && d.Manager.CustomerKey == key).FirstOrDefault();
        }

        public string InsertTrip(TriggerResponse response, string driverId)
        {
            Trip trip = new Trip();
            //trip.Id = 1;
            trip.AccountId = Int32.Parse(driverId);
            trip.DriverId = Int32.Parse(driverId);
            trip.AvailableFromTime = response.OutputPlan.Routes[0].StartDateTime;
            trip.AvailableTillTime = response.OutputPlan.Routes[0].FinishDateTime;
            trip.StartTime = response.OutputPlan.Routes[0].StartDateTime;
            trip.FinishTime = response.OutputPlan.Routes[0].FinishDateTime;
            trip.TotalDistanceInKm = response.OutputPlan.Routes[0].Distance;
            trip.TotalDurationInSec = response.OutputPlan.Routes[0].DurationInSec;
            _db.Trips.Add(trip);
            _db.SaveChanges();
            return null;
        }
    }
}
