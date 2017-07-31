using DriverApp.Models;
using DriverApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DriverApp.Controllers
{
    [AllowAnonymous]
    [Route("api/login")]
    public class LoginController : Controller
    {
        private DbRepository _dbRepo;
        private ILogger _logger;

        public LoginController(DbRepository dbRepo, ILoggerFactory loggerFactory)
        {
            _dbRepo = dbRepo;
            _logger = loggerFactory.CreateLogger("LoginLogger");
        }


		/* Routes */
        [HttpPost("manager")]
        public async Task<StatusCodeResult> ManagerLogin([FromBody] ReceiveManagerLoginDto data)
        {
            if (data == null) return StatusCode(400);

            string customerKey = data.customerKey;
            if (string.IsNullOrEmpty(customerKey)) return StatusCode(400);

            Manager acc = _dbRepo.GetManager(customerKey);
            if (acc == null) return StatusCode(404);

            var claims = new[] {
                new Claim("CustomerKey", customerKey),
                new Claim("Role", "Manager")
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.Authentication.SignInAsync("CookieAuth", principal);

            return StatusCode(200);
        }

        [HttpPost("driver")]
        public async Task<StatusCodeResult> DriverLogin([FromBody] ReceiveDriverLoginDto data)
        {
            string customerKey = data.customerKey;
            string driverId = data.driverId;
            if (string.IsNullOrEmpty(customerKey) || string.IsNullOrEmpty(driverId)) return StatusCode(400);

            Driver acc = _dbRepo.GetDriver(customerKey, driverId);
            if (acc == null) return StatusCode(404);

            var claims = new[] {
                new Claim("CustomerKey", customerKey),
                new Claim("DriverId", driverId),
                new Claim("Role", "Driver")
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.Authentication.SignInAsync("CookieAuth", principal);

            return StatusCode(200);
        }

        [HttpGet("logout")]
        public async Task<StatusCodeResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuth");
            return StatusCode(200);
        }

    }
}
