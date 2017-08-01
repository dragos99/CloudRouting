using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using DriverApp.Models;
using Microsoft.Extensions.Logging;

namespace DriverApp.Controllers
{
    [Produces("application/json")]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private DbRepository _dbRepo;
        private ILogger _logger;

        public OrderController(DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("LoginLogger");
        }

        [HttpGet()]
        public IEnumerable<Order> GetAvailableOrders()
        {
            return _dbRepo.GetAvailableOrders();
        }
    }
}