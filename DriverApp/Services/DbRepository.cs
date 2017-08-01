using DriverApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            //return GetManager(key).Drivers;
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
    }
}
