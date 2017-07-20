using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Controllers
{
    [Authorize(Policy = "DriversOnly")]
    [Route("api/driver")]
    public class DriverController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            return Json("Hello driver");
        }
    }
}
