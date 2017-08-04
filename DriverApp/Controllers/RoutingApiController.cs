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
            _logger = loggerFactory.CreateLogger("LoginLogger");
        }

        [HttpGet("orders")]
        public IEnumerable<Order> GetUnplannedOrders()
        {
            return _dbRepo.GetUnplannedOrders();
        }

        [HttpPost("trigger")]
        public StatusCodeResult TriggerRouting([FromBody] ReceiveTriggerRequestDto data)
        {
			string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
			string driverId = data.driverId;
            IEnumerable<Order> orders = _dbRepo.GetUnplannedOrders();
            if (orders.Any())
            {
                if (driverId == null) driverId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DriverId").Value;

                bool success = _dbRepo.InsertTrip(_cloudApi.TriggerRouting(orders).Result, customerKey, driverId);
                if (success)
                {
                    return StatusCode(200);
                }
                return StatusCode(400);
            }
			return StatusCode(400);
        }
    }
}