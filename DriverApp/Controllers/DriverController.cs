using DriverApp.Models;
using DriverApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DriverApp.Controllers
{
    [Authorize(Policy = "DriversOnly")]
    [Route("api/driver")]
    public class DriverController : Controller
    {
        private DbRepository _dbRepo;

        public DriverController(DbRepository dbRepo)
        {
            _dbRepo = dbRepo;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json("Hello driver");
        }

        [HttpGet("trips")]
        public IEnumerable<Trip> GetDriverTrips()
        {
            string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            string driverId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DriverId").Value;
            return _dbRepo.GetDriverTrips(customerKey, driverId);
        }

        [HttpGet("orders")]
        public IEnumerable<Order> GetDriverOrders()
        {
            string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            string driverId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DriverId").Value;
            return _dbRepo.GetDriverOrders(customerKey, driverId);
        }
    }
}
