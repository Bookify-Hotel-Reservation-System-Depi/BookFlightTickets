using Microsoft.AspNetCore.Mvc;

namespace PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SeatController : Controller
    {
        public IActionResult Select()
        {
            return View();
        }

        public IActionResult Confirm()
        {
            return View();
        }
    }
}
