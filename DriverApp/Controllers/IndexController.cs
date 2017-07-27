using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DriverApp.Controllers
{
	[Route("/")]
    public class IndexController : Controller
    {
		[Route("")]
		[Route("drivers")]
		[Route("trips")]
		[Route("orders")]
		public IActionResult Index()
        {
            return View();
        }
	}
}