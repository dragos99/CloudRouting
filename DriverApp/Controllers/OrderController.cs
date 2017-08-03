using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using DriverApp.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using static DriverApp.Services.CloudApi;

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

        [HttpGet("trigger")]
        public string TriggerRouting()
        {
            TriggerRequest triggerRequest = new TriggerRequest();
            triggerRequest.RequestReference = 13243;
            triggerRequest.RequestParameters.Add(new Parameter { Name = "command", Value = "single-route" });
            triggerRequest.Data.Addresses.Add(new Address { Lat = 50.140964, Long = 8.662275, Id = "886627"});
            triggerRequest.Data.Addresses.Add(new Address { Lat = 50.167130, Long = 8.679496, Id = "1867807" });
            triggerRequest.Data.Addresses.Add(new Address { Lat = 50.133628, Long = 50.133628, Id = "3038836" });
            triggerRequest.Data.Addresses.Add(new Address { Lat = 50.118740, Long = 8.699856, Id = "depot" });
            triggerRequest.Data.Depots.Add(new Depot { AddressId = "depot", Id = "depot1" });
            triggerRequest.Data.Orders.Add(new RequestOrder { TimeWindowTill = "2015-04-14T15:00:00", TimeWindowFrom = "2015-04-14T11:00:00", FixedDurationInSec = 300, AddressId = "886627", Type = "delivery", Id = 9436340 });
            triggerRequest.Data.Orders.Add(new RequestOrder { TimeWindowTill = "2015-04-14T15:00:00", TimeWindowFrom = "2015-04-14T11:00:00", FixedDurationInSec = 600, AddressId = "1867807", Type = "delivery", Id = 9436343 });
            triggerRequest.Data.Orders.Add(new RequestOrder { TimeWindowTill = "2015-04-14T15:00:00", TimeWindowFrom = "2015-04-14T11:00:00", FixedDurationInSec = 300, AddressId = "3038836", Type = "delivery", Id = 9436347 });
            triggerRequest.Data.Routes.Add(new Route { Id = "302901" });
            return _cloudApi.TriggerRouting(triggerRequest);
        }
    }
}