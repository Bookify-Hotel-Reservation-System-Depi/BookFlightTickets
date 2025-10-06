using Microsoft.AspNetCore.Mvc;
using Utility;

namespace PL.Areas.Customer.Controllers
{
    [Area(SD.Customer)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
