using Microsoft.AspNetCore.Mvc;

namespace PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Confirm()
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
