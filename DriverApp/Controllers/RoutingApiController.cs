using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using DriverApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using DriverApp.Dtos;

namespace DriverApp.Controllers
{
	[Authorize(Policy = "AllUsers")]
	[Route("api/routing")]
    public class RoutingApiController : Controller
    {
        private CloudApi _cloudApi;
        private DbRepository _dbRepo;
        private ILogger _logger;

        public RoutingApiController(CloudApi cloudApi, DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
            _cloudApi = cloudApi;
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("RoutingApiLogger");
        }

        [HttpGet("orders")]
        public IEnumerable<Order> GetUnplannedOrders()
        {
            return _dbRepo.GetUnplannedOrders();
        }

        [HttpGet("orders/{id}")]
        public IEnumerable<Order> GetTripOrders(int id)
        {
            return _dbRepo.GetTripOrders(id);
        }

        [HttpPost("trips")]
        public IEnumerable<Trip> GetUnplannedOrders([FromBody] ReceiveTriggerRequestDto data)
        {
            string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            return _dbRepo.GetDriverTrips(customerKey);
        }

        [HttpPost("trigger")]
        public int TriggerRouting([FromBody] ReceiveTriggerRequestDto data)
        {
			string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
			string driverId = data.driverId;
			if (driverId == null) driverId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DriverId").Value;

			IEnumerable<Order> orders = _dbRepo.GetUnplannedOrders();
            if (orders.Any())
            {
                int planned = _dbRepo.InsertTrip(_cloudApi.TriggerRouting(orders).Result, customerKey, driverId);
				return planned;
            }

			return 0;
        }
    }
}