using Microsoft.AspNetCore.Mvc;

namespace PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }

    }
}
