using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace DriverApp.Controllers
{
    [Authorize(Policy = "ManagersOnly")]
    [Route("api/manager")]
    public class ManagerController : Controller
    {
        private DbRepository _dbRepo;
        private ILogger _logger;

        public ManagerController(DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("ManagerLogger");
        }


        /* Routes */
        [HttpGet]
        public JsonResult Index()
        {
            return Json("salut");
        }

        [HttpGet("drivers")]
        public IEnumerable<SendDriverDto> GetDrivers()
        {
            var customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            var drivers = _dbRepo.GetDrivers(customerKey);

            return Mapper.Map<IEnumerable<SendDriverDto>>(drivers);
        }

		[HttpPost("newOrder")]
		public StatusCodeResult NewOrder([FromBody] ReceiveOrderDto order)
		{

			return Ok();
		}
    }
}