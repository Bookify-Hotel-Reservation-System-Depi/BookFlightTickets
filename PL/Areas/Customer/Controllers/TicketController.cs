using Microsoft.AspNetCore.Mvc;

namespace PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Download()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
