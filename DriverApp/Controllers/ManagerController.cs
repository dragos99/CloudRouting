using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DriverApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DriverApp.Models;
using System;
using DriverApp.Dtos;

namespace DriverApp.Controllers
{
    [Authorize(Policy = "ManagersOnly")]
    [Route("api/manager")]
    public class ManagerController : Controller
    {
        private DbRepository _dbRepo;
        private ILogger _logger;
		private ApiContext _db;
		public CloudApi _cloudApi;

        public ManagerController(ApiContext db, DbRepository dbRepo, ILoggerFactory loggerFactory, CloudApi cloudApi)
        {
			_db = db;
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("ManagerLogger");
			_cloudApi = cloudApi;
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
            string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            var drivers = _dbRepo.GetDrivers(customerKey);

            return Mapper.Map<IEnumerable<SendDriverDto>>(drivers);
        }

		[HttpPost("newOrders")]
		public JsonResult NewOrder([FromBody] ReceiveOrdersDto data)
		{
			try
			{
				foreach (var order in data.orders)
				{
					_db.Orders.Add(order);
				}

				_db.SaveChanges();
			} catch (Exception e)
			{
				_logger.LogError(e.Message);
				return Json(new { error = e.Message });
			}

			return Json(new { id = _db.Orders.Last().Id });
		}

		[HttpPost("assignOrders")]
		public JsonResult AssignOrders([FromBody] ReceiveOrdersAssignationDto data)
		{
			try
			{
				foreach (var orderId in data.orders)
				{
					var order = _db.Orders.Where(o => o.Id == orderId).FirstOrDefault();
					order.DriverId = data.driverId;
				}

				_db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return Json(new { error = e.Message });
			}

			return Json("ok");
		}

        [HttpGet("trips")]
        public IEnumerable<Trip> GetTrips()
        {
            string customerKey = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "CustomerKey").Value;
            return _dbRepo.GetTrips(customerKey);
        }

    }
}