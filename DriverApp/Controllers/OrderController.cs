using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using DriverApp.Models;
using Microsoft.Extensions.Logging;
using static DriverApp.Services.CloudApi;
using DriverApp.Dtos;

namespace DriverApp.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private CloudApi _cloudApi;
        private DbRepository _dbRepo;
        private ILogger _logger;

        public OrderController(CloudApi cloudApi, DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
            _cloudApi = cloudApi;
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("LoginLogger");
        }

        [HttpGet()]
        public IEnumerable<Order> GetAvailableOrders()
        {
            return _dbRepo.GetAvailableOrders();
        }

        [HttpPost("trigger")]
        public StatusCodeResult TriggerRouting([FromBody] ReceiveDriverLoginDto data)
        {
            if (_dbRepo.IsValidLogin(data))
            {
                var query = _dbRepo.InsertTrip(_cloudApi.TriggerRouting(_dbRepo.GetAvailableOrders()).Result, data.driverId);
                if(query)
                {
                    return StatusCode(200);
                }
                return StatusCode(400);
            }
            return StatusCode(400);
        }
    }
}