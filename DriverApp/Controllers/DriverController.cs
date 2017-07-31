using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
