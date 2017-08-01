using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DriverApp.Models;
using System;

namespace DriverApp.Controllers
{
    [Authorize(Policy = "ManagersOnly")]
    [Route("api/manager")]
    public class ManagerController : Controller
    {
        private DbRepository _dbRepo;
        private ILogger _logger;
		private ApiContext _db;

        public ManagerController(ApiContext db, DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
			_db = db;
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
		public JsonResult NewOrder([FromBody] Order order)
		{
			try
			{
				_db.Orders.Add(order);
				_db.SaveChanges();
				_db.Entry(order).GetDatabaseValues();
			} catch (Exception e)
			{
				_logger.LogError(e.Message);
				return Json(new { error = e.Message });
			}

			return Json(new { id = order.Id });
		}
    }
}